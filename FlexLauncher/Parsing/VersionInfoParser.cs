using System.Diagnostics;
using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public delegate bool ProfileJsonProvider(string name, out JObject? value);

public static class VersionInfoParser
{
    public static VersionInfo FromFile(string path)
    {
        var directory = Path.GetDirectoryName(path);
        var name = Path.GetFileNameWithoutExtension(path);
        return ReadJson(name, (string s, out JObject? value) =>
        {
            var jsonPath = Path.Combine(directory, $"{s}.json");
            if (!File.Exists(jsonPath))
            {
                value = null;
                return false;
            }
            
            var json = JObject.Parse(File.ReadAllText(jsonPath));
            value = json;
            return true;
        });
    }
    
    public static VersionInfo ParseJson(JObject json)
    {
        var id = json["name"]!.Value<string>()!;
        return ReadJson(id, (string s, out JObject? value) =>
        {
            value = null;
            if (s != id)
                return false;
            value = json;
            return true;
        });
    }

    public static VersionInfo ReadJson(string name, ProfileJsonProvider provider)
    {
        var json = GetJson(name, provider);

        var type = json["type"]!.Value<string>()! switch
        {
            "release" => VersionType.Release,
            "snapshot" => VersionType.Snapshot,
            "alpha" => VersionType.Alpha,
            "beta" => VersionType.Beta,
            "modded" => VersionType.Modded,
            _ => VersionType.Unknown
        };
        var mainClass = json["main_class"]!.Value<string>()!;

        var assetIndex = AssetIndexInfoParser.ParseJson(json["asset_index"]!.Value<JObject>()!);
        var mainJar = DownloadInfoParser.ParseJson(json["main_jar"]!.Value<JObject>()!);
        var libraries = json["libraries"]!
            .Cast<JObject>()
            .Select(LibraryParser.ParseJson);
        var jvmArguments = json["jvm_arguments"]!
            .Cast<JObject>()
            .Select(ArgumentListParser.ParseJson);
        var gameArguments = json["game_arguments"]!
            .Cast<JObject>()
            .Select(ArgumentListParser.ParseJson);

        return new VersionInfo(name, type, mainClass, assetIndex, mainJar, libraries, jvmArguments, gameArguments);
    }

    private static JObject GetJson(string name, ProfileJsonProvider provider)
    {
        if (!provider(name, out var json))
            throw new ArgumentException($"Profile '{name}' does not exist!");
        
        // Json is definitely not null here, but the compiler doesn't know that
        Debug.Assert(json != null);

        // We need to resolve the "inherits" property
        if (json.TryGetValue("inherits", out var inheritToken))
        {
            var inheritName = inheritToken.Value<string>()!;
            var baseJson = GetJson(inheritName, provider);
            
            // Merge the base and the inherited
            json = MergeJson(baseJson, json);
        }

        return json;
    }

    private static JObject MergeJson(JObject baseJson, JObject inheritJson)
    {
        var json = new JObject();
        
        // Add all properties from the base
        foreach (var property in baseJson.Properties())
            json.Add(property);

        // Add all properties from the inherited
        // Handle special cases for arrays and objects types
        foreach (var property in inheritJson.Properties())
        {
            var name = property.Name;
            var value = property.Value;
            
            if (json.TryGetValue(name, out var baseValue))
            {
                if (baseValue is JArray baseArray && value is JArray inheritArray)
                {
                    // Merge the arrays
                    var newArray = new JArray();
                    foreach (var item in baseArray)
                        newArray.Add(item);
                    foreach (var item in inheritArray)
                        newArray.Add(item);
                    
                    json[name] = newArray;
                }
                else if (baseValue is JObject baseObject && value is JObject inheritObject)
                {
                    // Merge the objects
                    json[name] = MergeJson(baseObject, inheritObject);
                }
                else
                {
                    // Overwrite the value
                    json[name] = value;
                }
            }
            else
            {
                // Add the value
                json[name] = value;
            }
        }
        
        return json;
    }
}
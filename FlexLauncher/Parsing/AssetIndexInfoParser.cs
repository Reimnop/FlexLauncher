using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public static class AssetIndexInfoParser
{
    public static AssetIndexInfo ParseJson(JObject json)
    {
        var name = json["name"]!.Value<string>()!;
        var url = json["url"]!.Value<string>()!;
        return new AssetIndexInfo(name, url);
    }
}
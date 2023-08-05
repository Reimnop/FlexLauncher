using System.Diagnostics;
using FlexLauncher.Core;
using FlexLauncher.Data;
using FlexLauncher.Parsing;
using FlexLauncher.Util;
using Newtonsoft.Json.Linq;

namespace FlexLauncher;

public class Launcher
{
    private readonly LaunchContext context;

    public Launcher(LaunchContext context)
    {
        this.context = context;
    }

    public GameInstance GetGameInstance()
    {
        // Launch game
        var process = new Process();
        process.StartInfo.FileName = context.Preferences.JavaPath;
        process.StartInfo.WorkingDirectory = context.Preferences.WorkingDirectory;
        
        // Add launch arguments
        foreach (var argument in GetLaunchArguments())
        {
            process.StartInfo.ArgumentList.Add(argument);
        }

        return new GameInstance(context.Version, context.Preferences, process);
    }

    private IEnumerable<string> GetLaunchArguments()
    {
        // Add JVM arguments
        foreach (var argument in context.Version.JvmArguments.SelectMany(x => x.EnumerateArguments(context)))
            yield return argument;

        // Add main class
        yield return context.Version.MainClass;

        // Add game arguments
        foreach (var argument in context.Version.GameArguments.SelectMany(x => x.EnumerateArguments(context)))
            yield return argument;
    }

    public async Task InstallVersionAsync(IProgress<DownloadProgress>? progress = null)
    {
        var jarDownloadInfos = GetJarDownloadInfos();
        var assetDownloadInfos = await GetAssetDownloadInfosAsync();
        var files = jarDownloadInfos
            .Concat(assetDownloadInfos)
            .Where(x => !File.Exists(x.Path)); // Strip existing files

        // Download files
        var downloader = new FileDownloader(files);
        await downloader.DownloadAsync(progress);
    }

    private IEnumerable<FileDownloadInfo> GetJarDownloadInfos()
    {
        // Get libraries
        var libraryDownloadInfos = context.Version.Libraries
            .Select(x => x.GetDownload(context))
            .OfType<DownloadInfo>() // Get rid of the libraries that don't have a download
            .Select(x => new FileDownloadInfo(x.Url, Path.Combine(context.Paths.LibrariesPath, x.Path)));
        
        foreach (var downloadInfo in libraryDownloadInfos)
            yield return downloadInfo;
        
        // Get main jar
        yield return new FileDownloadInfo(
            context.Version.MainJar.Url,
            Path.Combine(context.Paths.VersionsPath, context.Version.MainJar.Path));
    }

    private async Task<IEnumerable<FileDownloadInfo>> GetAssetDownloadInfosAsync()
    {
        // Get asset index
        var assetIndex = await GetAssetIndex();
        
        // Get assets list from asset index
        var assets = assetIndex["objects"]!
            .Children()
            .Select(x => x.Value<JProperty>())
            .OfType<JProperty>()
            .Select(x => x.Value)
            .OfType<JObject>()
            .Select(AssetParser.ParseJson);
        
        // Get asset download infos
        return EnumerateAssetDownloadInfos(assets);
    }

    private async Task<JObject> GetAssetIndex()
    {
        var indexesDir = context.Paths.AssetIndexesPath;
        var indexesPath = Path.Combine(indexesDir, $"{context.Version.AssetIndex.Name}.json");
        
        // If it already exists, return it
        if (File.Exists(indexesPath))
        {
            var str = await File.ReadAllTextAsync(indexesPath);
            return JObject.Parse(str);
        }
        else
        {
            // Download asset index
            var str = await WebHelper.Fetch(context.Version.AssetIndex.Url);
            Directory.CreateDirectory(indexesDir);
        
            // Write asset index to file
            await File.WriteAllTextAsync(indexesPath, str);
            
            // Return asset index
            return JObject.Parse(str);
        }
    }

    private IEnumerable<FileDownloadInfo> EnumerateAssetDownloadInfos(IEnumerable<Asset> assets)
    {
        foreach (var asset in assets)
        {
            var assetGroup = asset.Hash.Substring(0, 2);
            var assetPath = Path.Combine(context.Paths.AssetObjectsPath, assetGroup, asset.Hash);
            var url = $"https://resources.download.minecraft.net/{assetGroup}/{asset.Hash}";
            yield return new FileDownloadInfo(url, assetPath);
        }
    }
}
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

    public async Task<GameInstance> GetGameInstanceAsync()
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
        foreach (var argumentList in context.Version.JvmArguments)
        {
            foreach (var argument in argumentList.EnumerateArguments(context))
            {
                yield return argument;
            }
        }
        
        yield return context.Version.MainClass;
        
        foreach (var argumentList in context.Version.GameArguments)
        {
            foreach (var argument in argumentList.EnumerateArguments(context))
            {
                yield return argument;
            }
        }
    }
    
    public async Task InstallVersionAsync()
    {
        var jarDownloadInfos = GetJarDownloadInfos();
        var assetDownloadInfos = await GetAssetDownloadInfosAsync();
        var files = jarDownloadInfos
            .Concat(assetDownloadInfos)
            .Where(x => !File.Exists(x.Path)); // Strip existing files

        // Download files
        var downloader = new FileDownloader(files);
        await downloader.DownloadAsync();
    }

    private IEnumerable<FileDownloadInfo> GetJarDownloadInfos()
    {
        // Get libraries
        var libraryDownloadInfos = context.Version.Libraries
            .Select(x => x.GetDownload(context))
            .OfType<DownloadInfo>() // Get rid of the libraries that don't have a download
            .Select(x =>
                new FileDownloadInfo(x.Url, Path.Combine(context.Preferences.WorkingDirectory, "libraries", x.Path)));
        
        foreach (var downloadInfo in libraryDownloadInfos)
            yield return downloadInfo;
        
        // Get main jar
        yield return new FileDownloadInfo(
            context.Version.MainJar.Url,
            Path.Combine(context.Preferences.WorkingDirectory, context.Version.MainJar.Path));
    }

    private async Task<IEnumerable<FileDownloadInfo>> GetAssetDownloadInfosAsync()
    {
        // Download asset index
        var assetIndexStr = await WebHelper.Fetch(context.Version.AssetIndex.Url);
        var indexesDir = Path.Combine(context.Preferences.WorkingDirectory, "assets", "indexes");
        Directory.CreateDirectory(indexesDir);
        await File.WriteAllTextAsync(Path.Combine(indexesDir, $"{context.Version.AssetIndex.Name}.json"), assetIndexStr);
        
        // Parse asset index
        var assetIndex = JObject.Parse(assetIndexStr);
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

    private IEnumerable<FileDownloadInfo> EnumerateAssetDownloadInfos(IEnumerable<Asset> assets)
    {
        foreach (var asset in assets)
        {
            var assetGroup = asset.Hash.Substring(0, 2);
            var assetPath = Path.Combine(context.Preferences.WorkingDirectory, "assets", "objects", assetGroup, asset.Hash);
            var url = $"https://resources.download.minecraft.net/{assetGroup}/{asset.Hash}";
            yield return new FileDownloadInfo(url, assetPath);
        }
    }
}
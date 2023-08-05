using System.Collections.Concurrent;
using System.Diagnostics;

namespace FlexLauncher.Util;

public class DownloadProgress
{
    public int TotalFiles { get; }
    public int DownloadedFiles { get; }
    public string LastDownloadedFile { get; }
    
    public DownloadProgress(int totalFiles, int downloadedFiles, string lastDownloadedFile)
    {
        TotalFiles = totalFiles;
        DownloadedFiles = downloadedFiles;
        LastDownloadedFile = lastDownloadedFile;
    }
}

public class FileDownloadInfo
{
    public string Url { get; }
    public string Path { get; }
    
    public FileDownloadInfo(string url, string path)
    {
        Url = url;
        Path = path;
    }
}

/// <summary>
/// Util class to download multiple files at once
/// </summary>
public class FileDownloader
{
    private readonly ConcurrentQueue<FileDownloadInfo> queue = new();
    private readonly int totalFiles;

    public FileDownloader(IEnumerable<FileDownloadInfo> files)
    {
        foreach (var file in files)
            queue.Enqueue(file);
        
        totalFiles = queue.Count;
    }
    
    public async Task DownloadAsync(IProgress<DownloadProgress>? progress = null, int maxConcurrentDownloads = 8)
    {
        var tasks = new List<Task>();
        var downloadedFiles = 0;
        for (var i = 0; i < maxConcurrentDownloads; i++)
        {
            IProgress<string>? taskProgress = null;
            if (progress != null)
            {
                taskProgress = new Progress<string>(x =>
                {
                    downloadedFiles++;
                    progress.Report(new DownloadProgress(totalFiles, downloadedFiles, x));
                });
            }
            tasks.Add(DownloadQueuedFilesAsync(taskProgress));
        }
        await Task.WhenAll(tasks);
    }

    private async Task DownloadQueuedFilesAsync(IProgress<string>? progress)
    {
        using var client = new HttpClient();
        
        while (queue.TryDequeue(out var file))
        {
            var directory = Path.GetDirectoryName(file.Path)!;
            Directory.CreateDirectory(directory);
            
            using var response = await client.GetAsync(file.Url);
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(file.Path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await contentStream.CopyToAsync(fileStream);
            
            progress?.Report(file.Path);
        }
    }
}
using System.Collections.Concurrent;
using System.Diagnostics;

namespace FlexLauncher.Util;

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
    private readonly ConcurrentQueue<FileDownloadInfo> queue = new ConcurrentQueue<FileDownloadInfo>();

    public FileDownloader(IEnumerable<FileDownloadInfo> files)
    {
        foreach (var file in files)
        {
            queue.Enqueue(file);
        }
    }
    
    public async Task DownloadAsync(int maxConcurrentDownloads = 5)
    {
        var tasks = new List<Task>();
        for (int i = 0; i < maxConcurrentDownloads; i++)
        {
            tasks.Add(DownloadQueuedFilesAsync());
        }
        await Task.WhenAll(tasks);
    }

    private async Task DownloadQueuedFilesAsync()
    {
        using var client = new HttpClient();
        while (queue.TryDequeue(out var file))
        {
            Debug.WriteLine($"Downloading '{file.Url}' to '{file.Path}'");
            
            var directory = Path.GetDirectoryName(file.Path)!;
            Directory.CreateDirectory(directory);
            
            using var response = await client.GetAsync(file.Url);
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(file.Path, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            await contentStream.CopyToAsync(fileStream);
        }
    }
}
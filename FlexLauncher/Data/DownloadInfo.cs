namespace FlexLauncher.Data;

/// <summary>
/// Necessary information to download a file
/// </summary>
public class DownloadInfo
{
    public string Url { get; }
    public string Path { get; }
    public long? Size { get; }
    public string? Sha1 { get; }

    public DownloadInfo(string url, string path, long? size, string? sha1)
    {
        Url = url;
        Size = size;
        Sha1 = sha1;
        Path = path;
    }
}
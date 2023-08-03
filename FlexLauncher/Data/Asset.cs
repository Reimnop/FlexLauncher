namespace FlexLauncher.Data;

public class Asset
{
    public string Hash { get; }
    public long Size { get; }
    
    public Asset(string hash, long size)
    {
        Hash = hash;
        Size = size;
    }
}
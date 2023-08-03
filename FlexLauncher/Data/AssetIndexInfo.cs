namespace FlexLauncher.Data;

public class AssetIndexInfo
{
    public string Name { get; }
    public string Url { get; }
    
    public AssetIndexInfo(string name, string url)
    {
        Name = name;
        Url = url;
    }
}
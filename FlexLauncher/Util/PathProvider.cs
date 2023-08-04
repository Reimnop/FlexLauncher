namespace FlexLauncher.Util;

public class PathProvider
{
    // Relative paths
    public string LibrariesPathRelative => "libraries";
    public string NativesPathRelative => "natives";
    public string AssetsPathRelative => "assets";
    public string AssetIndexesPathRelative => Path.Combine(AssetsPathRelative, "indexes");
    public string AssetObjectsPathRelative => Path.Combine(AssetsPathRelative, "objects");
    public string VersionsPathRelative => "versions";
    
    // Absolute paths
    public string LibrariesPath => Path.Combine(basePath, LibrariesPathRelative);
    public string NativesPath => Path.Combine(basePath, NativesPathRelative);
    public string AssetsPath => Path.Combine(basePath, AssetsPathRelative);
    public string AssetIndexesPath => Path.Combine(basePath, AssetIndexesPathRelative);
    public string AssetObjectsPath => Path.Combine(basePath, AssetObjectsPathRelative);
    public string VersionsPath => Path.Combine(basePath, VersionsPathRelative);

    // Base path
    private readonly string basePath;

    public PathProvider(string basePath)
    {
        this.basePath = basePath;
    }
}
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Data;

/// <summary>
/// Information about a Minecraft version
/// </summary>
public class VersionInfo
{
    public string Name { get; }
    public VersionType Type { get; }
    public string MainClass { get; }

    public AssetIndexInfo AssetIndex { get; }
    public DownloadInfo MainJar { get; }
    
    public IReadOnlyList<Library> Libraries => libraries;
    public IReadOnlyList<ArgumentList> JvmArguments => jvmArguments;
    public IReadOnlyList<ArgumentList> GameArguments => gameArguments;
    
    private readonly List<Library> libraries;
    private readonly List<ArgumentList> jvmArguments;
    private readonly List<ArgumentList> gameArguments;

    public VersionInfo(
        string name, VersionType type, string mainClass, 
        AssetIndexInfo assetIndex, DownloadInfo mainJar, 
        IEnumerable<Library> libraries, IEnumerable<ArgumentList> jvmArguments, IEnumerable<ArgumentList> gameArguments)
    {
        Name = name;
        Type = type;
        MainClass = mainClass;
        AssetIndex = assetIndex;
        MainJar = mainJar;
        this.libraries = libraries.ToList();
        this.jvmArguments = jvmArguments.ToList();
        this.gameArguments = gameArguments.ToList();
    }
}
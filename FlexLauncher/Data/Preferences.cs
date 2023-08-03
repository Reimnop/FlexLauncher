namespace FlexLauncher.Data;

public class Preferences
{
    public string LauncherName { get; }
    public string LauncherVersion { get; }
    public string ClientId { get; }
    public string JavaPath { get; }
    public string WorkingDirectory { get; }
    public string GameDirectory { get; }
    public bool DemoMode { get; }
    public Resolution? CustomResolution { get; }
    public QuickPlay? QuickPlay { get; }

    public Preferences(
        string launcherName, 
        string launcherVersion, 
        string clientId, 
        string javaPath, 
        string workingDirectory,
        string gameDirectory, 
        bool demoMode, 
        Resolution? customResolution,
        QuickPlay? quickPlay)
    {
        LauncherName = launcherName;
        LauncherVersion = launcherVersion;
        ClientId = clientId;
        JavaPath = javaPath;
        GameDirectory = gameDirectory;
        WorkingDirectory = workingDirectory;
        DemoMode = demoMode;
        CustomResolution = customResolution;
        QuickPlay = quickPlay;
    }
}
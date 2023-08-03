using System.Diagnostics;

namespace FlexLauncher.Data;

public class GameInstance
{
    public VersionInfo Version { get; }
    public Preferences Preferences { get; }
    public Process Process { get; }
    
    public GameInstance(VersionInfo version, Preferences preferences, Process process)
    {
        Version = version;
        Preferences = preferences;
        Process = process;
    }
}
using System.Text;
using System.Text.RegularExpressions;
using FlexLauncher.Data;

namespace FlexLauncher.Core;

public class LaunchContext
{
    private static readonly Regex PlaceholderRegex = new(@"\$\{([a-zA-Z0-9_]+)\}");

    private readonly Dictionary<string, string> variables = new();

    public VersionInfo Version { get; }
    public Preferences Preferences { get; }

    public LaunchContext(VersionInfo version, AuthInfo authInfo, Preferences preferences)
    {
        Version = version;
        Preferences = preferences;
        
        variables.Add("version_name", version.Name);
        variables.Add("version_type", version.Type switch
        {
            VersionType.Release => "release",
            VersionType.Snapshot => "snapshot",
            VersionType.Beta => "beta",
            VersionType.Alpha => "alpha",
            VersionType.Modded => "modded",
            VersionType.Unknown => "unknown",
            _ => throw new ArgumentOutOfRangeException()
        });
        variables.Add("game_directory", preferences.GameDirectory);
        variables.Add("assets_root", "assets");
        variables.Add("assets_index_name", version.AssetIndex.Name);
        variables.Add("auth_player_name", authInfo.Username);
        variables.Add("auth_uuid", authInfo.Uuid);
        variables.Add("auth_access_token", authInfo.AccessToken);
        variables.Add("user_type", authInfo.UserType switch
        {
            UserType.Mojang => "mojang",
            UserType.Microsoft => "msa",
            _ => throw new ArgumentOutOfRangeException()
        });
        variables.Add("client_id", preferences.ClientId);
        variables.Add("version_id", version.Name);
        variables.Add("natives_directory", "natives");
        variables.Add("launcher_name", preferences.LauncherName);
        variables.Add("launcher_version", preferences.LauncherVersion);
        variables.Add("auth_xuid", authInfo.Xuid);
        variables.Add("library_directory", "libraries");
        variables.Add("libraries", GetLibraries(version));
        
        // Custom resolution
        if (preferences.CustomResolution != null)
        {
            var customResolution = preferences.CustomResolution.Value;
            variables.Add("resolution_width", customResolution.Width.ToString());
            variables.Add("resolution_height", customResolution.Height.ToString());
        }
        
        // Quick play
        if (preferences.QuickPlay != null)
        {
            var quickPlay = preferences.QuickPlay.Value;
            var quickPlayPath = quickPlay.Path ?? "quickPlay/log.json";
            variables.Add("quick_play_path", quickPlayPath);
            variables.Add("quick_play_identifier", quickPlay.Identifier);
        }
    }

    private string GetLibraries(VersionInfo version)
    {
        var separator = OperatingSystem.IsWindows() ? ';' : ':';
        
        var stringBuilder = new StringBuilder();
        foreach (var library in version.Libraries)
        {
            var download = library.GetDownload(this);

            if (download == null)
                continue;

            stringBuilder.Append(Path.Combine("libraries", download.Path));
            stringBuilder.Append(separator);
        }

        stringBuilder.Append(version.MainJar.Path);
        stringBuilder.Append(separator);
        
        return stringBuilder.ToString();
    }
    
    public string ParseVariables(string value)
    {
        return PlaceholderRegex.Replace(value, match =>
        {
            var key = match.Groups[1].Value;
            return variables.TryGetValue(key, out var result) ? result : match.Value;
        });
    }
}
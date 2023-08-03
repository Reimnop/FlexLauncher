using Newtonsoft.Json.Linq;

namespace FlexLauncher.Core;

public class EnvironmentCondition : Condition
{
    private readonly Dictionary<string, Func<JToken, bool>> conditionCheckers = new()
        {
            {"os", OsChecker},
            {"arch", ArchChecker}
        };

    private readonly IDictionary<string, JToken> values;

    public EnvironmentCondition(IDictionary<string, JToken> values)
    {
        this.values = values;
    }
    
    public override bool Check(LaunchContext context)
    {
        foreach (var (key, value) in values)
        {
            if (conditionCheckers.TryGetValue(key, out Func<JToken, bool>? checker))
            {
                if (!checker(value))
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    private static bool OsChecker(JToken name)
    {
        return name.Value<string>() switch
        {
            "windows" => OperatingSystem.IsWindows(),
            "macos" => OperatingSystem.IsMacOS(),
            "linux" => OperatingSystem.IsLinux(),
            _ => false
        };
    }

    private static bool ArchChecker(JToken arch)
    {
        return arch.Value<string>() switch
        {
            "x86" => !Environment.Is64BitOperatingSystem,
            "x64" => Environment.Is64BitOperatingSystem,
            _ => false
        };
    }
}
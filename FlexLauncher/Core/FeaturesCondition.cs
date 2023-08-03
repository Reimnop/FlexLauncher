using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Core;

public class FeaturesCondition : Condition
{
    private readonly Dictionary<string, Func<JToken, LaunchContext, bool>> conditionCheckers = new()
        {
            {"demo", DemoChecker},
            {"custom_resolution", CustomResolutionChecker},
            {"quick_plays_support", QuickPlaysSupportChecker},
            {"quick_play_singleplayer", QuickPlaySingleplayerChecker},
            {"quick_play_multiplayer", QuickPlayMultiplayerChecker},
            {"quick_play_realms", QuickPlayRealmsChecker}
        };
    
    private static bool DemoChecker(JToken value, LaunchContext context)
    {
        return value.Value<bool>() == context.Preferences.DemoMode;
    }

    private static bool CustomResolutionChecker(JToken value, LaunchContext context)
    {
        var customResolution = context.Preferences.CustomResolution != null;
        return value.Value<bool>() == customResolution;
    }
    
    private static bool QuickPlaysSupportChecker(JToken value, LaunchContext context)
    {
        var quickPlaysSupport = context.Preferences.QuickPlay != null;
        return value.Value<bool>() == quickPlaysSupport;
    }

    private static bool QuickPlaySingleplayerChecker(JToken value, LaunchContext context)
    {
        var quickPlaySingleplayer = 
            context.Preferences.QuickPlay != null && 
            context.Preferences.QuickPlay.Value.Type == QuickPlayType.Singleplayer;
        return value.Value<bool>() == quickPlaySingleplayer;
    }
    
    private static bool QuickPlayMultiplayerChecker(JToken value, LaunchContext context)
    {
        var quickPlayMultiplayer = 
            context.Preferences.QuickPlay != null && 
            context.Preferences.QuickPlay.Value.Type == QuickPlayType.Multiplayer;
        return value.Value<bool>() == quickPlayMultiplayer;
    }
    
    private static bool QuickPlayRealmsChecker(JToken value, LaunchContext context)
    {
        var quickPlayRealms = 
            context.Preferences.QuickPlay != null && 
            context.Preferences.QuickPlay.Value.Type == QuickPlayType.Realms;
        return value.Value<bool>() == quickPlayRealms;
    }

    private readonly IDictionary<string, JToken> values;

    public FeaturesCondition(IDictionary<string, JToken> values)
    {
        this.values = values;
    }

    public override bool Check(LaunchContext context)
    {
        foreach (var (key, value) in values)
        {
            if (conditionCheckers.TryGetValue(key, out var checker))
            {
                if (!checker(value, context))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
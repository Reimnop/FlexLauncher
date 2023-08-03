using Newtonsoft.Json.Linq;

namespace FlexLauncher.Core;

public static class ConditionFactory
{
    private static readonly Dictionary<string, Func<IDictionary<string, JToken>, Condition>> conditionFactories
        = new()
        {
            {"environment", keyValues => new EnvironmentCondition(keyValues)},
            {"features", keyValues => new FeaturesCondition(keyValues)}
        };

    public static Condition? GetCondition(string name, JObject json)
    {
        if (conditionFactories.TryGetValue(name, out var func))
        {
            Dictionary<string, JToken> keyValues = json
                .Properties()
                .Select(x => (x.Name, x.Value))
                .Where(x => x.Item2 != null)
                .ToDictionary(x => x.Name, x => x.Item2!); // it won't be null, but the compiler doesn't know that
            return func(keyValues);
        }
        
        return null;
    }
}
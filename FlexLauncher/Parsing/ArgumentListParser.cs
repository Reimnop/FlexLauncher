using FlexLauncher.Core;
using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public static class ArgumentListParser
{
    public static ArgumentList ParseJson(JObject json)
    {
        var conditions = new List<Condition>();
        var arguments = new List<string>();
        
        if (json["rules"] is JObject rules)
        {
            foreach (var rule in rules.Properties())
            {
                var name = rule.Name;
                if (rule.Value is not JObject args) 
                    continue;
                
                var condition = ConditionFactory.GetCondition(name, args);
                if (condition != null)
                {
                    conditions.Add(condition);
                }
            }
        }

        if (json["values"] is not JArray values)
        {
            return new ArgumentList(conditions, arguments);
        }

        foreach (var value in values)
        {
            var arg = value.Value<string>();
                
            if (arg != null)
            {
                arguments.Add(arg);
            }
        }
        
        return new ArgumentList(conditions, arguments);
    }
}
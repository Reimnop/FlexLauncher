using FlexLauncher.Core;
using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public static class LibraryParser
{
    public static Library ParseJson(JObject json)
    {
        var conditions = new List<Condition>();
        if (json["rules"] is JObject rules)
        {
            foreach (var rule in rules.Properties())
            {
                var name = rule.Name;
                if (rule.Value is JObject args)
                {
                    var condition = ConditionFactory.GetCondition(name, args);
                    if (condition != null)
                        conditions.Add(condition);
                }
            }
        }
        
        var downloadInfo = DownloadInfoParser.ParseJson(json);
        return new Library(downloadInfo, conditions);
    }
}
using FlexLauncher.Core;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Data;

public class Library
{
    private readonly DownloadInfo downloadInfo;
    private readonly List<Condition> conditions;
    
    public Library(DownloadInfo downloadInfo, List<Condition> conditions)
    {
        this.downloadInfo = downloadInfo;
        this.conditions = conditions;
    }

    public DownloadInfo? GetDownload(LaunchContext context)
    {
        if (!CheckConditions(context))
            return null;

        return downloadInfo;
    }
    
    private bool CheckConditions(LaunchContext context)
    {
        return conditions.All(condition => condition.Check(context));
    }
}
using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public static class DownloadInfoParser
{
    public static DownloadInfo ParseJson(JObject json)
    {
        var url = json["url"]!.Value<string>()!;
        var path = json["path"]!.Value<string>()!;
        var size = json["size"]?.Value<long>();
        var sha1 = json["sha1"]?.Value<string>()!;
        return new DownloadInfo(url, path, size, sha1);
    }
}
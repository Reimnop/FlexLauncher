using FlexLauncher.Data;
using Newtonsoft.Json.Linq;

namespace FlexLauncher.Parsing;

public static class AssetParser
{
    public static Asset ParseJson(JObject json)
    {
        var hash = json["hash"]!.Value<string>()!;
        var size = json["size"]!.Value<long>();
        return new Asset(hash, size);
    }
}
using System;
using System.Reflection;

namespace FlexLauncherUI.Utils;

public static class PathUtil
{
    public static Uri GetUri(string path)
    {
        var assemblyName = Assembly.GetEntryAssembly()!.GetName().Name!;
        return new Uri($"avares://{assemblyName}/{path}");
    }
}
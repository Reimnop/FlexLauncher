using System;

namespace FlexLauncherUI.Utils;

public static class SafetyUtil
{
    public static T EnsureNotNull<T>(this T? obj, string name) where T : class
    {
        if (obj is null)
            throw new ArgumentNullException(name);

        return obj;
    }
}
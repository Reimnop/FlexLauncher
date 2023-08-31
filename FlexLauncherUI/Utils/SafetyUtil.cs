using System;

namespace FlexLauncherUI.Utils;

public static class SafetyUtil
{
    public static T EnsureNotNull<T>(this T? value) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return value;
    }
}
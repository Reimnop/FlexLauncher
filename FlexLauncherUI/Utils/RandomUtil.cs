using System;

namespace FlexLauncherUI.Utils;

public static class RandomUtil
{
    private static Random Random { get; } = new();

    public static string GenerateId(int length = 16)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        Span<char> buffer = stackalloc char[length];
        for (var i = 0; i < length; i++)
            buffer[i] = characters[Random.Next(characters.Length)];
        return new string(buffer);
    }
}
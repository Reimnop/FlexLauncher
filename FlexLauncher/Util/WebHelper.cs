namespace FlexLauncher.Util;

public static class WebHelper
{
    public static async Task<string> Fetch(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }
}
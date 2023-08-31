using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace FlexLauncherUI.Utils;

public static class WebUtil
{
    public static async Task<Image> DownloadImageFromUrlAsync(string url)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        await using var stream = await response.Content.ReadAsStreamAsync();
        
        return await Image.LoadAsync(stream);
    }

    public static async Task<Image> DownloadPlayerFaceAsync(string uuid)
    {
        // Use Visage API to get the player face
        var url = $"https://visage.surgeplay.com/face/48/{uuid}";
        return await DownloadImageFromUrlAsync(url);
    }
}
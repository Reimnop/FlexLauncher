using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FlexLauncherUI.Utils;

public static class WebUtil
{
    public static async Task<Image<Rgba32>> DownloadImageFromUrlAsync(string url)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        await using var stream = await response.Content.ReadAsStreamAsync();
        
        return await Image.LoadAsync<Rgba32>(stream);
    }
}
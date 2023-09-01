using System;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FlexLauncherUI.Utils;

public static class MinecraftAppearanceUtil
{
    public static async Task<Image<Rgba32>> DownloadSkinFromUuidAsync(string uuid)
    {
        // Use Visage API to get the skin
        var url = $"https://visage.surgeplay.com/processedskin/{uuid}";
        return await WebUtil.DownloadImageFromUrlAsync(url);
    }

    public static Task<Image<Rgba32>> ExtractFaceFromSkinAsync(Image<Rgba32> image)
    {
        if (image.Width != 64 || image.Height != 64)
            throw new ArgumentException("The image must be 64x64!", nameof(image));
        
        // Extract the base face layer
        var baseFace = image.Clone(i => i.Crop(new Rectangle(8, 8, 8, 8)));
        
        // Extract the overlay face layer
        var overlayFace = image.Clone(i => i.Crop(new Rectangle(40, 8, 8, 8)));
        
        // Overlay the overlay face layer on the base face layer
        baseFace.Mutate(i => i.DrawImage(overlayFace, new Point(0, 0), 1f));
        
        return Task.FromResult(baseFace);
    }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using FlexLauncherUI.Models;
using FlexLauncherUI.Utils;

namespace FlexLauncherUI.Controls;

public class AccountDisplay : TemplatedControl
{
    public static readonly StyledProperty<AccountModel?> AccountProperty =
        AvaloniaProperty.Register<AccountDisplay, AccountModel?>(nameof(Account));
    
    public static readonly StyledProperty<IImage?> AccountIconProperty =
        AvaloniaProperty.Register<AccountDisplay, IImage?>(nameof(AccountIcon));

    public AccountModel? Account
    {
        get => GetValue(AccountProperty);
        set => SetValue(AccountProperty, value);
    }

    public IImage? AccountIcon
    {
        get => GetValue(AccountIconProperty);
        set => SetValue(AccountIconProperty, value);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        AccountIcon = LoadDefaultIcon();

        if (Account != null)
            LoadAccountIcon(Account)
                .ContinueWith(x =>
                {
                    if (x.Exception != null)
                        throw x.Exception; // Rethrow the exception

                    // Update the icon
                    Dispatcher.UIThread.InvokeAsync(() => AccountIcon = x.Result);
                });
    }
    
    private static IImage LoadDefaultIcon()
    {
        var uri = PathUtil.GetUri("Assets/Images/Placeholder.png");
        using var stream = AssetLoader.Open(uri);

        return new Bitmap(stream);
    }

    private static async Task<IImage> LoadAccountIcon(AccountModel account)
    {
        // Get face image
        var skin = await MinecraftAppearanceUtil.DownloadSkinFromUuidAsync(account.Uuid);
        var face = await MinecraftAppearanceUtil.ExtractFaceFromSkinAsync(skin);
        
        // Copy the pixels to the buffer
        var buffer = new byte[face.Width * face.Height * 4];
        face.CopyPixelDataTo(buffer);
        
        // Load the image as Avalonia Bitmap
        var bitmap = new WriteableBitmap(
            new PixelSize(face.Width, face.Height), 
            new Vector(96, 96),
            PixelFormat.Rgba8888,
            AlphaFormat.Unpremul);
        using var framebuffer = bitmap.Lock();
        Marshal.Copy(buffer, 0, framebuffer.Address, buffer.Length);

        return bitmap;
    }
}
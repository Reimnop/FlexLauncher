using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using FlexLauncherUI.Models;
using FlexLauncherUI.Utils;
using SixLabors.ImageSharp.PixelFormats;

namespace FlexLauncherUI.Controls;

public class AccountDisplay : TemplatedControl
{
    public static readonly DirectProperty<AccountDisplay, AccountModel?> AccountProperty =
        AvaloniaProperty.RegisterDirect<AccountDisplay, AccountModel?>(
            nameof(Account),
            o => o.Account,
            (o, v) => o.Account = v);
    
    public static readonly DirectProperty<AccountDisplay, IImage?> AccountIconProperty =
        AvaloniaProperty.RegisterDirect<AccountDisplay, IImage?>(
            nameof(AccountIcon),
            o => o.AccountIcon,
            (o, v) => o.AccountIcon = v);

    public AccountModel? Account
    {
        get => account;
        set => SetAndRaise(AccountProperty, ref account, value);
    }

    public IImage? AccountIcon
    {
        get => accountIcon;
        set => SetAndRaise(AccountIconProperty, ref accountIcon, value);
    }

    private AccountModel? account;
    private IImage? accountIcon;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        accountIcon = LoadDefaultIcon();
        
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
        var image = await WebUtil.DownloadPlayerFaceAsync(account.Uuid);
        
        // Copy the pixels to the buffer
        var imageRgba32 = image.CloneAs<Rgba32>();
        var buffer = new byte[image.Width * image.Height * 4];
        imageRgba32.CopyPixelDataTo(buffer);
        
        // Load the image as Avalonia Bitmap
        var bitmap = new WriteableBitmap(
            new PixelSize(image.Width, image.Height), 
            new Vector(96, 96),
            PixelFormat.Rgba8888,
            AlphaFormat.Unpremul);
        using var framebuffer = bitmap.Lock();
        Marshal.Copy(buffer, 0, framebuffer.Address, buffer.Length);

        return bitmap;
    }
}
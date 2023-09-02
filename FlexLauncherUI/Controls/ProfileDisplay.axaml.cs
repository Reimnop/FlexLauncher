using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FlexLauncherUI.Models;
using FlexLauncherUI.Utils;

namespace FlexLauncherUI.Controls;

public class ProfileDisplay : TemplatedControl
{
    public static readonly StyledProperty<ProfileModel?> ProfileProperty =
        AvaloniaProperty.Register<ProfileDisplay, ProfileModel?>(nameof(Profile));

    public ProfileModel? Profile
    {
        get => GetValue(ProfileProperty);
        set => SetValue(ProfileProperty, value);
    }
    
    private IImage ProfileIcon
    {
        get
        {
            // Return the profile icon
            // TODO: ^
            
            // Return a default icon if above fails
            var uri = PathUtil.GetUri("Assets/Images/Placeholder.png");
            using var stream = AssetLoader.Open(uri);

            return new Bitmap(stream);
        }
    }
    
    private string ProfileName => Profile?.Name ?? "null";
    
    private string ProfileLastUpdated => Profile != null ? $"Last updated {Profile.LastUpdated:dd/MM/yyyy hh:mm tt}" : "null";
    
    private string ProfileTime => Profile != null ? $"Played {Math.Ceiling(Profile.PlayTime.TotalHours)} hours - since {Profile.DateCreated:dd/MM/yyyy}" : "null";
}
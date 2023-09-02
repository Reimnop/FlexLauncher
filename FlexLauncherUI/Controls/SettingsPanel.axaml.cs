using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace FlexLauncherUI.Controls;

[TemplatePart("PART_ContentGrid", typeof(Grid))]
public class SettingsPanel : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<SettingsPanel, string>(nameof(Header));
    
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<SettingsPanel, double>(nameof(Spacing), 0);

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }
    
    private Grid contentGridPart = null!;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        contentGridPart = e.NameScope.Find<Grid>("PART_ContentGrid")!;
    }
}
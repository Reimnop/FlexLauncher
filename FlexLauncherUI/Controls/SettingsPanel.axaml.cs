using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;

namespace FlexLauncherUI.Controls;

[TemplatePart("PART_ContentGrid", typeof(Grid))]
public class SettingsPanel : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<SettingsPanel, string>(nameof(Header));
    
    public static readonly StyledProperty<double> HeaderSpacingProperty =
        AvaloniaProperty.Register<SettingsPanel, double>(nameof(HeaderSpacing));
    
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<SettingsPanel, double>(nameof(SpacingProperty));

    public static readonly AttachedProperty<string> TitleProperty =
        AvaloniaProperty.RegisterAttached<SettingsPanel, Control, string>(string.Empty);
    
    [Content]
    public Avalonia.Controls.Controls Children { get; } = new();

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public double HeaderSpacing
    {
        get => GetValue(HeaderSpacingProperty);
        set => SetValue(HeaderSpacingProperty, value);
    }
    
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    // private bool contentDirty = true;
    
    private Grid? contentGridPart;
    
    public static string GetTitle(Control control)
    {
        return control.GetValue(TitleProperty);
    }
    
    public static void SetTitle(Control control, string value)
    {
        control.SetValue(TitleProperty, value);
    }

    public SettingsPanel()
    {
        Children.CollectionChanged += (_, _) => InvalidateContent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        // Update content if spacing changes
        if (change.Property == SpacingProperty || change.Property == TitleProperty)
            InvalidateContent();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        var contentGrid = e.NameScope.Find<Grid>("PART_ContentGrid");
        Debug.Assert(contentGrid != null);
        
        contentGridPart = contentGrid;
        
        InvalidateContent();
    }

    private void InvalidateContent()
    {
        // contentDirty = true;
        // TODO: Optimize this
        UpdateContent();
    }

    private void UpdateContent()
    {
        if (contentGridPart == null)
            return;
        
        // Clear the content grid
        contentGridPart.Children.Clear();
        contentGridPart.RowDefinitions.Clear();
        
        // Calculate the last child index
        var lastChildIndex = Children.Count - 1;
        
        // Put each child in the content grid
        var childIndex = 0;
        foreach (var child in Children)
        {
            var rowIndex = contentGridPart.RowDefinitions.Count;
            
            // Create row definition
            contentGridPart.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
            
            // Create title
            var title = new TextBlock
            {
                Text = GetTitle(child),
                VerticalAlignment = VerticalAlignment.Center,
                Classes = { "HeadingSmall" }
            };
            
            // Set title row and column
            Grid.SetRow(title, rowIndex);
            Grid.SetColumn(title, 0);
            
            // Set child row and column
            Grid.SetRow(child, rowIndex);
            Grid.SetColumn(child, 1);
            
            // Add title and child to content grid
            contentGridPart.Children.Add(title);
            contentGridPart.Children.Add(child);
            
            // Add spacing if not last child
            if (Spacing > 0 && childIndex != lastChildIndex)
                contentGridPart.RowDefinitions.Add(new RowDefinition(Spacing, GridUnitType.Pixel));
            
            // Increment child index
            childIndex++;
        }
    }
}
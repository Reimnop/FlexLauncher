namespace FlexLauncher.Data;

/// <summary>
/// Represents a display resolution
/// </summary>
public struct Resolution
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public Resolution(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
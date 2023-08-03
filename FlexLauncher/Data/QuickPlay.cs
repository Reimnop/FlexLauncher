namespace FlexLauncher.Data;

public enum QuickPlayType
{
    Singleplayer,
    Multiplayer,
    Realms
}

// See https://www.minecraft.net/en-us/article/minecraft-snapshot-23w14a for more information
public struct QuickPlay
{
    public QuickPlayType Type { get; set; } 
    public string Identifier { get; set; }
    public string? Path { get; set; }
    
    public QuickPlay(QuickPlayType type, string identifier, string? path = null)
    {
        Type = type;
        Identifier = identifier;
        Path = path;
    }
}
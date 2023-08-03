namespace FlexLauncher.Data;

public enum UserType
{
    Mojang,
    Microsoft
}

public class AuthInfo
{
    public string Username { get; }
    public string Uuid { get; }
    public string AccessToken { get; }
    public string Xuid { get; }
    public UserType UserType { get; }
    
    public AuthInfo(string username, string uuid, string accessToken, string xuid, UserType userType)
    {
        Username = username;
        Uuid = uuid;
        AccessToken = accessToken;
        Xuid = xuid;
        UserType = userType;
    }
}
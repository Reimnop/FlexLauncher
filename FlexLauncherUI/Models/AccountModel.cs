using ReactiveUI;

namespace FlexLauncherUI.Models;

public class AccountModel : ReactiveObject
{
    public string Id
    {
        get => id;
        set => this.RaiseAndSetIfChanged(ref id, value);
    }
    
    public string Uuid
    {
        get => uuid;
        set => this.RaiseAndSetIfChanged(ref uuid, value);
    }
    
    public string Username
    {
        get => username;
        set => this.RaiseAndSetIfChanged(ref username, value);
    }

    private string id = string.Empty;
    private string uuid = string.Empty;
    private string username = string.Empty;
}
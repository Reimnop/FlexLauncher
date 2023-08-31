using System;
using ReactiveUI;

namespace FlexLauncherUI.Models;

public class ProfileModel : ReactiveObject
{
    public string Id
    {
        get => id;
        set => this.RaiseAndSetIfChanged(ref id, value);
    }
    
    public string Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged(ref name, value);
    }

    public DateTime LastUpdated
    {
        get => lastUpdated;
        set => this.RaiseAndSetIfChanged(ref lastUpdated, value);
    }
    
    public DateTime DateCreated
    {
        get => dateCreated;
        set => this.RaiseAndSetIfChanged(ref dateCreated, value);
    }

    public TimeSpan PlayTime
    {
        get => playTime;
        set => this.RaiseAndSetIfChanged(ref playTime, value);
    }

    private string id = string.Empty;
    private string name = string.Empty;
    private DateTime lastUpdated;
    private DateTime dateCreated;
    private TimeSpan playTime;
}
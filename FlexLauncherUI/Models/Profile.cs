using System;

namespace FlexLauncherUI.Models;

public class Profile
{
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan PlayTime { get; set; }
    
    public Profile(string name, string description, TimeSpan playTime)
    {
        Name = name;
        Description = description;
        PlayTime = playTime;
    }
}
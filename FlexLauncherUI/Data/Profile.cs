using System;

namespace FlexLauncherUI.Data;

public record Profile(string Id, string Name, DateTime LastUpdated, DateTime DateCreated, TimeSpan PlayTime);
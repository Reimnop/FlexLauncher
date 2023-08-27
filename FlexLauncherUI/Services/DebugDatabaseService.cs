using System;
using System.Collections.Generic;
using FlexLauncherUI.Data;

namespace FlexLauncherUI.Services;

public class DebugDatabaseService : IDatabaseService
{
    public async IAsyncEnumerable<Profile> FetchProfilesAsync()
    {
        yield return new Profile("1", "Test Profile", DateTime.Now, DateTime.Now, TimeSpan.FromHours(1));
        yield return new Profile("2", "Test Profile 2", DateTime.Now, DateTime.Now, TimeSpan.FromHours(2));
        yield return new Profile("3", "Test Profile 3", DateTime.Now, DateTime.Now, TimeSpan.FromHours(3));
        yield return new Profile("4", "Test Profile 4", DateTime.Now, DateTime.Now, TimeSpan.FromHours(4));
        yield return new Profile("5", "Test Profile 5", DateTime.Now, DateTime.Now, TimeSpan.FromHours(5));
        yield return new Profile("6", "Test Profile 6", DateTime.Now, DateTime.Now, TimeSpan.FromHours(6));
    }
}
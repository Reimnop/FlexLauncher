using System;
using System.Collections.Generic;
using FlexLauncherUI.Data;
using FlexLauncherUI.Utils;

namespace FlexLauncherUI.Services;

public class DebugDatabaseService : IDatabaseService
{
    public async IAsyncEnumerable<Profile> FetchProfilesAsync()
    {
        yield return new Profile(RandomUtil.GenerateId(), "Test Profile", DateTime.Now, DateTime.Now, TimeSpan.FromHours(1));
        yield return new Profile(RandomUtil.GenerateId(), "Test Profile 2", DateTime.Now, DateTime.Now, TimeSpan.FromHours(2));
    }

    public async IAsyncEnumerable<Account> FetchAccountsAsync()
    {
        yield return new Account(RandomUtil.GenerateId(), "2d4faffa-8e09-4627-9fdd-a0eddc2fc981", "Reimnop", string.Empty, string.Empty);
        yield return new Account(RandomUtil.GenerateId(), "32ce0fee-4929-4d86-b16b-48e005376655", "ReimAlt", string.Empty, string.Empty);
    }
}
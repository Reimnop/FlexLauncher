using System.Collections.Generic;
using FlexLauncherUI.Data;

namespace FlexLauncherUI.Services;

public interface IDatabaseService
{
    IAsyncEnumerable<Profile> FetchProfilesAsync();
    IAsyncEnumerable<Account> FetchAccountsAsync();
}
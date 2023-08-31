using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FlexLauncherUI.Data;
using FlexLauncherUI.Models;
using FlexLauncherUI.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace FlexLauncherUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    public IEnumerable<ProfileModel> ProfileModels
    {
        get => profilesModels;
        private set => this.RaiseAndSetIfChanged(ref profilesModels, value);
    }

    public IEnumerable<AccountModel> AccountModels
    {
        get => accountModels;
        private set => this.RaiseAndSetIfChanged(ref accountModels, value);
    }

    private IEnumerable<ProfileModel> profilesModels = null!;
    private IEnumerable<AccountModel> accountModels = null!;

    private readonly IDatabaseService databaseService;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        databaseService = serviceProvider.GetRequiredService<IDatabaseService>();
        
        ProfileModels = databaseService
            .FetchProfilesAsync()
            .Select(CreateProfileModel)
            .ToEnumerable();
        AccountModels = databaseService
            .FetchAccountsAsync()
            .Select(CreateAccountModel)
            .ToEnumerable();
    }

    private static ProfileModel CreateProfileModel(Profile profile)
        => new()
        {
            Id = profile.Id,
            Name = profile.Name,
            LastUpdated = profile.LastUpdated,
            DateCreated = profile.DateCreated,
            PlayTime = profile.PlayTime
        };
    
    private static AccountModel CreateAccountModel(Account account)
        => new()
        {
            Id = account.Id,
            Uuid = account.Uuid,
            Username = account.Username
        };
}
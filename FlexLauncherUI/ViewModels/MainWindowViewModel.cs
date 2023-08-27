using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FlexLauncherUI.Data;
using FlexLauncherUI.Models;
using FlexLauncherUI.Services;
using ReactiveUI;

namespace FlexLauncherUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand OnRefreshButtonClickedCommand { get; }

    public IEnumerable<ProfileModel> ProfileModels
    {
        get => profilesModels;
        private set => this.RaiseAndSetIfChanged(ref profilesModels, value);
    }

    private IEnumerable<ProfileModel> profilesModels = null!;

    private readonly IDatabaseService databaseService;
    
    public MainWindowViewModel(IDatabaseService databaseService)
    {
        this.databaseService = databaseService;
        ProfileModels = databaseService
            .FetchProfilesAsync()
            .Select(CreateProfileModel)
            .ToEnumerable();
        OnRefreshButtonClickedCommand = ReactiveCommand.Create(OnRefreshButtonClicked);
    }

    private void OnRefreshButtonClicked()
    {
        ProfileModels = databaseService
            .FetchProfilesAsync()
            .Select(CreateProfileModel)
            .ToEnumerable();
    }

    private static ProfileModel CreateProfileModel(Profile profile)
        => new()
        {
            Name = profile.Name,
            LastUpdated = profile.LastUpdated,
            DateCreated = profile.DateCreated,
            PlayTime = profile.PlayTime
        };
}
using System;
using System.Windows.Input;
using ReactiveUI;

namespace FlexLauncherUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand NavigateToSettingsCommand 
        => ReactiveCommand.Create(() => CurrentViewModel = new SettingsViewModel());
    
    public ICommand NavigateToMainCommand 
        => ReactiveCommand.Create(() => CurrentViewModel = new MainViewModel(serviceProvider));
    
    public ViewModelBase CurrentViewModel
    {
        get => viewModel;
        private set => this.RaiseAndSetIfChanged(ref viewModel, value);
    }

    private ViewModelBase viewModel = null!;
    
    private readonly IServiceProvider serviceProvider;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        
        CurrentViewModel = new MainViewModel(this.serviceProvider);
    }
}
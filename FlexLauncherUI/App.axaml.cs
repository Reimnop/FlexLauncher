using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FlexLauncherUI.Services;
using FlexLauncherUI.ViewModels;
using FlexLauncherUI.Views;

namespace FlexLauncherUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var profileDatabaseService = new SqliteDatabaseService("user_data.sqlite");
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(profileDatabaseService),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
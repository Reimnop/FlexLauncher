using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FlexLauncherUI.Services;
using FlexLauncherUI.ViewModels;
using FlexLauncherUI.Views;
using Microsoft.Extensions.DependencyInjection;

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
            var services = new ServiceCollection()
                // .AddSingleton<IDatabaseService>(_ => new SqliteDatabaseService("user_data.sqlite"));
                .AddSingleton<IDatabaseService, DebugDatabaseService>();
            
            var serviceProvider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(serviceProvider),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
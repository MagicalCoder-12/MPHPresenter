using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using MPHPresenter.Services;
using MPHPresenter.ViewModels;
using MPHPresenter.Views;

namespace MPHPresenter
{
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure dependency injection
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<DatabaseService>();
                    services.AddSingleton<ProjectionService>();
                    services.AddSingleton<MediaService>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<SongViewModel>();
                    services.AddSingleton<BibleViewModel>();
                    services.AddSingleton<MediaViewModel>();
                    services.AddSingleton<ProjectionViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();

            ServiceProvider = host.Services;

            // Create and show the main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
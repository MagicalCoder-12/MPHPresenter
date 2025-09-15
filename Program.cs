using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Windows;
using MPHPresenter.Services;
using MPHPresenter.ViewModels;
using MPHPresenter.Views;

namespace MPHPresenter
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
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
                })
                .Build();

            // Create application
            var app = new App();
            app.Run();
        }
    }
}
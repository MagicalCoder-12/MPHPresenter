using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using MPHPresenter.Services;

namespace MPHPresenter.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ProjectionService _projectionService;
        private bool _isDarkTheme = true;

        public SongViewModel SongViewModel { get; set; }
        public BibleViewModel BibleViewModel { get; set; }
        public MediaViewModel MediaViewModel { get; set; }

        public RelayCommand StartProjectionCommand { get; }
        public RelayCommand StopProjectionCommand { get; }
        public RelayCommand ToggleThemeCommand { get; }
        public RelayCommand ExitCommand { get; }

        public MainWindowViewModel()
        {
            // Initialize view models
            var databaseService = new DatabaseService();
            _projectionService = new ProjectionService();
            
            SongViewModel = new SongViewModel(databaseService, _projectionService);
            BibleViewModel = new BibleViewModel(databaseService, _projectionService);
            MediaViewModel = new MediaViewModel(databaseService, _projectionService);

            // Commands
            StartProjectionCommand = new RelayCommand(() => _projectionService.ShowProjection());
            StopProjectionCommand = new RelayCommand(() => _projectionService.HideProjection());
            ToggleThemeCommand = new RelayCommand(ToggleTheme);
            ExitCommand = new RelayCommand(ExitApp);
        }

        public void SetProjectionService(ProjectionService projectionService)
        {
            SongViewModel = new SongViewModel(new DatabaseService(), projectionService);
            BibleViewModel = new BibleViewModel(new DatabaseService(), projectionService);
            MediaViewModel = new MediaViewModel(new DatabaseService(), projectionService);
        }

        private void ToggleTheme()
        {
            _isDarkTheme = !_isDarkTheme;
            // In a real app, we'd apply theme changes here
            // For now, just toggle the flag
            MessageBox.Show($"Theme toggled to: {(_isDarkTheme ? "Dark" : "Light")}");
        }

        private void ExitApp()
        {
            _projectionService?.Dispose();
            Application.Current.Shutdown();
        }
    }
}
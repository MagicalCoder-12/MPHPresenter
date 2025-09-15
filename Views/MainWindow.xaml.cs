using System.Windows;
using MPHPresenter.Services;
using MPHPresenter.ViewModels;

namespace MPHPresenter.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Ensure projection service is initialized
            var projectionService = new ProjectionService();
            var viewModel = (MainWindowViewModel)DataContext;
            viewModel.SetProjectionService(projectionService);
        }
    }
}
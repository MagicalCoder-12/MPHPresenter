using MPHPresenter.ViewModels;
using System;
using System.Windows;

namespace MPHPresenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                // For now, let's not set the DataContext to see if that's causing the issue
                // DataContext = new MainViewModel();
                this.Loaded += MainWindow_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing MainWindow: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // This will be called when the window is loaded
            System.Diagnostics.Debug.WriteLine("MainWindow loaded successfully");
        }
    }
}
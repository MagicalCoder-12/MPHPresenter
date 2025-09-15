using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using MPHPresenter.ViewModels;

namespace MPHPresenter.Views
{
    public partial class ProjectionWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer? _hideTimer;
        private bool _controlsVisible = false;

        public ProjectionWindow()
        {
            InitializeComponent();
            
            // Set up timer to hide controls after inactivity
            _hideTimer = new System.Windows.Threading.DispatcherTimer();
            _hideTimer.Interval = TimeSpan.FromSeconds(3);
            _hideTimer.Tick += HideControlsTimer_Tick!;
            
            // Handle keyboard shortcuts
            KeyDown += ProjectionWindow_KeyDown!;
            
            // Initialize with black background
            ContentContainer.Background = new SolidColorBrush(Colors.Black);
        }

        private void ProjectionWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                // Toggle play/pause for media
                var mediaElement = FindName("VideoContent") as MediaElement;
                if (mediaElement != null)
                {
                    if (mediaElement.Source != null)
                    {
                        if (mediaElement.Position.TotalSeconds > 0)
                        {
                            if (mediaElement.IsPlaying())
                                mediaElement.Pause();
                            else
                                mediaElement.Play();
                        }
                    }
                }
                
                e.Handled = true;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_controlsVisible)
            {
                ControlOverlay.Visibility = Visibility.Visible;
                _controlsVisible = true;
                _hideTimer?.Stop();
                _hideTimer?.Start();
            }
        }

        private void HideControlsTimer_Tick(object? sender, EventArgs e)
        {
            ControlOverlay.Visibility = Visibility.Collapsed;
            _controlsVisible = false;
            _hideTimer?.Stop();
        }

        private void ToggleDisplayModeButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ProjectionViewModel)DataContext;
            vm.ToggleDisplayMode();
            
            // Apply display mode
            switch (vm.DisplayMode)
            {
                case "Normal":
                    ContentContainer.Background = new SolidColorBrush(Colors.Black);
                    break;
                case "BlackBorder":
                    ContentContainer.Background = new SolidColorBrush(Colors.Black);
                    break;
                case "BlurBorder":
                    // Apply blur effect
                    var blurEffect = new BlurEffect { Radius = 10 };
                    ContentContainer.Effect = blurEffect;
                    break;
            }
        }

        private void HideProjectionButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _hideTimer?.Stop();
            _hideTimer = null;
        }

    }

    public static class MediaElementExtensions
    {
        // Extension method to check if MediaElement is playing
        public static bool IsPlaying(this MediaElement mediaElement)
        {
            return mediaElement.NaturalDuration.HasTimeSpan && 
                   mediaElement.Position.TotalSeconds > 0 &&
                   mediaElement.Position < mediaElement.NaturalDuration.TimeSpan;
        }
    }
}
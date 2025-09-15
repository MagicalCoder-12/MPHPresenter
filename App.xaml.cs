using System;
using System.Windows;

namespace MPHPresenter
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Set DPI awareness for better scaling on high-DPI displays
            System.Windows.Interop.HwndSource hwndSource = PresentationSource.FromVisual(Application.Current.MainWindow) as System.Windows.Interop.HwndSource;
            if (hwndSource != null)
            {
                System.Windows.Interop.HwndTarget hwndTarget = hwndSource.CompositionTarget;
                if (hwndTarget != null)
                {
                    // Enable DPI awareness
                    System.Windows.Interop.DpiHelper.EnableDpiAwareness();
                }
            }
        }
    }
}
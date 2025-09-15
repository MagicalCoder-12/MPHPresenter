using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using MPHPresenter.Services;

namespace MPHPresenter.ViewModels
{
    public partial class ProjectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _content = "";

        [ObservableProperty]
        private string _contentType = "";

        [ObservableProperty]
        private string _displayMode = "Normal"; // Normal, BlackBorder, BlurBorder

        private readonly ProjectionService _projectionService;

        public ProjectionViewModel(ProjectionService projectionService)
        {
            _projectionService = projectionService;
            _projectionService.OnProjectionContentChanged += OnProjectionContentChanged;
            _projectionService.OnProjectionVisibilityChanged += OnProjectionVisibilityChanged;
        }

        private void OnProjectionContentChanged(string content)
        {
            Content = content;
        }

        private void OnProjectionVisibilityChanged(bool isVisible)
        {
            // Handle visibility changes if needed
        }

        public void ToggleDisplayMode()
        {
            var modes = new[] { "Normal", "BlackBorder", "BlurBorder" };
            var currentIndex = Array.IndexOf(modes, DisplayMode);
            var nextIndex = (currentIndex + 1) % modes.Length;
            DisplayMode = modes[nextIndex];
        }

        public void CloseProjection()
        {
            _projectionService.HideProjection();
        }
    }
}
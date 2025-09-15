using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MPHPresenter.Models;
using MPHPresenter.Services;

namespace MPHPresenter.ViewModels
{
    public partial class MediaViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MediaItemModel> _mediaItems = new();

        [ObservableProperty]
        private MediaItemModel? _selectedMediaItem;

        private readonly DatabaseService _databaseService;
        private readonly ProjectionService _projectionService;

        public RelayCommand SendToProjectionCommand { get; }
        public RelayCommand RefreshMediaCommand { get; }

        public MediaViewModel(DatabaseService databaseService, ProjectionService projectionService)
        {
            _databaseService = databaseService;
            _projectionService = projectionService;

            SendToProjectionCommand = new RelayCommand(SendToProjection, () => SelectedMediaItem != null);
            RefreshMediaCommand = new RelayCommand(RefreshMedia);

            RefreshMedia();
            _projectionService.OnProjectionContentChanged += OnProjectionContentChanged;
        }

        private void RefreshMedia()
        {
            MediaItems = new ObservableCollection<MediaItemModel>(_databaseService.GetMediaItems());
            SelectedMediaItem = null;
        }

        private void SendToProjection()
        {
            if (SelectedMediaItem != null)
            {
                _projectionService.SendMediaToProjection(SelectedMediaItem);
            }
        }

        private void OnProjectionContentChanged(string content)
        {
            // If projection shows different content, clear selection
            if (string.IsNullOrEmpty(content))
            {
                SelectedMediaItem = null;
            }
        }
    }
}
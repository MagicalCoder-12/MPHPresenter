using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MPHPresenter.Models;
using MPHPresenter.Services;

namespace MPHPresenter.ViewModels
{
    public partial class SongViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private ObservableCollection<SongModel> _songs = new();

        [ObservableProperty]
        private SongModel? _selectedSong;

        [ObservableProperty]
        private string _categoryFilter = "All";

        private readonly DatabaseService _databaseService;
        private readonly ProjectionService _projectionService;

        public RelayCommand AddSongCommand { get; }
        public RelayCommand EditSongCommand { get; }
        public RelayCommand DeleteSongCommand { get; }
        public RelayCommand SendToProjectionCommand { get; }
        public RelayCommand ClearSearchCommand { get; }

        public string[] Categories => new[] { "All", "Hymns", "Worship", "Choir", "Special" };

        public SongViewModel(DatabaseService databaseService, ProjectionService projectionService)
        {
            _databaseService = databaseService;
            _projectionService = projectionService;

            AddSongCommand = new RelayCommand(AddSong);
            EditSongCommand = new RelayCommand(EditSong, () => SelectedSong != null);
            DeleteSongCommand = new RelayCommand(DeleteSong, () => SelectedSong != null);
            SendToProjectionCommand = new RelayCommand(SendToProjection, () => SelectedSong != null);
            ClearSearchCommand = new RelayCommand(ClearSearch);

            LoadSongs();
            _projectionService.OnProjectionContentChanged += OnProjectionContentChanged;
        }

        private void LoadSongs()
        {
            Songs = new ObservableCollection<SongModel>(_databaseService.GetSongs());
            SelectedSong = null;
        }

        private void AddSong()
        {
            var newSong = new SongModel
            {
                Title = "New Song",
                Lyrics = "Enter lyrics here...",
                Category = ""
            };

            _databaseService.AddSong(newSong);
            LoadSongs();
            SelectedSong = newSong;
        }

        private void EditSong()
        {
            if (SelectedSong == null) return;

            var dialog = new SongEditDialog(SelectedSong);
            var result = dialog.ShowDialog();

            if (result == true)
            {
                _databaseService.UpdateSong(SelectedSong);
                LoadSongs();
            }
        }

        private void DeleteSong()
        {
            if (SelectedSong == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedSong.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                _databaseService.DeleteSong(SelectedSong.Id);
                LoadSongs();
            }
        }

        private void SendToProjection()
        {
            if (SelectedSong != null)
            {
                _projectionService.SendSongToProjection(SelectedSong);
            }
        }

        private void ClearSearch()
        {
            SearchText = "";
        }

        private void OnProjectionContentChanged(string content)
        {
            // If projection shows different content, clear selection
            if (string.IsNullOrEmpty(content))
            {
                SelectedSong = null;
            }
        }

        public void RefreshSongs()
        {
            LoadSongs();
        }

        public ObservableCollection<SongModel> FilteredSongs
        {
            get
            {
                var filtered = Songs.AsEnumerable();
                
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filtered = filtered.Where(s => 
                        s.Title.ToLower().Contains(SearchText.ToLower()) || 
                        s.Lyrics.ToLower().Contains(SearchText.ToLower()));
                }

                if (CategoryFilter != "All")
                {
                    filtered = filtered.Where(s => s.Category == CategoryFilter);
                }

                return new ObservableCollection<SongModel>(filtered);
            }
        }
    }

    public class SongEditDialog : Window
    {
        private readonly SongModel _song;

        public SongEditDialog(SongModel song)
        {
            DataContext = this;
            _song = song;
            Title = "Edit Song";
        }

        public new string Title
        {
            get => _song.Title;
            set => _song.Title = value;
        }

        public string Lyrics
        {
            get => _song.Lyrics;
            set => _song.Lyrics = value;
        }

        public string Category
        {
            get => _song.Category;
            set => _song.Category = value;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Please enter a title for the song.", "Missing Title", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
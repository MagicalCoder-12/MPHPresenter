using MPHPresenter.Models;
using MPHPresenter.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MPHPresenter.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly SongService _songService;
        private SongModel? _selectedSong;
        private string? _searchText;

        public ObservableCollection<SongModel> Songs { get; set; }

        public SongModel? SelectedSong
        {
            get => _selectedSong;
            set
            {
                _selectedSong = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSongSelected));
            }
        }

        public bool IsSongSelected => SelectedSong != null;

        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterSongs();
            }
        }

        public ICommand AddSongCommand { get; }
        public ICommand EditSongCommand { get; }
        public ICommand DeleteSongCommand { get; }
        public ICommand ShowOnScreenCommand { get; }

        public MainViewModel()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Initializing MainViewModel...");
                _songService = new SongService();
                Songs = new ObservableCollection<SongModel>();
                LoadSongs();

                AddSongCommand = new RelayCommand(AddSong);
                EditSongCommand = new RelayCommand(EditSong, () => IsSongSelected);
                DeleteSongCommand = new RelayCommand(DeleteSong, () => IsSongSelected);
                ShowOnScreenCommand = new RelayCommand(ShowOnScreen, () => IsSongSelected);
                System.Diagnostics.Debug.WriteLine("MainViewModel initialized successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing MainViewModel: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void LoadSongs()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Loading songs...");
                var songs = _songService.LoadSongs();
                Songs.Clear();
                foreach (var song in songs)
                {
                    Songs.Add(song);
                }
                System.Diagnostics.Debug.WriteLine($"Loaded {songs.Count} songs");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading songs: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void FilterSongs()
        {
            // This is a simplified filter implementation
            // In a real application, you might want to preserve the original list
            LoadSongs();
        }

        private void AddSong()
        {
            try
            {
                var newSong = new SongModel { Title = "New Song", Lyrics = "Lyrics here..." };
                Songs.Add(newSong);
                SelectedSong = newSong;
                SaveSongs();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding song: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void EditSong()
        {
            // In a real application, you would show a dialog to edit the song
            // For now, we'll just trigger a save to demonstrate the functionality
            SaveSongs();
        }

        private void DeleteSong()
        {
            try
            {
                if (SelectedSong != null)
                {
                    Songs.Remove(SelectedSong);
                    SelectedSong = Songs.FirstOrDefault();
                    SaveSongs();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting song: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void ShowOnScreen()
        {
            try
            {
                if (SelectedSong != null)
                {
                    // Create and show the projection window
                    var projectionWindow = new ProjectionWindow();
                    projectionWindow.ProjectionViewModel.CurrentSong = SelectedSong;
                    projectionWindow.Show();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing on screen: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void SaveSongs()
        {
            try
            {
                _songService.SaveSongs(Songs.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving songs: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
            // Update command states when properties change
            if (propertyName == nameof(SelectedSong))
            {
                ((RelayCommand)EditSongCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteSongCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ShowOnScreenCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
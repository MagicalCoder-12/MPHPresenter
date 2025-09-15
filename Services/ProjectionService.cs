using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MPHPresenter.Models;

namespace MPHPresenter.Services
{
    public class ProjectionService : IDisposable
    {
        private ProjectionWindow? _projectionWindow;
        private readonly object _lock = new object();
        private bool _isInitialized = false;

        public event Action<string> OnProjectionContentChanged;
        public event Action<bool> OnProjectionVisibilityChanged;

        public string CurrentContent { get; private set; } = "";
        public string CurrentContentType { get; private set; } = "";
        public object? CurrentMediaSource { get; private set; }

        public void Initialize()
        {
            if (_isInitialized) return;

            // Create projection window on the secondary display if available
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 1)
            {
                var secondaryScreen = screens[1];
                _projectionWindow = new ProjectionWindow
                {
                    Left = secondaryScreen.Bounds.Left,
                    Top = secondaryScreen.Bounds.Top,
                    Width = secondaryScreen.Bounds.Width,
                    Height = secondaryScreen.Bounds.Height
                };
            }
            else
            {
                // Fallback: use main screen in fullscreen mode
                _projectionWindow = new ProjectionWindow();
            }

            _projectionWindow.Closed += (s, e) => 
            {
                lock (_lock)
                {
                    _projectionWindow = null;
                    OnProjectionVisibilityChanged?.Invoke(false);
                }
            };

            _isInitialized = true;
        }

        public void ShowProjection()
        {
            if (!_isInitialized) Initialize();

            lock (_lock)
            {
                if (_projectionWindow != null && !_projectionWindow.IsVisible)
                {
                    _projectionWindow.Show();
                    _projectionWindow.WindowState = WindowState.Maximized;
                    _projectionWindow.Focus();
                    OnProjectionVisibilityChanged?.Invoke(true);
                }
            }
        }

        public void HideProjection()
        {
            lock (_lock)
            {
                if (_projectionWindow != null && _projectionWindow.IsVisible)
                {
                    _projectionWindow.Hide();
                    OnProjectionVisibilityChanged?.Invoke(false);
                }
            }
        }

        public void SendSongToProjection(SongModel song)
        {
            if (song == null) return;

            lock (_lock)
            {
                CurrentContent = song.Lyrics.Replace("\n", "<br/>");
                CurrentContentType = "song";
                CurrentMediaSource = null;
                
                OnProjectionContentChanged?.Invoke(CurrentContent);
                ShowProjection();
            }
        }

        public void SendBibleVerseToProjection(BibleVerseModel verse)
        {
            if (verse == null) return;

            lock (_lock)
            {
                CurrentContent = $"{verse.Book} {verse.Chapter}:{verse.Verse}<br/>{verse.Text}";
                CurrentContentType = "bible";
                CurrentMediaSource = null;
                
                OnProjectionContentChanged?.Invoke(CurrentContent);
                ShowProjection();
            }
        }

        public void SendMediaToProjection(MediaItemModel mediaItem)
        {
            if (mediaItem == null) return;

            lock (_lock)
            {
                CurrentContent = "";
                CurrentContentType = mediaItem.Type;
                CurrentMediaSource = mediaItem.FilePath;
                
                OnProjectionContentChanged?.Invoke("");
                ShowProjection();
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _projectionWindow?.Close();
                _projectionWindow = null;
            }
        }
    }
}
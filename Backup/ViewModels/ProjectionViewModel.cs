using MPHPresenter.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MPHPresenter.ViewModels
{
    public class ProjectionViewModel : INotifyPropertyChanged
    {
        private SongModel? _currentSong;
        private string? _currentSlide;
        private int _currentSlideIndex;
        private Brush? _backgroundBrush;

        public SongModel? CurrentSong
        {
            get => _currentSong;
            set
            {
                _currentSong = value;
                OnPropertyChanged();
                if (_currentSong != null)
                {
                    // Split lyrics into slides (paragraphs)
                    Slides = _currentSong.Lyrics?.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries) 
                             ?? new string[0];
                    CurrentSlideIndex = 0;
                }
            }
        }

        private string[] _slides = new string[0];

        public string[] Slides
        {
            get => _slides;
            set
            {
                _slides = value;
                OnPropertyChanged();
            }
        }

        public string? CurrentSlide
        {
            get => _currentSlide;
            set
            {
                _currentSlide = value;
                OnPropertyChanged();
            }
        }

        public int CurrentSlideIndex
        {
            get => _currentSlideIndex;
            set
            {
                _currentSlideIndex = value;
                if (_slides != null && _slides.Length > 0 && value >= 0 && value < _slides.Length)
                {
                    CurrentSlide = _slides[value];
                }
                else
                {
                    CurrentSlide = string.Empty;
                }
                OnPropertyChanged();
            }
        }

        public Brush? BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                _backgroundBrush = value;
                OnPropertyChanged();
            }
        }

        public ProjectionViewModel()
        {
            // Set a default background
            BackgroundBrush = Brushes.Black;
        }

        public void NextSlide()
        {
            if (CurrentSlideIndex < Slides.Length - 1)
            {
                CurrentSlideIndex++;
            }
        }

        public void PreviousSlide()
        {
            if (CurrentSlideIndex > 0)
            {
                CurrentSlideIndex--;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
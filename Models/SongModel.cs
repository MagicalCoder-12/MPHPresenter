using System.ComponentModel;

namespace MPHPresenter.Models
{
    public class SongModel : INotifyPropertyChanged
    {
        private int _id;
        private string _title = string.Empty;
        private string _lyrics = string.Empty;
        private string _category = string.Empty;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value ?? string.Empty;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Lyrics
        {
            get => _lyrics;
            set
            {
                _lyrics = value ?? string.Empty;
                OnPropertyChanged(nameof(Lyrics));
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value ?? string.Empty;
                OnPropertyChanged(nameof(Category));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
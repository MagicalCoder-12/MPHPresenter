using System.ComponentModel;

namespace MPHPresenter.Models
{
    public class MediaItemModel : INotifyPropertyChanged
    {
        private int _id;
        private string _filePath = string.Empty;
        private string _fileName = string.Empty;
        private string _type = string.Empty; // "image", "video", "audio"
        private string _thumbnailPath = string.Empty;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value ?? string.Empty;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value ?? string.Empty;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value ?? string.Empty;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string ThumbnailPath
        {
            get => _thumbnailPath;
            set
            {
                _thumbnailPath = value ?? string.Empty;
                OnPropertyChanged(nameof(ThumbnailPath));
            }
        }

        public bool IsImage => Type == "image";
        public bool IsVideo => Type == "video";
        public bool IsAudio => Type == "audio";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
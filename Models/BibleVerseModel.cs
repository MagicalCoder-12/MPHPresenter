using System.ComponentModel;

namespace MPHPresenter.Models
{
    public class BibleVerseModel : INotifyPropertyChanged
    {
        private int _id;
        private string _book;
        private int _chapter;
        private int _verse;
        private string _text;
        private string _translation;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
            }
        }

        public int Chapter
        {
            get => _chapter;
            set
            {
                _chapter = value;
                OnPropertyChanged(nameof(Chapter));
            }
        }

        public int Verse
        {
            get => _verse;
            set
            {
                _verse = value;
                OnPropertyChanged(nameof(Verse));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged(nameof(Translation));
            }
        }

        public string Reference => $"{Book} {Chapter}:{Verse}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
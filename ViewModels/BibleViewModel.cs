using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MPHPresenter.Models;
using MPHPresenter.Services;

namespace MPHPresenter.ViewModels
{
    public partial class BibleViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private ObservableCollection<BibleVerseModel> _verses = new();

        [ObservableProperty]
        private BibleVerseModel? _selectedVerse;

        [ObservableProperty]
        private string _selectedBook = "";

        [ObservableProperty]
        private int _selectedChapter = 1;

        [ObservableProperty]
        private string _selectedTranslation = "KJV";

        private readonly DatabaseService _databaseService;
        private readonly ProjectionService _projectionService;

        public ObservableCollection<string> Books { get; private set; } = new();
        public ObservableCollection<string> Translations { get; private set; } = new();

        public RelayCommand SearchCommand { get; }
        public RelayCommand SendToProjectionCommand { get; }
        public RelayCommand ClearSearchCommand { get; }
        public RelayCommand LoadBookCommand { get; }

        public BibleViewModel(DatabaseService databaseService, ProjectionService projectionService)
        {
            _databaseService = databaseService;
            _projectionService = projectionService;

            SearchCommand = new RelayCommand(SearchBible);
            SendToProjectionCommand = new RelayCommand(SendToProjection, () => SelectedVerse != null);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            LoadBookCommand = new RelayCommand(LoadBookBySelection);

            Translations = new ObservableCollection<string>(_databaseService.GetTranslations());
            Books = new ObservableCollection<string>(_databaseService.GetBooks());

            if (Books.Count > 0)
            {
                SelectedBook = Books[0];
            }

            _projectionService.OnProjectionContentChanged += OnProjectionContentChanged;
        }

        private void SearchBible()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Verses = new ObservableCollection<BibleVerseModel>();
                return;
            }

            Verses = new ObservableCollection<BibleVerseModel>(
                _databaseService.SearchBible(SearchText, SelectedTranslation));
        }

        private void SendToProjection()
        {
            if (SelectedVerse != null)
            {
                _projectionService.SendBibleVerseToProjection(SelectedVerse);
            }
        }

        private void ClearSearch()
        {
            SearchText = "";
            Verses = new ObservableCollection<BibleVerseModel>();
        }

        private void LoadBookBySelection()
        {
            if (string.IsNullOrWhiteSpace(SelectedBook) || SelectedChapter < 1)
                return;

            Verses = new ObservableCollection<BibleVerseModel>(
                _databaseService.GetVersesByBookChapter(SelectedBook, SelectedChapter, SelectedTranslation));
        }

        private void OnProjectionContentChanged(string content)
        {
            // If projection shows different content, clear selection
            if (string.IsNullOrEmpty(content))
            {
                SelectedVerse = null;
            }
        }

        public void RefreshBible()
        {
            Books = new ObservableCollection<string>(_databaseService.GetBooks());
            Translations = new ObservableCollection<string>(_databaseService.GetTranslations());
            
            if (Books.Count > 0 && string.IsNullOrWhiteSpace(SelectedBook))
            {
                SelectedBook = Books[0];
            }
        }
    }
}
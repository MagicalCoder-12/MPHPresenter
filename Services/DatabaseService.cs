using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.IO;
using MPHPresenter.Models;

namespace MPHPresenter.Services
{
    public class DatabaseService
    {
        private readonly string _songsDbPath;
        private readonly string _bibleDbPath;

        public DatabaseService()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MPHPresenter");
            Directory.CreateDirectory(appDataPath);

            _songsDbPath = Path.Combine(appDataPath, "demo_songs.db");
            _bibleDbPath = Path.Combine(appDataPath, "demo_bible.db");

            InitializeDatabases();
        }

        private void InitializeDatabases()
        {
            // Initialize songs database
            if (!File.Exists(_songsDbPath))
            {
                CreateSongsDatabase();
                SeedSongsData();
            }

            // Initialize Bible database
            if (!File.Exists(_bibleDbPath))
            {
                CreateBibleDatabase();
                SeedBibleData();
            }
        }

        private void CreateSongsDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Songs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Lyrics TEXT NOT NULL,
                    Category TEXT
                );";
            command.ExecuteNonQuery();
        }

        private void CreateBibleDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Verses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Book TEXT NOT NULL,
                    Chapter INTEGER NOT NULL,
                    Verse INTEGER NOT NULL,
                    Text TEXT NOT NULL,
                    Translation TEXT DEFAULT 'KJV'
                );
                
                CREATE INDEX IF NOT EXISTS idx_book ON Verses(Book);
                CREATE INDEX IF NOT EXISTS idx_chapter ON Verses(Chapter);
                CREATE INDEX IF NOT EXISTS idx_verse ON Verses(Verse);";
            command.ExecuteNonQuery();
        }

        private void SeedSongsData()
        {
            var songs = new[]
            {
                new { Title = "Amazing Grace", Lyrics = "Amazing grace! How sweet the sound\nThat saved a wretch like me!\nI once was lost, but now am found,\nWas blind, but now I see.", Category = "Hymns" },
                new { Title = "How Great Thou Art", Lyrics = "O Lord my God, when I in awesome wonder\nConsider all the worlds Thy Hands have made;\nI see the stars, I hear the rolling thunder,\nThy power throughout the universe displayed.", Category = "Hymns" },
                new { Title = "It Is Well With My Soul", Lyrics = "When peace, like a river, attendeth my way,\nWhen sorrows like sea billows roll;\nWhatever my lot, Thou hast taught me to say,\nIt is well, it is well, with my soul.", Category = "Hymns" },
                new { Title = "Rejoice Always", Lyrics = "Rejoice always, pray without ceasing,\nIn everything give thanks;\nFor this is the will of God in Christ Jesus\nConcerning you all.", Category = "Worship" },
                new { Title = "Holy, Holy, Holy", Lyrics = "Holy, holy, holy! Lord God Almighty!\nEarly in the morning our song shall rise to Thee;\nHoly, holy, holy, merciful and mighty!\nGod in three persons, blessed Trinity!", Category = "Hymns" }
            };

            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            foreach (var song in songs)
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Songs (Title, Lyrics, Category) VALUES (@title, @lyrics, @category)";
                command.Parameters.AddWithValue("@title", song.Title);
                command.Parameters.AddWithValue("@lyrics", song.Lyrics);
                command.Parameters.AddWithValue("@category", song.Category);
                command.ExecuteNonQuery();
            }
        }

        private void SeedBibleData()
        {
            var verses = new[]
            {
                new { Book = "John", Chapter = 3, Verse = 16, Text = "For God so loved the world that He gave His only begotten Son, that whoever believes in Him should not perish but have everlasting life.", Translation = "KJV" },
                new { Book = "John", Chapter = 3, Verse = 17, Text = "For God did not send His Son into the world to condemn the world, but that the world through Him might be saved.", Translation = "KJV" },
                new { Book = "Philippians", Chapter = 4, Verse = 6, Text = "Be anxious for nothing, but in everything by prayer and supplication, with thanksgiving, let your requests be made known to God;", Translation = "KJV" },
                new { Book = "Philippians", Chapter = 4, Verse = 7, Text = "and the peace of God, which surpasses all understanding, will guard your hearts and minds through Christ Jesus.", Translation = "KJV" },
                new { Book = "Romans", Chapter = 8, Verse = 28, Text = "And we know that all things work together for good to those who love God, to those who are the called according to His purpose.", Translation = "KJV" },
                new { Book = "Psalm", Chapter = 23, Verse = 1, Text = "The LORD is my shepherd; I shall not want.", Translation = "KJV" },
                new { Book = "Psalm", Chapter = 23, Verse = 2, Text = "He makes me to lie down in green pastures; He leads me beside the still waters.", Translation = "KJV" },
                new { Book = "1 Corinthians", Chapter = 13, Verse = 4, Text = "Love suffers long and is kind; love does not envy; love does not parade itself, is not puffed up;", Translation = "KJV" },
                new { Book = "1 Corinthians", Chapter = 13, Verse = 5, Text = "does not behave rudely, does not seek its own, is not provoked, thinks no evil;", Translation = "KJV" },
                new { Book = "Matthew", Chapter = 28, Verse = 19, Text = "Go therefore and make disciples of all the nations, baptizing them in the name of the Father and of the Son and of the Holy Spirit,", Translation = "KJV" },
                new { Book = "Matthew", Chapter = 28, Verse = 20, Text = "teaching them to observe all things that I have commanded you; and lo, I am with you always, even to the end of the age. Amen.", Translation = "KJV" }
            };

            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            foreach (var verse in verses)
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Verses (Book, Chapter, Verse, Text, Translation) VALUES (@book, @chapter, @verse, @text, @translation)";
                command.Parameters.AddWithValue("@book", verse.Book);
                command.Parameters.AddWithValue("@chapter", verse.Chapter);
                command.Parameters.AddWithValue("@verse", verse.Verse);
                command.Parameters.AddWithValue("@text", verse.Text);
                command.Parameters.AddWithValue("@translation", verse.Translation);
                command.ExecuteNonQuery();
            }
        }

        public ObservableCollection<SongModel> GetSongs()
        {
            var songs = new ObservableCollection<SongModel>();
            
            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Lyrics, Category FROM Songs ORDER BY Title";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                songs.Add(new SongModel
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Lyrics = reader.GetString("Lyrics"),
                    Category = reader.IsDBNull("Category") ? "" : reader.GetString("Category")
                });
            }
            
            return songs;
        }

        public void AddSong(SongModel song)
        {
            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Songs (Title, Lyrics, Category) VALUES (@title, @lyrics, @category)";
            command.Parameters.AddWithValue("@title", song.Title);
            command.Parameters.AddWithValue("@lyrics", song.Lyrics);
            command.Parameters.AddWithValue("@category", string.IsNullOrEmpty(song.Category) ? null : song.Category);
            command.ExecuteNonQuery();

            song.Id = (int)command.LastInsertRowId;
        }

        public void UpdateSong(SongModel song)
        {
            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Songs SET Title = @title, Lyrics = @lyrics, Category = @category WHERE Id = @id";
            command.Parameters.AddWithValue("@title", song.Title);
            command.Parameters.AddWithValue("@lyrics", song.Lyrics);
            command.Parameters.AddWithValue("@category", string.IsNullOrEmpty(song.Category) ? null : song.Category);
            command.Parameters.AddWithValue("@id", song.Id);
            command.ExecuteNonQuery();
        }

        public void DeleteSong(int id)
        {
            using var connection = new SqliteConnection($"Data Source={_songsDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Songs WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public ObservableCollection<BibleVerseModel> SearchBible(string searchTerm, string translation = "KJV")
        {
            var verses = new ObservableCollection<BibleVerseModel>();
            
            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, Book, Chapter, Verse, Text, Translation 
                                   FROM Verses 
                                   WHERE Book LIKE @search OR Text LIKE @search 
                                   AND Translation = @translation
                                   ORDER BY Book, Chapter, Verse";
            
            command.Parameters.AddWithValue("@search", $"%{searchTerm}%");
            command.Parameters.AddWithValue("@translation", translation);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                verses.Add(new BibleVerseModel
                {
                    Id = reader.GetInt32("Id"),
                    Book = reader.GetString("Book"),
                    Chapter = reader.GetInt32("Chapter"),
                    Verse = reader.GetInt32("Verse"),
                    Text = reader.GetString("Text"),
                    Translation = reader.GetString("Translation")
                });
            }
            
            return verses;
        }

        public ObservableCollection<BibleVerseModel> GetVersesByBookChapter(string book, int chapter, string translation = "KJV")
        {
            var verses = new ObservableCollection<BibleVerseModel>();
            
            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, Book, Chapter, Verse, Text, Translation 
                                   FROM Verses 
                                   WHERE Book = @book AND Chapter = @chapter 
                                   AND Translation = @translation
                                   ORDER BY Verse";
            
            command.Parameters.AddWithValue("@book", book);
            command.Parameters.AddWithValue("@chapter", chapter);
            command.Parameters.AddWithValue("@translation", translation);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                verses.Add(new BibleVerseModel
                {
                    Id = reader.GetInt32("Id"),
                    Book = reader.GetString("Book"),
                    Chapter = reader.GetInt32("Chapter"),
                    Verse = reader.GetInt32("Verse"),
                    Text = reader.GetString("Text"),
                    Translation = reader.GetString("Translation")
                });
            }
            
            return verses;
        }

        public ObservableCollection<string> GetBooks()
        {
            var books = new ObservableCollection<string>();
            
            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT Book FROM Verses ORDER BY Book";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                books.Add(reader.GetString("Book"));
            }
            
            return books;
        }

        public ObservableCollection<string> GetTranslations()
        {
            var translations = new ObservableCollection<string>();
            
            using var connection = new SqliteConnection($"Data Source={_bibleDbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT Translation FROM Verses ORDER BY Translation";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                translations.Add(reader.GetString("Translation"));
            }
            
            return translations;
        }

        public ObservableCollection<MediaItemModel> GetMediaItems()
        {
            var mediaItems = new ObservableCollection<MediaItemModel>();
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
            
            if (!Directory.Exists(mediaPath))
                Directory.CreateDirectory(mediaPath);

            // Create sample media files if they don't exist
            CreateSampleMediaFiles(mediaPath);

            // Scan for media files
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
            var videoExtensions = new[] { ".mp4", ".avi", ".mkv" };
            var audioExtensions = new[] { ".mp3", ".wav" };

            var files = Directory.GetFiles(mediaPath);
            
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file).ToLowerInvariant();
                string type = "";
                
                if (imageExtensions.Contains(extension)) type = "image";
                else if (videoExtensions.Contains(extension)) type = "video";
                else if (audioExtensions.Contains(extension)) type = "audio";
                else continue;

                mediaItems.Add(new MediaItemModel
                {
                    Id = mediaItems.Count + 1,
                    FilePath = file,
                    FileName = Path.GetFileName(file),
                    Type = type,
                    ThumbnailPath = type == "image" ? file : ""
                });
            }
            
            return mediaItems;
        }

        private void CreateSampleMediaFiles(string mediaPath)
        {
            // Create sample image
            var imagePath = Path.Combine(mediaPath, "sample_image.png");
            if (!File.Exists(imagePath))
            {
                // Create a simple PNG image (white background with text)
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(800, 600);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    g.Clear(System.Drawing.Color.White);
                    var font = new System.Drawing.Font("Arial", 24);
                    var brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    var text = "Sample Image";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, (800 - size.Width) / 2, (600 - size.Height) / 2);
                }
                bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
            }

            // Create sample audio
            var audioPath = Path.Combine(mediaPath, "sample_audio.mp3");
            if (!File.Exists(audioPath))
            {
                // Create a very small MP3 file (silence)
                byte[] silence = {
                    0x49, 0x44, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };
                File.WriteAllBytes(audioPath, silence);
            }

            // Create sample video
            var videoPath = Path.Combine(mediaPath, "sample_video.mp4");
            if (!File.Exists(videoPath))
            {
                // Create a very small MP4 file (black screen)
                byte[] video = {
                    0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6f, 0x6d, 0x00, 0x00, 0x02, 0x00,
                    0x69, 0x73, 0x6f, 0x6d, 0x69, 0x73, 0x6f, 0x6d, 0x61, 0x76, 0x63, 0x31, 0x00, 0x00, 0x00, 0x08,
                    0x6d, 0x6f, 0x6f, 0x76, 0x00, 0x00, 0x00, 0x6c, 0x6d, 0x76, 0x68, 0x64, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };
                File.WriteAllBytes(videoPath, video);
            }
        }
    }
}
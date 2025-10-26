using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DebugTest
{
    public class SongModel
    {
        public string? Title { get; set; }
        public string? Lyrics { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing SongService functionality...");
            
            try
            {
                // Test JSON serialization/deserialization
                var songs = new List<SongModel>
                {
                    new SongModel { Title = "Amazing Grace", Lyrics = "Amazing grace, how sweet the sound\nThat saved a wretch like me" },
                    new SongModel { Title = "How Great Thou Art", Lyrics = "O Lord my God, when I in awesome wonder\nConsider all the worlds Thy hands have made" }
                };
                
                // Serialize
                var json = JsonConvert.SerializeObject(songs, Formatting.Indented);
                Console.WriteLine("Serialized JSON:");
                Console.WriteLine(json);
                
                // Deserialize
                var deserializedSongs = JsonConvert.DeserializeObject<List<SongModel>>(json);
                Console.WriteLine($"\nDeserialized {deserializedSongs?.Count ?? 0} songs:");
                if (deserializedSongs != null)
                {
                    foreach (var song in deserializedSongs)
                    {
                        Console.WriteLine($"- {song.Title}");
                    }
                }
                
                // Test file operations
                var dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }
                
                var filePath = Path.Combine(dataDir, "test_songs.json");
                File.WriteAllText(filePath, json);
                Console.WriteLine($"\nSaved to file: {filePath}");
                
                var loadedJson = File.ReadAllText(filePath);
                var loadedSongs = JsonConvert.DeserializeObject<List<SongModel>>(loadedJson);
                Console.WriteLine($"\nLoaded from file {loadedSongs?.Count ?? 0} songs:");
                if (loadedSongs != null)
                {
                    foreach (var song in loadedSongs)
                    {
                        Console.WriteLine($"- {song.Title}");
                    }
                }
                
                Console.WriteLine("\nTest completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
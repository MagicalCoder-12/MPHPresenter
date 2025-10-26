using MPHPresenter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPHPresenter.Services
{
    public class SongService
    {
        private readonly string _filePath;

        public SongService()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Initializing SongService...");
                _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "songs.json");
                System.Diagnostics.Debug.WriteLine($"Songs file path: {_filePath}");
                // Ensure Data directory exists
                var dataDir = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(dataDir))
                {
                    System.Diagnostics.Debug.WriteLine($"Creating Data directory: {dataDir}");
                    Directory.CreateDirectory(dataDir!);
                }
                System.Diagnostics.Debug.WriteLine("SongService initialized successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing SongService: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public List<SongModel> LoadSongs()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading songs from: {_filePath}");
                if (!File.Exists(_filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Songs file does not exist, returning empty list");
                    return new List<SongModel>();
                }

                var json = File.ReadAllText(_filePath);
                System.Diagnostics.Debug.WriteLine($"Read JSON: {json}");
                var songs = JsonConvert.DeserializeObject<List<SongModel>>(json) ?? new List<SongModel>();
                System.Diagnostics.Debug.WriteLine($"Deserialized {songs.Count} songs");
                return songs;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading songs: {ex.Message}\n{ex.StackTrace}");
                return new List<SongModel>();
            }
        }

        public void SaveSongs(List<SongModel> songs)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Saving {songs.Count} songs to: {_filePath}");
                var json = JsonConvert.SerializeObject(songs, Formatting.Indented);
                System.Diagnostics.Debug.WriteLine($"JSON to save: {json}");
                File.WriteAllText(_filePath, json);
                System.Diagnostics.Debug.WriteLine("Songs saved successfully");
            }
            catch (Exception ex)
            {
                // In a real application, you might want to log this exception
                System.Diagnostics.Debug.WriteLine($"Error saving songs: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
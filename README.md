# MPHPresenter

A WPF song presentation application built with .NET 8 and C# 12, similar to VideoPsalm.

## Features

- Add, edit, and delete songs with titles and lyrics
- Organize songs in a list for presentation
- Display songs on a secondary monitor in full screen
- Smooth fade-in/out transitions between slides
- Local JSON storage for song data
- Simple UI with intuitive controls

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or newer (optional, for development)

### Running the Application

1. Open a terminal in the project directory
2. Run the following command:
   ```
   dotnet run
   ```

Or open the solution in Visual Studio and press F5 to build and run.

### Using the Application

1. **Adding a Song**:
   - Click the "Add Song" button
   - Edit the title and lyrics in the details panel

2. **Editing a Song**:
   - Select a song from the list
   - Modify the title and lyrics in the details panel

3. **Deleting a Song**:
   - Select a song from the list
   - Click the "Delete Song" button

4. **Presenting on Secondary Screen**:
   - Select a song from the list
   - Click the "Show on Screen 2" button
   - Use arrow keys or spacebar to navigate between slides
   - Press Escape to close the presentation window

## Project Structure

- **Models**: Data models (SongModel)
- **Services**: Business logic (SongService)
- **ViewModels**: View models implementing MVVM pattern (MainViewModel, ProjectionViewModel)
- **Views**: XAML views (MainWindow, ProjectionWindow)
- **Data**: Local storage directory for songs.json

## Technical Details

- **Architecture**: MVVM pattern
- **Framework**: .NET 8 with WPF
- **Language**: C# 12
- **Data Storage**: JSON serialization with Newtonsoft.Json
- **UI**: XAML with data binding

## Dependencies

- Newtonsoft.Json (for JSON serialization)

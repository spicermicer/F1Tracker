using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using F1TournamentTracker.Data;
using F1TournamentTracker.Managers;
using System.Linq;

namespace F1TournamentTracker;

public partial class SeasonCreatorWindow : Window
{
    public SeasonCreatorWindow()
    {
        InitializeComponent();

        DatTracks.ItemsSource = SaveManager.LoadTracks();
    }

    public TrackInfo[] SelectedTracks { get; set; }



    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Close(false);
    private void Confirm_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tracks = DatTracks.SelectedItems.Cast<TrackInfo>().ToArray();
        if (tracks is null || tracks.Length == 0)
            return;

        if (ChkShuffle.IsChecked == true)
        {
            var r = new System.Random();
            tracks = [.. tracks.OrderBy(a => r.Next())];
        }

        SelectedTracks = tracks;
        Close(true);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using F1TournamentTracker.Data;
using F1TournamentTracker.Managers;
using System.IO;

namespace F1TournamentTracker;

public partial class ImporterWindow : Window
{
    public ImporterWindow()
    {
        InitializeComponent();

        cmbRaces.ItemsSource = SaveManager.LoadTracks();
        cmbRaces.SelectedIndex = 0;
    }

    private async void Browse_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this)!;

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            var path = files[0].Path.LocalPath;
            txtPath.Text = path;
        }
    }

    public string SelectedPath { get; set; }
    public TrackInfo SelectedTrack { get; set;}

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(false);
    }

    private void Confirm_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SelectedPath = txtPath.Text;
        SelectedTrack = cmbRaces.SelectedItem as TrackInfo;


        Close(true);
    }



}
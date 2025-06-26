using Avalonia.Controls;
using Avalonia.Platform.Storage;
using F1TournamentTracker.Data;
using F1TournamentTracker.Data.Raw;
using F1TournamentTracker.Managers;
using System;
using System.IO;
using System.Linq;

namespace F1TournamentTracker;

public partial class ImporterWindow : Window
{
    public ImporterWindow()
    {
        InitializeComponent();

        var tracks = SaveManager.LoadTracks();
        cmbRaces.ItemsSource = tracks;
        cmbRaces.SelectedIndex = 0;
        cmbRaces.SelectionChanged += CmbRaces_SelectionChanged;

        txtPath.TextChanged += TxtPath_TextChanged;
        txtContents.TextChanged += TxtContents_TextChanged;

        SelectedTrack = tracks.First();
    }

    private void CmbRaces_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        SelectedTrack = (cmbRaces.SelectedItem as TrackInfo)!;
    }

    private void TxtContents_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ImportData = txtContents.Text;

        bool isValid = true;
        try
        {
            var test = CsvParser.Open(txtContents.Text, SelectedTrack);
            isValid = test.CheckValidity();            
        }
        catch (Exception ex)
        {
            isValid = false;
        }

        txtError.IsVisible = !isValid;
        btnConfirm.IsEnabled = isValid;
    }

    private void TxtPath_TextChanged(object? sender, TextChangedEventArgs e)
    {
        SelectedPath = txtPath.Text!;

        if (File.Exists(txtPath.Text))
            txtContents.Text = File.ReadAllText(txtPath.Text);
    }

    private async void Browse_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this)!;

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

    public string SelectedPath { get; set; } = string.Empty;
    public TrackInfo SelectedTrack { get; set;} = new TrackInfo();
    public string ImportData { get; set; } = string.Empty;

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(false);
    }

    private void Confirm_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(true);
    }



}
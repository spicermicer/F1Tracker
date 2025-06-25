using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using F1TournamentTracker.Data;
using F1TournamentTracker.Data.Raw;
using F1TournamentTracker.Managers;
using F1TournamentTracker.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using F1TournamentTracker.Misc;
using System.Linq;
using F1TournamentTracker.Graph;

namespace F1TournamentTracker.ViewModels;

public class MainViewModel : ViewModelBase
{    
    private TeamInfo[] _teamData;

    
    public Axis[] XAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) }];
    public Axis[] YAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)), MinLimit = 0 }];
    public Axis[] NoAxes { get; set; } = [new Axis { IsVisible = false }];    

    private ISeries[] _championshipRace;
    public ISeries[] ChampionshipRace
    {
        get => _championshipRace;
        set => this.RaiseAndSetIfChanged(ref _championshipRace, value);
    }

    private ISeries[] _constructorsStandings;
    public ISeries[] ConstructorsStandings
    {
        get => _constructorsStandings;
        set => this.RaiseAndSetIfChanged(ref _constructorsStandings, value);
    }

    private string[] _penalties;
    public string[] Penalties
    {
        get => _penalties;
        set => this.RaiseAndSetIfChanged(ref _penalties, value);
    }

    public MainViewModel()
    {
        Loadup();
    }

    private string _dataDir = "Data";

    public async void Import()
    {
        var window = new ImporterWindow();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            await window.ShowDialog(desktopLifetime.MainWindow);

        if (string.IsNullOrWhiteSpace(window.SelectedPath))
            return;
        
        System.IO.Directory.CreateDirectory(_dataDir);
        System.IO.File.Copy(window.SelectedPath, System.IO.Path.Combine(_dataDir, window.SelectedTrack.Name) + ".csv");

        Loadup();
    }

    private RaceInfo[] _races;
    public RaceInfo[] Races
    {
        get => _races;
        set => this.RaiseAndSetIfChanged(ref _races, value);
    }

    public SolidColorPaint LegendTextPaint { get; set; } =
        new SolidColorPaint
        {
            Color = new SKColor(255, 255, 255),
        };

    public void Loadup()
    {
        var tracks = SaveManager.LoadTracks();

        Races = System.IO.Directory
            .GetFiles(_dataDir, "*.csv")
            .Select(CsvParser.Open)
            .OrderBy(a => tracks.IndexOf(b => b.Name == a.Track.Name))
            .ToArray();

        ChampionshipRace = GraphManager.GenerateChampionshipRace(Races);
        ConstructorsStandings = GraphManager.GenerateConstructorChampionship(Races);
        Penalties = GraphManager.GeneratePenaltyCount(Races);

        
    }



    public void Teams()
    {
        var window = new TeamManager(_teamData);
        window.Show();
    }    
}

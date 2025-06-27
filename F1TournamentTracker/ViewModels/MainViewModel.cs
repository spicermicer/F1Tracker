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
using System.IO;

namespace F1TournamentTracker.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly string _dataDir = "Data/Races";


    public Axis[] XAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) }];
    public Axis[] YAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)), MinLimit = 0 }];
    public Axis[] NoAxes { get; set; } = [new Axis { IsVisible = false }];

    private TeamInfo[] _teamData = [];
    public TeamInfo[] TeamData
    {
        get => _teamData;
        set => this.RaiseAndSetIfChanged(ref _teamData, value);
    }

    private TrackInfo[] _trackData = [];
    public TrackInfo[] TrackData
    {
        get => _trackData;
        set => this.RaiseAndSetIfChanged(ref _trackData, value);
    }

    private ISeries[] _championshipRace = [];
    public ISeries[] ChampionshipRace
    {
        get => _championshipRace;
        set => this.RaiseAndSetIfChanged(ref _championshipRace, value);
    }

    private ISeries[] _constructorsStandings = [];
    public ISeries[] ConstructorsStandings
    {
        get => _constructorsStandings;
        set => this.RaiseAndSetIfChanged(ref _constructorsStandings, value);
    }

    private string[] _penalties = [];
    public string[] Penalties
    {
        get => _penalties;
        set => this.RaiseAndSetIfChanged(ref _penalties, value);
    }        


    private RaceInfo[] _races = [];
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


    public MainViewModel()
    {
        Load();
    }

    public void Load()
    {
        var tracks = SaveManager.LoadTracks();

        if (Directory.Exists(_dataDir))
        {
            Races = System.IO.Directory
                .GetFiles(_dataDir, "*.csv")
                .Select(CsvParser.Open)
                .OrderBy(a => tracks.IndexOf(b => b.Name == a!.Track.Name))
                .ToArray()!;

            ChampionshipRace = GraphManager.GenerateChampionshipRace(Races);
            ConstructorsStandings = GraphManager.GenerateConstructorChampionship(Races);
            Penalties = GraphManager.GeneratePenaltyCount(Races);
        }

        TeamData = SaveManager.LoadTeams();
        TrackData = SaveManager.LoadTracks();
    }

    public void Save()
    {
        SaveManager.Save(TrackData);
        SaveManager.Save(TeamData);

        Load();
    }

    public async void Import()
    {
        var window = new ImporterWindow();
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            if (await window.ShowDialog<bool>(desktopLifetime.MainWindow!) == false)
                return;

        if (string.IsNullOrWhiteSpace(window.SelectedPath))
            return;

        
        var path = System.IO.Path.Combine(_dataDir, window.SelectedTrack.Name) + ".csv";
        System.IO.Directory.CreateDirectory(_dataDir);
        System.IO.File.WriteAllText(path, window.ImportData);

        Load();
    }

}

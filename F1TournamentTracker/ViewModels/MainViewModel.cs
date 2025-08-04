using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using F1TournamentTracker.Data;
using F1TournamentTracker.Data.Raw;
using F1TournamentTracker.Managers;
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

    private Axis[] _raceXAxes = [];
    private Axis[] _raceYAxes = [];


    public Axis[] XAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) }];
    public Axis[] YAxes { get; set; } = [new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)), MinLimit = 0 }];

    public Axis[] RaceXAxes 
    { 
        get => _raceXAxes;
        set => this.RaiseAndSetIfChanged(ref _raceXAxes, value);
    }
    public Axis[] NoAxes { get; set; } = [new Axis { IsVisible = false }];

    public Axis[] RaceYAxes
    {
        get => _raceYAxes;
        set => this.RaiseAndSetIfChanged(ref _raceYAxes, value);
    }

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

    private Replacement[] _replacements = [];
    public Replacement[] Replacements
    {
        get => _replacements;
        set => this.RaiseAndSetIfChanged(ref _replacements, value);
    }

    private ISeries[] _championshipRace = [];
    public ISeries[] ChampionshipRace
    {
        get => _championshipRace;
        set => this.RaiseAndSetIfChanged(ref _championshipRace, value);
    }

    private RectangularSection[] _championshopSections = [];
    public RectangularSection[] ChampionshipSections
    {
        get => _championshopSections;
        set => this.RaiseAndSetIfChanged(ref _championshopSections, value);
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

    public int _raceCount = 25;
    public int RaceCount
    {
        get => _raceCount;
        set => this.RaiseAndSetIfChanged(ref _raceCount, value);
    }

    private int _winCount = 0;
    public int WinCount
    {
        get => _winCount;
        set => this.RaiseAndSetIfChanged(ref _winCount, value);
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
        PropertyChanged += MainViewModel_PropertyChanged;
    }

    private void MainViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(RaceCount):
            case nameof(SelectedSeason):
                Load();
                break;

                

            default:
                return;
        }    
    }

    public async void AddSeason()
    {
        //Ask the user about what the new season should look like
        var window = new SeasonCreatorWindow();
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            if (await window.ShowDialog<bool>(desktopLifetime.MainWindow!) == false)
                return;

        //Create the new season
        var tracks = window.SelectedTracks;
        var races = tracks.Select(a => new RaceInfo(a, [], [])).ToArray();
        SaveManager.SaveRaceInfo(races, SaveManager.GetSeasons().Length + 1);

        //Reset seasons and Load
        Seasons = [];
        Load();
    }

    public void Load()
    {
        //Load seasons
        if (Seasons.Length == 0)
        {
            Seasons = SaveManager.GetSeasons();
            SelectedSeason = Seasons.LastOrDefault();
        }

        //If there are any seasons, load the selected season
        if (Seasons.Length > 0)
        {
            //Load info of selected season
            Races = SaveManager.LoadRaceInfo(SelectedSeason);
            if (Races.Length > 0)
            {
                (ChampionshipRace, WinCount, var maxScore) = GraphManager.GenerateChampionshipRace(Races, RaceCount);
                ConstructorsStandings = GraphManager.GenerateConstructorChampionship(Races);
                Penalties = GraphManager.GeneratePenaltyCount(Races);
                RaceXAxes = GraphManager.GenerateXAxes(Races, RaceCount);
                RaceYAxes = GraphManager.GenerateYAxes(maxScore, WinCount);
                ChampionshipSections = GraphManager.CreateYSection(WinCount);

            }
        }

        TeamData = SaveManager.LoadTeams();
        TrackData = SaveManager.LoadTracks();
        Replacements = SaveManager.LoadReplacements();
    }

    private int[] _seasons = [];
    public int[] Seasons
    {
        get => _seasons;
        set => this.RaiseAndSetIfChanged(ref _seasons, value);
    }

    private int _selectedSeason;
    public int SelectedSeason
    {
        get => _selectedSeason;
        set => this.RaiseAndSetIfChanged(ref _selectedSeason, value);
    }

    public void AddTrack()
    {
        TrackData = [.. TrackData, new()];
    }

    public void AddReplacement()
    {
        Replacements = [.. Replacements, new()];
    }

    public void Save()
    {
        SaveManager.SaveTracks(TrackData);
        SaveManager.SaveTeams(TeamData);
        SaveManager.SaveReplacements(Replacements);

        Load();
    }

    public async void Import()
    {
        //Open window
        var window = new ImporterWindow(Races, Replacements);
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            if (await window.ShowDialog<bool>(desktopLifetime.MainWindow!) == false)
                return;
        
        //Load race info
        var race = CsvParser.Open(window.ImportData, window.SelectedTrack);

        //Find where we're going to put it
        var index = Races.IndexOf(a => a.Track.Name == race.Track.Name && a.Results.Length == 0);
        if (index < 0 || race is null)
            return;

        //Place new race
        Races[index] = race;

        //Save then load
        SaveManager.SaveRaceInfo(Races, SelectedSeason);
        Load();
    }

}

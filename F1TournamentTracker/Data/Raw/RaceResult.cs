using ReactiveUI;
using System;

namespace F1TournamentTracker.Data.Raw
{
    public class RaceResult : ReactiveObject
    {
        private int _pos;
        private string _driver;
        private string _team;
        private int _grid;
        private int _stops;
        private TimeSpan _best;
        private string _time;
        private int _pts;
        private string _driverType;


        public int Pos
        {
            get => _pos;
            set => this.RaiseAndSetIfChanged(ref _pos, value);
        }

        public string Driver
        {
            get => _driver;
            set => this.RaiseAndSetIfChanged(ref _driver, value);
        }

        public string Team
        {
            get => _team;
            set => this.RaiseAndSetIfChanged(ref _team, value);
        }

        public int Grid
        {
            get => _grid;
            set => this.RaiseAndSetIfChanged(ref _grid, value);
        }

        public int Stops
        {
            get => _stops;
            set => this.RaiseAndSetIfChanged(ref _stops, value);
        }

        public TimeSpan Best
        {
            get => _best;
            set => this.RaiseAndSetIfChanged(ref _best, value);
        }

        public string Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public int Pts
        {
            get => _pts;
            set => this.RaiseAndSetIfChanged(ref _pts, value);
        }

        public string DriverType
        {
            get => _driverType;
            set => this.RaiseAndSetIfChanged(ref _driverType, value);
        }

        public RaceResult(int pos, string driver, string team, int grid, int stops, TimeSpan best, string time, int pts, string driverType)
        {
            _pos = pos;
            _driver = driver;
            _team = team;
            _grid = grid;
            _stops = stops;
            _best = best;
            _time = time;
            _pts = pts;
            _driverType = driverType;
        }
    }
}

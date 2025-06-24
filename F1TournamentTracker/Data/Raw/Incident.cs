using ReactiveUI;
using System;

namespace F1TournamentTracker.Data.Raw
{
    public class Incident : ReactiveObject
    {
        private TimeSpan _time;
        private int _lap;
        private string _driver;
        private string _team;
        private string _incidentType;
        private string _penalty;

        public TimeSpan Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public int Lap
        {
            get => _lap;
            set => this.RaiseAndSetIfChanged(ref _lap, value);
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

        public string IncidentType
        {
            get => _incidentType;
            set => this.RaiseAndSetIfChanged(ref _incidentType, value);
        }

        public string Penalty
        {
            get => _penalty;
            set => this.RaiseAndSetIfChanged(ref _penalty, value);
        }

        public Incident(TimeSpan time, int lap, string driver, string team, string incidentType, string penalty)
        {
            _time = time;
            _lap = lap;
            _driver = driver;
            _team = team;
            _incidentType = incidentType;
            _penalty = penalty;
        }
    }
}

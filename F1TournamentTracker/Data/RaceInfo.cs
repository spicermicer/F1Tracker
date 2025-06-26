using F1TournamentTracker.Data.Raw;
using ReactiveUI;

namespace F1TournamentTracker.Data
{
    public class RaceInfo : ReactiveObject
    {
        private TrackInfo _track;
        public TrackInfo Track
        {
            get => _track;
            set => this.RaiseAndSetIfChanged(ref _track, value);
        }

        private RaceResult[] _results;
        public RaceResult[] Results
        {
            get => _results;
            set => this.RaiseAndSetIfChanged(ref _results, value);
        }

        private Incident[] _incidents;
        public Incident[] Incidents
        {
            get => _incidents;
            set => this.RaiseAndSetIfChanged(ref _incidents, value);
        }        

        public RaceInfo(TrackInfo track, RaceResult[] results, Incident[] incidents)
        {
            _track = track;
            _results = results;            
            _incidents = incidents;
        }

        public bool CheckValidity()
        {
            foreach (var result in _results)
                if (result.Driver.Equals("Player", System.StringComparison.OrdinalIgnoreCase))
                    return false;

            foreach (var incident in _incidents)
                if (incident.Driver.Equals("Player", System.StringComparison.OrdinalIgnoreCase))
                    return false;

            return true;
        }
    }
}

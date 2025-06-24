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
    }
}

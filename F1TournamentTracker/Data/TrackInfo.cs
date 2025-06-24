using F1TournamentTracker.Data.Raw;
using ReactiveUI;
using System.Globalization;

namespace F1TournamentTracker.Data
{
    public class TrackInfo : ReactiveObject
    {
        private string _name;
        private string _abbreviations;
        private CultureInfo _country;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Abbreviations
        {
            get => _abbreviations;
            set => this.RaiseAndSetIfChanged(ref _abbreviations, value);
        }

        public CultureInfo Country
        {
            get => _country;
            set => this.RaiseAndSetIfChanged(ref _country, value);
        }

        public TrackInfo(string name, string abbreviations, string country)
        {
            _name = name;
            _abbreviations = abbreviations;
            _country = new CultureInfo(country);
        }
    }
}

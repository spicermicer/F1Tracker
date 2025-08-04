using F1TournamentTracker.Data.Raw;
using ReactiveUI;
using System.Globalization;
using System.Text.Json.Serialization;

namespace F1TournamentTracker.Data
{
    public class TrackInfo : ReactiveObject
    {
        private string _name;
        private string _abbreviations;
        private string _countryInfo;


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

        public string CountryInfo
        {
            get => _countryInfo;
            set => this.RaiseAndSetIfChanged(ref _countryInfo, value);
        }

        [JsonIgnore]
        public CultureInfo Country => new(CountryInfo);

        public TrackInfo(string name, string abbreviations, string country)
        {
            _name = name;
            _abbreviations = abbreviations;
            _countryInfo = country;
        }

        public TrackInfo() : this(string.Empty, string.Empty, string.Empty) { }


        public override string ToString()
        {
            return $"{Abbreviations} - {Name}";
        }

    }
}

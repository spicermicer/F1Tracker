using Avalonia.Media;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.ImageFilters;
using ReactiveUI;
using SkiaSharp;

namespace F1TournamentTracker.Data
{
    public class TeamInfo : ReactiveObject
    {
        private string _name = "";
        private byte _red;
        private byte _green;
        private byte _blue;

        public string Name 
        { 
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }


        public Color Color => new(255, Red, Green, Blue);
        public SolidColorPaint GraphColor => new(new SKColor(Red, Green, Blue)) { StrokeThickness = 4 };

        public byte Red
        {
            get => _red;
            set => this.RaiseAndSetIfChanged(ref _red, value);
        }

        public byte Green
        {
            get => _green;
            set => this.RaiseAndSetIfChanged(ref _green, value);
        }

        public byte Blue
        {
            get => _blue;
            set => this.RaiseAndSetIfChanged(ref _blue, value);
        }        



        public TeamInfo(string name, byte red, byte green, byte blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;

            PropertyChanged += (s, e) =>
            {
                this.RaisePropertyChanged(nameof(Color));
            };
        }
    }
}

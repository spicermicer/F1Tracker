using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1TournamentTracker.Graph.Objects
{
    internal class RacePoint : ObservableValue
    {
        public SolidColorPaint TeamColor { get; set; }

        public string Driver { get; set; }

        public RacePoint(string driver, SolidColorPaint teamColor, double value)
        {
            Driver = driver;
            TeamColor = teamColor;
            Value = value;
        }

    }
}

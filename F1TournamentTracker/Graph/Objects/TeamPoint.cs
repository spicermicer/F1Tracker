using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1TournamentTracker.Graph.Objects
{
    internal class TeamPoint : ObservableValue
    {
        public string TeamName { get; set; }

        public TeamPoint(string name, int points)
        {
            TeamName = name;
            Value = points;
        }

    }
}

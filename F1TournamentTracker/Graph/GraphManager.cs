using F1TournamentTracker.Data;
using F1TournamentTracker.Graph.Objects;
using F1TournamentTracker.Managers;
using F1TournamentTracker.Views;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1TournamentTracker.Graph
{
    internal static class GraphManager
    {
        public static ISeries[] GenerateChampionshipRace(RaceInfo[] races)
        {
            var teams = SaveManager.LoadTeams().ToDictionary(a => a.Name);

            var scoreDict = new Dictionary<string, List<RacePoint>>();            
            foreach (var race in races) {
                foreach (var result in race.Results)
                {
                    if (!scoreDict.TryAdd(result.Driver, new List<RacePoint>([new RacePoint(result.Driver, teams[result.Team].GraphColor, result.Pts)])))
                    {
                        double last = (double)scoreDict[result.Driver].Last().Value!;                        
                        scoreDict[result.Driver].Add(new RacePoint(result.Driver, teams[result.Team].GraphColor, last + result.Pts));
                    }
                }
            }

            var series = scoreDict.Select(a =>
            {
                return new LineSeries<RacePoint>()
                {
                    Name = a.Key,
                    Values = [.. a.Value],
                    DataLabelsPosition = DataLabelsPosition.End,
                    Stroke = a.Value.First().TeamColor,         
                    GeometrySize = 0,
                    GeometryStroke = null,
                    Fill = null,
                };
            }).OrderByDescending(a => a.Values.Last().Value);

            return [.. series];
        }

        public static ISeries[] GenerateConstructorChampionship(RaceInfo[] races)
        {
            var teams = SaveManager.LoadTeams().ToDictionary(a => a.Name);
            var teamDict = new Dictionary<string, int>();
            foreach (var race in races)
                foreach (var result in race.Results)
                    if (!teamDict.TryAdd(result.Team, result.Pts))
                        teamDict[result.Team] += result.Pts;

            var series = new RowSeries<TeamPoint>()
            {
                Values = teamDict.OrderBy(a => a.Value).Select(a => new TeamPoint(a.Key, a.Value)).ToArray(),
                DataLabelsPaint = new SolidColorPaint(new SKColor(245, 245, 245)),
                DataLabelsPosition = DataLabelsPosition.End,
                DataLabelsTranslate = new(-1, 0),
                DataLabelsFormatter = point => $"{point.Model!.TeamName} {point.Coordinate.PrimaryValue}",
                MaxBarWidth = 50,
                Padding = 10,
            };

            EventExtensions.OnPointMeasured(series, point =>
            {
                if (point.Visual is null) return;
                point.Visual.Fill = teams[point.Model!.TeamName].GraphColor;
            });


            return [series];
        }

    }
}

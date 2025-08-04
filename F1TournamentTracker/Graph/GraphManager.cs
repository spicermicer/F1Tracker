using F1TournamentTracker.Data;
using F1TournamentTracker.Graph.Objects;
using F1TournamentTracker.Managers;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F1TournamentTracker.Graph
{
    internal static class GraphManager
    {        
        public static (ISeries[], int, int) GenerateChampionshipRace(RaceInfo[] races, int numberOfRaces)
        {
            var teams = SaveManager.LoadTeams().ToDictionary(a => a.Name);

            //Generate driver shapes
            bool anyRaces = false;
            var teamCount = new Dictionary<string, int>();
            var driverGeometry = new Dictionary<string, string>();
            foreach (var race in races) {
                foreach (var result in race.Results)
                {
                    anyRaces = true;

                    if (!driverGeometry.ContainsKey(result.Driver))
                    {
                        if (!teamCount.TryGetValue(result.Team, out int index))
                            index = 0;

                        driverGeometry[result.Driver] = GetGeometry(index);
                        teamCount[result.Team] = index + 1;

                    }
                }
            }

            //Generate score data
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


            //Add everyone starting at 0
            foreach(var score in scoreDict)            
                score.Value.Insert(0, new RacePoint(score.Key, score.Value.First().TeamColor, 0));


            //Ordered Score Array
            var scoreArray = scoreDict.OrderByDescending(a => a.Value!.Last().Value).ToArray();

            //Calculate number of races left
            var leadDriverPoints = anyRaces ? scoreArray[0].Value.Last().Value : 0;
            var secondDriverPoints = anyRaces ? scoreArray[1].Value.Last().Value : 0;
            var maximumRemainingPoints = anyRaces ? 26 * (numberOfRaces - races.Length) : 0;
            var pointsNeeded = anyRaces ? secondDriverPoints + maximumRemainingPoints + 1 : 0;

            //Convert to series
            var series = scoreArray.Select(a =>
            {
                return new LineSeries<RacePoint, VariableSVGPathGeometry>()
                {
                    Name = a.Key,
                    Values = [.. a.Value],
                    DataLabelsPosition = DataLabelsPosition.End,
                    Stroke = a.Value.First().TeamColor,
                    GeometryStroke = a.Value.First().TeamColor,
                    GeometrySvg = driverGeometry[a.Key],
                    GeometrySize = 10,
                    LineSmoothness = 0,
                    Fill = null,
                };
            });

            return ([.. series], (int)pointsNeeded, (int)leadDriverPoints);
        }

        public static RectangularSection[] CreateYSection(int sectionValue)
        {
            return [
               new RectangularSection
                {
                    Yi = sectionValue,
                    Yj = sectionValue,
                    Stroke = new SolidColorPaint
                    {
                        Color = SKColors.Red,
                        StrokeThickness = 3,
                        PathEffect = new DashEffect([6, 6])
                    }
                }
           ];
        }

        private static string GetGeometry(int count)
        {
            return count % 2 == 0 ? SVGPoints.Square : SVGPoints.Circle;
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
                Values = [.. teamDict.OrderBy(a => a.Value).Select(a => new TeamPoint(a.Key, a.Value))],
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

        public static string[] GeneratePenaltyCount(RaceInfo[] races)
        {
            Dictionary<string, Dictionary<string, int>> dict = [];



            foreach (var race in races)
                foreach (var incident in race.Incidents)
                {
                    dict.TryAdd(incident.Driver, []);
                    dict[incident.Driver].TryAdd(incident.IncidentType, 0);
                    dict[incident.Driver][incident.IncidentType]++;
                }

            return [.. dict
                .OrderByDescending(a => a.Value
                    .Sum(b => b.Value))
                .Select(a => $"{a.Key} : \n\t{string.Join("\n\t", a.Value
                    .OrderByDescending(a => a.Value)
                    .Select(a => $"{a.Key} : {a.Value}"))}")];
        }

        public static Axis[] GenerateXAxes(RaceInfo[] races, int raceCount)
        {
            var axis = new Axis()
            {
                LabelsPaint = new SolidColorPaint(SKColors.White),
                Labels = [ "", .. races.Select(a => a.Track.Abbreviations)],
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) {  StrokeThickness = 1 },
                MaxLimit = raceCount
            };
            return [axis];
        }

        public static Axis[] GenerateYAxes(int maxScore, int winValue)
        {
            var axis = new Axis()
            {
                LabelsPaint = new SolidColorPaint(SKColors.White),
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                MaxLimit = Math.Max(maxScore, winValue) + 10
            };
            return [axis];
        }
    }
}

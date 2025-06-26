using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DynamicData;
using F1TournamentTracker.Converters;
using F1TournamentTracker.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace F1TournamentTracker.Controls
{
    internal class RaceControls : Control
    {

        private readonly CountryFlagConverter _converter = new();

        /// <summary>
        /// RaceResults StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<RaceInfo[]> ItemsSourceProperty =
            AvaloniaProperty.Register<RaceControls, RaceInfo[]>(nameof(ItemsSource));

        /// <summary>
        /// Gets or sets the RaceResults property. This StyledProperty
        /// indicates ....
        /// </summary>
        public RaceInfo[] ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        static RaceControls()
        {
            AffectsRender<RaceControls>(ItemsSourceProperty);
        }


        private static FormattedText CreateText(string txt) => CreateText(txt, 16, Brushes.White);

        private static FormattedText CreateText(string txt, IBrush brush) => CreateText(txt, 16, brush);


        private static FormattedText CreateText(string txt, int size, IBrush brush)
        {
            return new FormattedText(txt, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface.Default, size, brush);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (ItemsSource == null || ItemsSource.Length == 0)
                return;

            var racerNames = ItemsSource.SelectMany(a => a.Results.Select(b => b.Driver)).Distinct().ToArray();


            var scoreDict = new Dictionary<string, int>();
            foreach (var item in ItemsSource)            
                foreach (var race in item.Results)                
                    if (!scoreDict.TryAdd(race.Driver, race.Pts))
                        scoreDict[race.Driver] += race.Pts;

            racerNames = [.. racerNames.OrderByDescending(a => scoreDict[a])];

            

            var racerFormattedText = racerNames.Select(a => CreateText(a)).ToArray();

            var races = ItemsSource.Select(a => a.Track).ToArray();

            var topBarHeight = 40;
            var totalsWidth = 60;

            var nameSize = new Size(
                racerFormattedText.Max(a => a.Width),
                (Bounds.Height - topBarHeight) / (racerFormattedText.Length));


            var cellArea = new Rect(nameSize.Width + 5, topBarHeight, Bounds.Width - nameSize.Width - totalsWidth, Bounds.Height - topBarHeight);
            var cellSize = new Size(
                cellArea.Width / races.Length,
                nameSize.Height);

            //Draw driver names + totals
            for (int i = 0; i < racerFormattedText.Length; i++)
            {
                var y = topBarHeight + (i * nameSize.Height);


                context.DrawText(racerFormattedText[i], 
                    new Point(nameSize.Width - racerFormattedText[i].Width, y));


                var totalText = CreateText(scoreDict[racerNames[i]].ToString());
                context.DrawText(totalText, new Point(Bounds.Width - (totalsWidth / 2) - (totalText.Width / 2), y));
            }

            //Draw Race labels            
            for (int i = 0; i < races.Length; i++)
            {
                var x = cellArea.X + (cellSize.Width * i);

                var race = ItemsSource[i];

                var image = _converter.Convert(race.Track.Country, typeof(bool), null, CultureInfo.CurrentCulture) as Bitmap;
                context.DrawImage(image!, new Rect(
                    x + (cellSize.Width / 2) - 12,
                    0, 24, 16));


                var titleText = CreateText(race.Track.Abbreviations);
                context.DrawText(titleText,  new Point(x + (cellSize.Width / 2) - (titleText.Width / 2), topBarHeight - titleText.Height));


                var bestDriver = race.Results.MinBy(a => a.Best)!.Driver;
                foreach (var cell in race.Results)
                {
                    var index = racerNames.IndexOf(cell.Driver);

                    var cellRect = new Rect(
                        x,
                        topBarHeight + (cellSize.Height * index),
                        cellSize.Width,
                        cellSize.Height);

                    bool isDnf = cell.Time == "DNF";
                    bool isPole = cell.Grid == 1;
                    bool isFastestQualifying = cell.Driver == bestDriver;

                    var cellString = isDnf ? "DNF" : cell.Pos.ToString();

                    if (isPole)
                        cellString += "ᵖ";
                    if (isFastestQualifying)
                        cellString += "ᶠ";



                    Color background = new(255, 75, 75, 75);
                    if (cell.Pos == 1)
                        background = Colors.Gold;
                    else if (cell.Pos == 2)
                        background = Colors.Silver;
                    else if (cell.Pos == 3)
                        background = Colors.Brown;
                    else if (cell.Pos <= 10)
                        background = Colors.Green;
                    else if (isDnf)
                        background = Colors.Black;

                    Color foreground = background.R + background.G + background.B > 380 ? Colors.Black : Colors.White;                    
                    var cellText = CreateText(cellString, new SolidColorBrush(foreground));


                    context.DrawRectangle(new SolidColorBrush(background), null, cellRect);
                    context.DrawText(cellText, new Point(
                        cellRect.Center.X - (cellText.Width / 2),
                        cellRect.Center.Y - (cellText.Height / 2)));
                    
                }
            }
            
        }

    }
}

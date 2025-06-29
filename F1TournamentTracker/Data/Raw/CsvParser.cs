using F1TournamentTracker.Managers;
using System;
using System.IO;
using System.Linq;

namespace F1TournamentTracker.Data.Raw
{
    internal class CsvParser
    {
        public static TimeSpan ParseTime(string input)
        {
            if (input == "DNF")
                return TimeSpan.Zero;

            input = input.Replace("+", "");
            string[] parts = input.Split([':', '.']).Reverse().ToArray();
            double milliseconds = parts.Length > 0 ? double.Parse(parts[0]) : 0;
            double seconds = parts.Length > 1 ? double.Parse(parts[1]) : 0;
            int minutes = parts.Length > 2 ? int.Parse(parts[2]) : 0;


            return TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds) + TimeSpan.FromMilliseconds(milliseconds);

        }

        public static RaceInfo? Open(string path)
        {
            var tracks = SaveManager.LoadTracks();
            var track = tracks.FirstOrDefault(a => a.Name == Path.GetFileNameWithoutExtension(path).Split("-")[0]);
            if (track == null)
                return null;

            var data = File.ReadAllText(path);

            return Open(data, track);
        }

        public static RaceInfo? Open(string data, TrackInfo track)
        {
            var datas = data.Split("\n\n").Select(a => a.Trim()).ToArray();

            var results = datas[0].Split("\n").Skip(1).Select(a => a.Split(",").Select(b => b.Replace("\"", "").Trim()).ToArray()).ToArray();
            var incidents = datas[1].Split("\n").Skip(1).Select(a => a.Split(",").Select(b => b.Replace("\"", "").Trim()).ToArray()).ToArray();

            var raceArray = results!.Select(a => new RaceResult(
                int.Parse(a[0]), 
                a[1], 
                a[2], 
                int.Parse(a[3]),
                int.Parse(a[4]),
                ParseTime(a[5]),
                a[6], 
                int.Parse(a[7]), 
                a[8]))
                .ToArray();

            var incidentArray = incidents!.Select(a => new Incident(
                ParseTime(a[0]),
                int.Parse(a[1]),
                a[2],
                a[3],
                a[4],
                a[5]))
                .ToArray();


            return new RaceInfo(track, raceArray, incidentArray);
        }

    }
}

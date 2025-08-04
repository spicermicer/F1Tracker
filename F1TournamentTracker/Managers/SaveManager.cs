using Avalonia.Controls;
using F1TournamentTracker.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace F1TournamentTracker.Managers
{
    static class SaveManager
    {
        static private readonly string _trackPath = "Settings/Tracks.json";
        static private readonly string _teamsPath = "Settings/Teams.json";
        static private readonly string _replacementsPath = "Settings/Replacements.json";

        private static string GetPath(int season)
        {
            return $"Data/Season-{season}.json"; ;
        }

        private static JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };


        public static void SaveTracks(TrackInfo[] tracks)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_trackPath)!);

            var json = JsonSerializer.Serialize(tracks);
            File.WriteAllText(_trackPath, json);
        }

        public static void SaveTeams(TeamInfo[] teams)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_teamsPath)!);

            var json = JsonSerializer.Serialize(teams);
            File.WriteAllText(_teamsPath, json);
        }

        public static void SaveReplacements(Replacement[] replacements)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_replacementsPath)!);

            var json = JsonSerializer.Serialize(replacements);
            File.WriteAllText(_replacementsPath, json);
        }


        public static TrackInfo[] LoadTracks()
        {
            //If none exist, load default
            if (!Path.Exists(_trackPath))
                return [
                    new TrackInfo("Bahrain International Circuit", "BHR", "BH"),
                    new TrackInfo("Jeddah Corniche Circuit", "SAU", "SA"),
                    new TrackInfo("Albert Park Circuit", "AUS", "AU"),
                    new TrackInfo("Suzuka International Racing Course", "JPN", "JP"),
                    new TrackInfo("Shanghai International Circuit", "CHN", "CN"),
                    new TrackInfo("Miami International Autodrome", "MIA", "US"),
                    new TrackInfo("Autodromo Enzo e Dino Ferrari", "EMI", "IT"),
                    new TrackInfo("Circuit de Monaco", "MON", "MC"),
                    new TrackInfo("Circuit Gilles Villeneuve", "CAN", "CA"),
                    new TrackInfo("Circuit de Barcelona‑Catalunya", "ESP", "ES"),
                    new TrackInfo("Red Bull Ring", "AUT", "AT"),
                    new TrackInfo("Silverstone Circuit", "GBR", "GB"),
                    new TrackInfo("Hungaroring", "HUN", "HU"),
                    new TrackInfo("Circuit de Spa‑Francorchamps", "BEL", "BE"),
                    new TrackInfo("Circuit Zandvoort", "NED", "NL"),
                    new TrackInfo("Autodromo Nazionale di Monza", "ITA", "IT"),
                    new TrackInfo("Baku City Circuit", "AZE", "AZ"),
                    new TrackInfo("Marina Bay Street Circuit", "SIN", "SG"),
                    new TrackInfo("Circuit of the Americas", "USA", "US"),
                    new TrackInfo("Autódromo Hermanos Rodríguez", "MXC", "MX"),
                    new TrackInfo("Autódromo José Carlos Pace", "SAP", "BR"),
                    new TrackInfo("Las Vegas Strip Circuit", "LVG", "US"),
                    new TrackInfo("Lusail International Circuit", "QAT", "QA"),
                    new TrackInfo("Yas Marina Circuit", "ABU", "AE"),
                    ];

            //Otherwise, load json
            var json = File.ReadAllText(_trackPath);
            return JsonSerializer.Deserialize<TrackInfo[]>(json)!;
        }

        public static TeamInfo[] LoadTeams()
        {
            //If none exist, load default
            if (!Path.Exists(_teamsPath))
                return [
                    new TeamInfo("McLaren",244, 118, 0),
                    new TeamInfo("Mercedes-AMG Petronas", 0, 215, 182),
                    new TeamInfo("Scuderia Ferrari HP", 237, 17, 49),
                    new TeamInfo("Red Bull", 71, 129, 215),
                    new TeamInfo("Williams", 24, 104, 219),
                    new TeamInfo("Haas", 156, 159, 162),
                    new TeamInfo("Visa Cash App RB", 108, 152, 255),
                    new TeamInfo("Aston Martin", 34, 153, 113),
                    new TeamInfo("KICK Sauber", 1, 192, 14),
                    new TeamInfo("Alpine", 0, 161, 232),
                    ];

            //Otherwise, load json
            var json = File.ReadAllText(_teamsPath);
            return JsonSerializer.Deserialize<TeamInfo[]>(json)!;
        }

        public static Replacement[] LoadReplacements()
        {
            if (!Path.Exists(_replacementsPath))
                return [];

            var json = File.ReadAllText(_replacementsPath);
            return JsonSerializer.Deserialize<Replacement[]>(json)!;
        }
        
        public static void SaveRaceInfo(RaceInfo[] data, int season)
        {
            var path = GetPath(season);
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(path, json);
        }

        public static RaceInfo[] LoadRaceInfo(int season)
        {
            var path = GetPath(season);
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<RaceInfo[]>(json)!;
        }

        public static int[] GetSeasons()
        {
            var ret = new List<int>();
            for (int i = 1; true; i++)
            {
                var path = GetPath(i);
                if (!File.Exists(path))
                    break;

                ret.Add(i);
            }

            return [.. ret];
        }

    }
}

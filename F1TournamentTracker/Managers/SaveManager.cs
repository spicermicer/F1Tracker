using Avalonia.Controls;
using F1TournamentTracker.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace F1TournamentTracker.Managers
{
    static class SaveManager
    {
        static private readonly string _trackPath = "Settings/Tracks.json";
        static private readonly string _teamsPath = "Settings/Teams.json";
        static private readonly string _orderPath = "Data/Order.json";


        public static void Save(TrackInfo[] tracks)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_trackPath)!);

            var json = JsonSerializer.Serialize(tracks);
            File.WriteAllText(_trackPath, json);
        }

        public static void Save(TeamInfo[] teams)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_teamsPath)!);

            var json = JsonSerializer.Serialize(teams);
            File.WriteAllText(_teamsPath, json);
        }

        public static void Save(string[] order)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_teamsPath)!);

            var json = JsonSerializer.Serialize(order);
            File.WriteAllText(_orderPath, json);

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

        public static string[] LoadOrder()
        {
            if (!Path.Exists(_orderPath))
                return Directory.EnumerateFiles("Data/Races", "*.csv").Select(a => Path.GetFileName(a)).ToArray();

            var json =  File.ReadAllText(_orderPath);
            return JsonSerializer.Deserialize<string[]>(json)!;


        }

    }
}

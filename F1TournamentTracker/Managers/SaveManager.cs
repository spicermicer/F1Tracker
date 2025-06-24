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
        static private string _trackPath = "tracks.json";
        static private string _teamsPath = "teams.json";


        public static void Save(TrackInfo[] tracks)
        {
            var json = JsonSerializer.Serialize(tracks);
            File.WriteAllText(_trackPath, json);
        }


        public static TrackInfo[] LoadTracks()
        {
            //If none exist, load default
            if (!Path.Exists(_trackPath))
                return [
                    new TrackInfo("Bahrain International Circuit", "BIC", "BH"),
                    new TrackInfo("Jeddah Corniche Circuit", "JCC", "SA"),
                    new TrackInfo("Albert Park Circuit", "APC", "AU"),
                    new TrackInfo("Suzuka International Racing Course", "SIRC", "JP"),
                    new TrackInfo("Shanghai International Circuit", "SIC", "CN"),
                    new TrackInfo("Miami International Autodrome", "MIA", "US"),
                    new TrackInfo("Autodromo Enzo e Dino Ferrari", "Imola", "IT"),
                    new TrackInfo("Circuit de Monaco", "MON", "MC"),
                    new TrackInfo("Circuit Gilles Villeneuve", "CGV", "CA"),
                    new TrackInfo("Circuit de Barcelona‑Catalunya", "CAT", "ES"),
                    new TrackInfo("Red Bull Ring", "RBR", "AT"),
                    new TrackInfo("Silverstone Circuit", "SIL", "GB"),
                    new TrackInfo("Hungaroring", "HUN", "HU"),
                    new TrackInfo("Circuit de Spa‑Francorchamps", "SPA", "BE"),
                    new TrackInfo("Circuit Zandvoort", "ZAN", "NL"),
                    new TrackInfo("Autodromo Nazionale di Monza", "MON", "IT"),
                    new TrackInfo("Baku City Circuit", "BAK", "AZ"),
                    new TrackInfo("Marina Bay Street Circuit", "SGP", "SG"),
                    new TrackInfo("Circuit of the Americas", "COTA", "US"),
                    new TrackInfo("Autódromo Hermanos Rodríguez", "MEX", "MX"),
                    new TrackInfo("Autódromo José Carlos Pace", "INT", "BR"),
                    new TrackInfo("Las Vegas Strip Circuit", "LV", "US"),
                    new TrackInfo("Lusail International Circuit", "QAT", "QA"),
                    new TrackInfo("Yas Marina Circuit", "YMC", "AE"),
                    ];

            //Otherwise, load json
            var json = File.ReadAllText(_trackPath);
            return JsonSerializer.Deserialize<TrackInfo[]>(json);
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
            return JsonSerializer.Deserialize<TeamInfo[]>(json);
        }

    }
}

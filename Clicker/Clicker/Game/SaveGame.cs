using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Clicker.Game {
    /// <summary>
    /// This class contains all the data stored in a gamesave, and can be used
    /// to load and save savegames.
    /// </summary>
    public class SaveGame {
        public class Bonus {
            public string Name { get; set; }
            public uint InitialCost { get; set; }
            public uint CookiesPerPeriod { get; set; }
            public float Period { get; set; }
            public ulong Count { get; set; }
            public float Time { get; set; }
        };

        public ulong Score { get; set; }
        public List<Bonus> Bonuses { get; set; }

        public SaveGame(){
            Score = 0;
            Bonuses = new List<Bonus>();
        }

        public SaveGame(GameState state){
            Score = state.Score;
            Bonuses = new List<Bonus>();

            foreach(Clicker.Game.Bonus curr in state.Bonuses){
                Bonus b = new Bonus();
                b.Name = curr.Name;
                b.InitialCost = curr.InitialCost;
                b.CookiesPerPeriod = curr.CookiesPerPeriod;
                b.Period = curr.Period;
                b.Count = curr.Count;
                b.Time = curr.Time;
                Bonuses.Add(b);
            }
        }

        public GameState ToState(){
            GameState result = new GameState();
            result.Score = Score;
            result.Bonuses.Clear();

            foreach(Bonus curr in Bonuses){
                result.Bonuses.Add(new Game.Bonus(
                    name: curr.Name,
                    initialCost: curr.InitialCost,
                    perPeriod: curr.CookiesPerPeriod,
                    period: curr.Period,
                    count: curr.Count,
                    time: curr.Time
                ));
            }

            return result;
        }

        public void Write(){
            string path = GetSavePath();
            string json = JsonConvert.SerializeObject(this);
            System.IO.File.WriteAllText(path, json);
        }

        public static SaveGame Read(){
            try {
                string path = GetSavePath();
                string json = System.IO.File.ReadAllText(path);

                return JsonConvert.DeserializeObject<SaveGame>(json);
            } catch( Exception ){
                return null;
            }
        }

        /// <summary>
        /// Gets the path to the user's home directory. 
        /// </summary>
        /// <returns>The home directory.</returns>
        private static string GetHomeDirectory() {
            // First try using the standard API
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if( path != null && path.Length > 0 )
                return path;

            // If that didn't work, fall back to platform-specific implementations.
            PlatformID platform = Environment.OSVersion.Platform;

            if( platform == PlatformID.Unix || platform == PlatformID.MacOSX ) {
                // Unix and Mac (which is also Posix).
                return Environment.GetEnvironmentVariable("HOME");
            } else {
                // Windows and everything else.
                return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
        }

        /// <summary>
        /// Gets the path to the save file.
        /// </summary>
        /// <returns>The save path.</returns>
        private static string GetSavePath() {
            return SaveGame.GetHomeDirectory() + "/Clicker.save";
        }
    }
}

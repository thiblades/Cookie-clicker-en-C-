using Clicker.Engine.Public;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clicker.Game
{
    /// <summary>
    /// Manage the score
    /// Can be [un]serialized
    /// </summary>
    public class Score : AJsonSerializable
    {

        public decimal PlayerScore { get; set; }

        public const string Path = "Assets/Save.json";

        public Score()
        {
            PlayerScore = 0;
        }

        public decimal Add(decimal value)
        {
            return PlayerScore += value;
        }

        public override string ToString()
        {
            return "Score : " + PlayerScore;
        }


        /// <summary>
        ///  Read the values from the save file
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonSerializationException"></exception>
        /// <exception cref="JsonException"></exception>
        public static Score Read()
        {
            JsonSerializerSettings jsonSerializerSettings = InitJsonSerializerSettings();

            JsonSerializer serializer = JsonSerializer.Create(jsonSerializerSettings);

            StreamReader stream;
            JsonTextReader jsonText;
            Score result;
            using (stream = new StreamReader(Path))
            using (jsonText = new JsonTextReader(stream))
            {
                // Now read the score.
                result = serializer.Deserialize<Score>(jsonText);
            }

            return result;
        }

        /// <summary>
        ///  Write values to the score file
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="JsonSerializationException"></exception>
        /// <exception cref="JsonException"></exception>
        public static void Write(Score score)
        {
            JsonSerializerSettings jsonSerializerSettings = InitJsonSerializerSettings();

            JsonSerializer serializer = JsonSerializer.Create(jsonSerializerSettings);

            StreamWriter stream;
            JsonTextWriter jsonText;
            using (stream = new StreamWriter(Path))
            using (jsonText = new JsonTextWriter(stream))
            {
                // Now write the score.
                serializer.Serialize(jsonText, score);
            }

        }

    }
}

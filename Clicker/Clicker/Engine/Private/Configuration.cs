using System;

using System.IO;
using Newtonsoft.Json;

namespace Clicker.Engine.Private {
    public class Configuration {
        public const string Path = "Assets/Config.json";

        public class ResolutionConfig {
            public uint Width { get; set;  }
            public uint Height { get; set;  }
            public bool FullScreen { get; set; }
        }

        // The name of the class to load as game
        public string GameClass { get; set; }

        // Whether or not to synchronize rendering to the target monitor's
        // VBLANK signal.
        public bool VSyncEnabled { get; set; }

        // Framerate at which to perform updates.
        public uint Framerate { get; set; }

        public ResolutionConfig Resolution { get; set; }

        /// <summary>
        ///  Read the values from the configuration file
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonSerializationException"></exception>
        /// <exception cref="JsonException"></exception>
        public static Configuration Read()
        {
            JsonSerializerSettings jsonSerializerSettings = InitJsonSerializerSettings();

            JsonSerializer serializer = JsonSerializer.Create(jsonSerializerSettings);

            StreamReader stream;
            JsonTextReader jsonText;
            Configuration result;
            using (stream = new StreamReader(Path))
            using (jsonText = new JsonTextReader(stream))
            {
                // Now read the configuration.
                result = serializer.Deserialize<Configuration>(jsonText);
            }

            return result;
        }

        /// <summary>
        ///  Write values to the configuration file
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
        public static void Write(Configuration configuration)
        {
            JsonSerializerSettings jsonSerializerSettings = InitJsonSerializerSettings();

            JsonSerializer serializer = JsonSerializer.Create(jsonSerializerSettings);

            StreamWriter stream;
            JsonTextWriter jsonText;
            using (stream = new StreamWriter(Path))
            using (jsonText = new JsonTextWriter(stream))
            {
                // Now write the configuration.
                serializer.Serialize(jsonText, configuration);
            }

        }

        /// <summary>
        ///  Initialize <see cref="JsonSerializerSettings"/> properly
        /// </summary>
        private static JsonSerializerSettings InitJsonSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            // If a member is not present during deserialization, throw an Exception
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
            // Always serialize and deserialize null values
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
            // Add the default value during serialization
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
            // Add indentation for readability
            jsonSerializerSettings.Formatting = Formatting.Indented;

            return jsonSerializerSettings;
        }
    }
}

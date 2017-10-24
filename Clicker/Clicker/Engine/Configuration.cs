using System;

using Microsoft.Extensions.Configuration;

using System.IO;

namespace Clicker.Engine {
    public class Configuration {
        public const string Path = "Assets/Config.json";

        public class ResolutionConfig {
            public uint Width { get; set;  }
            public uint Height { get; set;  }
            public bool FullScreen { get; set; }
        }

        // Name of the game, displayed in the window.
        public string GameName { get; set; }

        // Whether or not to synchronize rendering to the target monitor's
        // VBLANK signal.
        public bool VSyncEnabled { get; set; }

        // Framerate at which to perform updates.
        public uint Framerate { get; set; }

        public ResolutionConfig Resolution { get; set; }

        // Read the values from the configuration file
        public static Configuration Read(){
            // Open the configuration file.
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path, optional:false, reloadOnChange:true);
            
            IConfigurationRoot config = configBuilder.Build();

            // Now read the configuration.
            Configuration result = new Configuration();
            config.Bind(result);
            return result;
        }
    }
}

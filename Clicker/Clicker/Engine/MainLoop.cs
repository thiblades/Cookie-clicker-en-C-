using System;

using SFML.Window;
using SFML.Graphics;
using SFML.System;


namespace Clicker.Engine {
    public class MainLoop {
        // The window in which the game is being rendered.
        private RenderWindow window;

        // The configuration currently in use.
        private Configuration configuration;

        // The scene that's currently running.
        private Scene currentScene = null;
        private LoadingScene loadingScene = null;

        // Main game loop.
        private void Run(){
            // Read the configuration
            configuration = Configuration.Read();

            // Prepare the loading scene.
            loadingScene = new LoadingScene();

            // Create the game window and set it up according to the configuration
            VideoMode vm = new VideoMode(configuration.Resolution.Width, configuration.Resolution.Height);
            window = new RenderWindow(vm, configuration.GameName);
            window.SetVerticalSyncEnabled(configuration.VSyncEnabled);
            window.SetFramerateLimit(configuration.Framerate);

            // When the window gets closed, close the game.
            window.Closed += (object sender, EventArgs e) => {
                window.Close();
            };

            window.Resized += (object sender, SizeEventArgs e) => {
                View viewport = new View();
                viewport.Center = new Vector2f(e.Width / 2, e.Height / 2);
                viewport.Size = new Vector2f(e.Width, e.Height);
                window.SetView(viewport);
            };

            // Main game loop.
            while( window.IsOpen ){
                window.DispatchEvents();

                if( currentScene != null ){
                    currentScene.Update(window);
                    currentScene.Display(window);
                } else {
                    loadingScene.Update(window);
                    loadingScene.Display(window);
                }

                window.Display();
            }
        }

        /*
         * Entry point.
         * 
         * This method is run when the engine starts. It performs initialization
         * and runs the main loop that keeps the game running.
         */
        static void Main(string[] args){
            MainLoop loop = new MainLoop();
            loop.Run();
        }
    }
}

using System;
using System.Threading;
using System.Reflection;

using SFML.System;
using SFML.Window;

using Clicker.Engine.Public;

namespace Clicker.Engine.Private {
    class DummyProgressReport : IProgressReport {
        void IProgressReport.ReportProgress(float progress, string msg){
            
        }
    }

    public class CInstance : IInstance, IProgressReport {
        public enum State {
            Initializing,
            LoadingGame,
            LoadingInitialScene,
            LoadingScene,
            Running,
        };

        private State           state           = State.Initializing;
        private Configuration   configuration   = null;
        private GameWindow      gameWindow      = null;
        private Public.Game     game            = null;
        private LoadingScene    loadingScene    = null;
        //private Thread          workerThread    = null;

        private Public.Game CreateGameInstance(){
            Type gameClass = Type.GetType(configuration.GameClass);

            if( !gameClass.IsClass ) {
                Console.WriteLine("Error: " + configuration.GameClass + " is not a class");
                Environment.Exit(1);
            }

            if( gameClass == null ) {
                Console.WriteLine("Error: Could not find class " + configuration.GameClass);
                Environment.Exit(1);
            }

            if( !typeof(Public.Game).IsAssignableFrom(gameClass) ) {
                Console.WriteLine("Error: " + gameClass.FullName + " does not implement Game");
                Environment.Exit(1);
            }

            return (Public.Game) Activator.CreateInstance(gameClass);
        }

        private void InitializeEvents() {
            gameWindow.Window.KeyPressed += (object sender, KeyEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnKeyDown(e);
            };

            gameWindow.Window.KeyReleased += (object sender, KeyEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnKeyUp(e);
            };

            gameWindow.Window.MouseMoved += (object sender, MouseMoveEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseMove(e);
            };

            gameWindow.Window.MouseButtonPressed += (object sender, MouseButtonEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseDown(e);
            };

            gameWindow.Window.MouseButtonReleased += (object sender, MouseButtonEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseUp(e);
            };

            gameWindow.Window.MouseWheelMoved += (object sender, MouseWheelEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseWheel(e);
            };

            gameWindow.Window.MouseEntered += (object sender, EventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseEnter();
            };

            gameWindow.Window.MouseLeft += (object sender, EventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnMouseLeave();
            };

            gameWindow.Window.TextEntered += (object sender, TextEventArgs e) => {
                if( gameWindow.HasScene() )
                    gameWindow.Scene.OnTextEntered(e);
            };
        }

        private void Initialize(){
            // Read the configuration, and use the default configuration if
            // we can't read the config file.
            configuration = Configuration.Read();

            // Create the game object.
            game = CreateGameInstance();
            game.Instance = this;

            // Create the game window
            gameWindow = new GameWindow(configuration);
            gameWindow.Open(game.Name);
            InitializeEvents();

            // Load the loading scene, and display it ASAP.
            loadingScene = new LoadingScene(game);
            loadingScene.Instance = this;
            loadingScene.Load(new DummyProgressReport());
            loadingScene.Layout(gameWindow.Size);
            gameWindow.Scene = loadingScene;

            // Do the actual loading in a separate thread so it doesn't block
            // the entire game if it decides to takes a while.
            ThreadStart start = new ThreadStart(
                () => this.InitializeGameAsync()
            );

            Thread workerThread = new Thread(start);
            workerThread.Name = "Game Load";
            workerThread.Start();
        }

        private void InitializeGameAsync(){
            // First load the actual game.
            state = State.LoadingGame;
            game.InitialLoad(this);

            // Then create and switch to the initial scene.
            state = State.LoadingInitialScene;
            Scene firstScene = game.CreateInitialScene();

            // At this point, we should call IInstance.SwitchToScene() to
            // perform the loading, but that's just a shortcut for launching
            // LoadAndSwitchToSceneAsync() in the worker thread. Since we're
            // already in the worker thread, we can just do that directly.
            LoadAndSwitchToSceneAsync(firstScene);
        }

        private void LoadAndSwitchToSceneAsync(Scene nextScene){
            // Let the scene load its resources.
            nextScene.Instance = this;
            nextScene.Load(this);

            // Let it perform its layout before its first rendering.
            nextScene.Layout(gameWindow.Size);

            // The scene is ready. Switch states and display it.
            state = State.Running;
            gameWindow.Scene = nextScene;
        }

        void IInstance.Quit(){
            gameWindow.Close();
        }

        void IInstance.SwitchToScene(Scene newScene){
            // Switch to the loading state.
            state = State.LoadingScene;

            // Reset and display the loading scene.
            loadingScene.SetDisplay(0, "");
            gameWindow.Scene = loadingScene;

            // Perform the loading in a new thread so we don't block the main
            // game thread.
            ThreadStart start = new ThreadStart(
                () => this.LoadAndSwitchToSceneAsync(newScene)
            );

            Thread workerThread = new Thread(start);
            workerThread.Name = "Scene Load";
            workerThread.Start();
        }

        Vector2u IInstance.TargetSize {
            get {
                return gameWindow.Size;
            }
        }

        Public.Game IInstance.Game {
            get {
                return game;
            }
        }

        void IProgressReport.ReportProgress(float ratio, string msg){
            // Constrain the progress ratio within [0;1].
            if( ratio < 0.0f )
                ratio = 0.0f;

            if( ratio > 1.0f )
                ratio = 1.0f;

            // When loading the game, two things happen during one loading screen:
            // the game itself is loaded, and the initial scene is loaded. To
            // better represent this, we divide the progress of these 2 steps
            // by 2, so that loading the game is 50% progress, and loading the
            // initial scene is the other 50%. Not doing so would result in the
            // progress bar jumping back to 0%, which would be confusing.
            if( state == State.LoadingGame ){
                ratio = ratio * 0.5f;
                loadingScene.SetDisplay(ratio, msg);
            }

            if( state == State.LoadingInitialScene ){
                ratio = 0.5f + ratio * 0.5f;
                loadingScene.SetDisplay(ratio, msg);
            }

            // When loading a normal scene (for example, to change scenes), we
            // can directly display the given progress.
            if( state == State.LoadingScene ){
                loadingScene.SetDisplay(ratio, msg);
            }

        }

        public void Run(){
            // Run the main game loop.
            while( gameWindow.IsOpen )
                gameWindow.Update();

            // Let the game finalize itself.
            if( game != null )
                game.Quit();
        }

        static void Main(string[] args){
            try {
                // Name the main thread.
                Thread.CurrentThread.Name = "Main Loop";

                // Create a game instance
                CInstance instance = new CInstance();
                instance.Initialize();
                instance.Run();
            } catch( Exception e ){
                Console.WriteLine("Erreur: " + e.Message);
                while( true ) ;
            }
        }
    }
}

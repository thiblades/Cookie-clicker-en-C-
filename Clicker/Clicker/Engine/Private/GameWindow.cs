using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.Engine.Private {
    public class GameWindow {
        private Configuration configuration = null;
        private RenderWindow window = null;
        private Scene currentScene = null;

        public Scene Scene {
            get {
                return currentScene;
            }

            set {
                // TODO: Have a way of unloading the old scene?

                // Store the new scene.
                currentScene = value;

                // Inform it of the target size.
                currentScene.Layout(window.Size);
            }
        }

        public GameWindow(Configuration config) {
            configuration = config;
        }

        public void Open(string title) {
            VideoMode vm = new VideoMode();
            vm.Width = configuration.Resolution.Width;
            vm.Height = configuration.Resolution.Height;

            window = new RenderWindow(vm, title);
            window.SetFramerateLimit(configuration.Framerate);
            window.SetVerticalSyncEnabled(configuration.VSyncEnabled);

            window.Closed += (sender, e) =>  {
                window.Close();
            };

            window.Resized += (object sender, SizeEventArgs e) => {
                // Update the OpenGL viewport
                View viewport = new View();
                viewport.Center = new Vector2f(e.Width / 2, e.Height / 2);
                viewport.Size = new Vector2f(e.Width, e.Height);
                window.SetView(viewport);

                // If we have a scene, tell it to recalculate its layout
                if( Scene != null )
                    Scene.Layout(new Vector2u(e.Width, e.Height));
            };
        }

        public void Update() {
            window.DispatchEvents();

            if( currentScene != null ) {
                float dt = 1.0f / (float) configuration.Framerate;
                currentScene.Update(dt);
                currentScene.Render(window);
            }

            window.Display();
        }

        public void Close(){
            window.Close();
        }

        public bool IsOpen {
            get {
                return window.IsOpen;
            }
        }

        public Vector2u Size {
            get {
                return window.Size;
            }
        }
    }
}

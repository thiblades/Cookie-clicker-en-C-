using System;

using Clicker.Engine.Public;
using Clicker.GameKit;

using SFML.System;
using SFML.Graphics;

namespace Clicker.Game {
    public class GameplayScene: Scene {
        public const string BACKGROUND_IMAGE = "Assets/Background.png";

        private BackgroundImage background;
        private TimeAccumulator time;

        public GameplayScene() {
        }

        public override void Load(IProgressReport pr){
            // Load all images
            pr.ReportProgress(0, "Chargement des images...");
            background = new BackgroundImage(BACKGROUND_IMAGE);

            // Prpeare the time accumulator
            time = new TimeAccumulator();
        }

        public override void Layout(Vector2u newSize){
            background.Layout(newSize);
        }

        public override void Update(float dt){
            time.Frame(dt);
        }

        public override void Render(RenderTarget rt){
            background.Render(rt);
        }
    }
}

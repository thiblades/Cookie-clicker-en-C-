using System;
using SFML.System;
using SFML.Graphics;

namespace Clicker.Engine {
    public class LoadingScene : Scene {
        private Text loadingText;

        public LoadingScene() {
            loadingText = new Text();
            loadingText.Font = new Font("Assets/GenericFont.otf");
            loadingText.Color = Color.White;
            loadingText.DisplayedString = "Chargement...";
            loadingText.CharacterSize = 120;
        }

        public void Display(RenderTarget target) {
            target.Clear(Color.Black);
            target.Draw(loadingText);
        }

        public void Update(RenderTarget target) {
            FloatRect bounds = loadingText.GetLocalBounds();

            loadingText.Position = new Vector2f(
                (target.Size.X - bounds.Width) / 2,
                (target.Size.Y - bounds.Height) / 2
            );
        }
    }
}

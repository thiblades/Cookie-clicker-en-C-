using System;

using SFML.System;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.Engine.Private {
    public class LoadingScene : Scene {
        private string message = "";
        private float progress = 0;

        private Text loadingText = null;

        public LoadingScene() {
        }

        public void SetDisplay(float progressRatio, string msg){
            // Store the new stats.
            progress = progressRatio;
            message = msg;

            // Update the display if it's ready.
            if( loadingText != null ) {
                loadingText.DisplayedString = msg;

                // Force a layout so the text re-centers for its new length.
                Layout(Instance.TargetSize);
            }
        }

        override public void Load(IProgressReport pr){
            // Just load the font
            loadingText = new Text();
            loadingText.Font = new Font("Assets/GenericFont.otf");
            loadingText.Color = Color.White;
            loadingText.DisplayedString = "";
            loadingText.CharacterSize = 60;
        }

        override public void Layout(Vector2u newSize){
            // Center the status text.
            FloatRect bounds = loadingText.GetLocalBounds();

            loadingText.Position = new Vector2f(
                (newSize.X - bounds.Width) / 2,
                (newSize.Y - bounds.Height) / 2
            );
        }

        override public void Update(float dt){
        }

        override public void Render(RenderTarget target){
            target.Clear(Color.Black);
            target.Draw(loadingText);
        }
    }
}

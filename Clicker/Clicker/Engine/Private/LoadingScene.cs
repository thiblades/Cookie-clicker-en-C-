using System;

using SFML.System;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.Engine.Private {
    public class LoadingScene : Scene {
        private string message = "";
        private float progress = 0;

        private string logoPath = null;
        private Text loadingText = null;
        private Sprite logoSprite = null;
        private RectangleShape progressBar = null;
        private float progressBarWidth;

        public static int PROGRESS_BAR_HEIGHT = 12;

        public LoadingScene(Public.Game game) {
            logoPath = game.Logo;
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
            // Load the font we'll use for the logo
            loadingText = new Text();
            loadingText.Font = new Font("Assets/GenericFont.otf");
            loadingText.Color = Color.Black;
            loadingText.DisplayedString = "";
            loadingText.CharacterSize = 24;

            // Load the logo we'll be displaying
            Texture texture = new Texture(logoPath);
            texture.Repeated = false;
            texture.Smooth = true;
            logoSprite = new Sprite(texture);

            // Create the rectangle shape we need
            progressBar = new RectangleShape();
            progressBar.FillColor = Color.Black;
            progressBar.Size = new Vector2f(0, PROGRESS_BAR_HEIGHT);
        }

        override public void Layout(Vector2u newSize){
            float TargetLogoSize = newSize.Y / 2.0f;

            // Scale and center the logo
            logoSprite.Scale = new Vector2f(
                TargetLogoSize / logoSprite.Texture.Size.X,
                TargetLogoSize / logoSprite.Texture.Size.Y
            );

            logoSprite.Position = new Vector2f(
                (newSize.X - TargetLogoSize) / 2,
                (newSize.Y - TargetLogoSize) / 2
            );

            // Now place the text right below that
            FloatRect bounds = loadingText.GetLocalBounds();

            loadingText.Position = new Vector2f(
                // Center horizontally
                (newSize.X - bounds.Width) / 2,

                // Place right under the logo vertically
                logoSprite.Position.Y + TargetLogoSize + bounds.Height
            );

            // Finally, place the progress bar right under this
            progressBarWidth = TargetLogoSize * 1.5f;

            progressBar.Size = new Vector2f(progressBarWidth, PROGRESS_BAR_HEIGHT);

            progressBar.Position = new Vector2f(
                (newSize.X - progressBarWidth) / 2,
                loadingText.Position.Y + bounds.Height + 12
            );

            progressBar.Scale = new Vector2f(progress, 1);
        }

        override public void Update(float dt){
        }

        override public void Render(RenderTarget target){
            target.Clear(Color.White);
            target.Draw(logoSprite);
            target.Draw(loadingText);
            target.Draw(progressBar);
        }
    }
}

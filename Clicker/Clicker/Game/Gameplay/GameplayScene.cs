using System;

using Clicker.Engine.Public;
using Clicker.GameKit;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Clicker.Game {
    public class GameplayScene: Scene {
        public const string BACKGROUND_IMAGE = "Assets/Background.png";
        public const string PANEL_BACKGROUND_IMAGE = "Assets/BonusPanel.png";

        private BackgroundImage background;
        private BackgroundImage panelBackground;
        private TimeAccumulator time;
        private BonusPanel bonusPanel;

        private Sprite cookieButton;
        private Font font;
        private Text scoreText;
        private Text perSecondText;

        private GameState state;

        public GameplayScene(GameState gameState) {
            state = gameState;
        }

        public override void Load(IProgressReport pr){
            // Prpeare the time accumulator
            time = new TimeAccumulator();

            // Load all images
            pr.ReportProgress(0, "Chargement des images...");
            background = new BackgroundImage(BACKGROUND_IMAGE);
            panelBackground = new BackgroundImage(PANEL_BACKGROUND_IMAGE);

            cookieButton = new Sprite(new Texture("Assets/CookieButton.png"));
            cookieButton.Origin = new Vector2f(cookieButton.Texture.Size.X / 2, cookieButton.Texture.Size.Y / 2);
            cookieButton.Scale = new Vector2f(.5f, .5f);

            // Load text
            pr.ReportProgress(0.5f, "Chargement des polices...");
            font = new Font("Assets/GenericFont.otf");
            scoreText = new Text("", font);
            scoreText.Color = Color.White;
            scoreText.CharacterSize = 40;
            scoreText.Style = Text.Styles.Bold;
            scoreText.Position = new Vector2f(20, 20);

            perSecondText = new Text("", font);
            perSecondText.Color = Color.White;
            perSecondText.CharacterSize = 30;
            perSecondText.Style = Text.Styles.Regular;
            perSecondText.Position = new Vector2f(25, 70);

            // Prepare the bonus panel
            pr.ReportProgress(0.75f, "Préparation des bonus...");
            bonusPanel = new BonusPanel(state, font);

        }

        public override void Layout(Vector2u newSize){
            // Reposition the background images.
            background.Layout(newSize);
            panelBackground.Layout(newSize);

            // Reposition the cookie buttons.
            cookieButton.Position = new Vector2f(newSize.X * 0.33f, newSize.Y * 0.5f);

            // Reposition the bonus panel.
            bonusPanel.Layout(newSize);

            // The score indicators fall right in the top left corner, which
            // happens to be the origin of coordinates, so no need to update ;)
        }

        public override void Update(float dt){
            // Account for time (for animations).
            time.Frame(dt);

            // Update the game state.
            state.Update(dt);

            // Update the bonus panel
            bonusPanel.Update();

            // Update the displayed statistics.
            scoreText.DisplayedString = String.Format(
                "Score: {0}",
                NumberFormatter.Format(state.Score)
            );

            float scorePerSecond = state.ScorePerSecond;

            if( scorePerSecond > 0 )
                perSecondText.DisplayedString = "+" + NumberFormatter.Format(scorePerSecond) + " par seconde";
        }

        public override void Render(RenderTarget rt){
            Layout(rt.Size);

            background.Render(rt);
            panelBackground.Render(rt);
            bonusPanel.Render(rt);
            rt.Draw(cookieButton);
            rt.Draw(scoreText);
            rt.Draw(perSecondText);
        }

        public override void OnMouseDown(MouseButtonEventArgs evt){
            if( cookieButton.GetGlobalBounds().Contains(evt.X, evt.Y)){
                cookieButton.Scale = new Vector2f(.6f, .6f);
            } else if( bonusPanel.ContainsPoint(evt.X, evt.Y) ){
                bonusPanel.OnMouseDown(evt.Button, evt.X, evt.Y);
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs evt) {
            if( cookieButton.GetGlobalBounds().Contains(evt.X, evt.Y) ) {
                cookieButton.Scale = new Vector2f(.5f, .5f);
                state.Click();
            }

            // FIX: A player might press the mouse button within the cell, then
            //      move the cursor outside the cell and release the button. The
            //      only clean way to handle this properly, is to let the
            //      bonus panel know about every MouseUp event.
            bonusPanel.OnMouseUp(evt.Button, evt.X, evt.Y);
        }
    }
}

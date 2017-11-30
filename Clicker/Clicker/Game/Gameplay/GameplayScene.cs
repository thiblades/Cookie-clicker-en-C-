using System;

using Clicker.Engine.Public;
using Clicker.GameKit;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Clicker.Game {
    public class GameplayScene: Scene {
        public const string BackgroundImagePath = "Assets/Background.png";
        public const string PanelBackgroundImagePath = "Assets/BonusPanel.png";

        public const ulong ScoreWhereMusicChanges = 1000000;
        public const float MusicBPM = 140.0f;

        private BackgroundImage background;
        private BackgroundImage panelBackground;
        private TimeAccumulator time;
        private BonusPanel bonusPanel;
        private CenteredText statusLine;

        private Sprite cookieButton;
        private Font font;
        private Text scoreText;
        private Text perSecondText;

        private GameState state;
        private SoundManager sound;

        private void OnScoreChange(ulong oldScore, ulong newScore){
            statusLine.DisplayedString = StatusMessages.Message(newScore);

            // Switch to track 2 when the player reaches the threshold.
            if( oldScore < ScoreWhereMusicChanges && newScore >= ScoreWhereMusicChanges ){
                sound.PlayTrack(1);
            }

            // Switch back to track 1 when the player falls back under it.
            if( oldScore >= ScoreWhereMusicChanges && newScore < ScoreWhereMusicChanges ){
                sound.PlayTrack(0);
            }
        }

        public GameplayScene(GameState gameState) {
            state = gameState;
            state.OnScoreChange += this.OnScoreChange;
        }

        public override void Load(IProgressReport pr){
            // Prpeare the time accumulator
            time = new TimeAccumulator();

            // Load all images
            pr.ReportProgress(0.00f, "Chargement des images...");
            background = new BackgroundImage(BackgroundImagePath);
            panelBackground = new BackgroundImage(PanelBackgroundImagePath);

            cookieButton = new Sprite(new Texture("Assets/CookieButton.png"));
            cookieButton.Origin = new Vector2f(cookieButton.Texture.Size.X / 2, cookieButton.Texture.Size.Y / 2);
            cookieButton.Scale = new Vector2f(.5f, .5f);

            // Load text
            pr.ReportProgress(0.25f, "Chargement des polices...");
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

            // Prepare the status line
            statusLine = new CenteredText("TEST TEST TEST TEST", font, 40);
            statusLine.CenterX = true;
            statusLine.CenterY = true;
            statusLine.Color = Color.White;

            // Prepare the bonus panel
            pr.ReportProgress(0.50f, "Préparation des bonus...");
            bonusPanel = new BonusPanel(state, font);

            // Prepare the sounds
            pr.ReportProgress(0.75f, "Chargement des sons...");
            sound = new SoundManager();

            // Start playing the right track
            if( state.Score < ScoreWhereMusicChanges )
                sound.PlayTrack(0);
            else
                sound.PlayTrack(1);
        }

        public override void Layout(Vector2u newSize){
            // Reposition the background images.
            background.Layout(newSize);
            panelBackground.Layout(newSize);

            // Reposition the cookie buttons.
            cookieButton.Position = new Vector2f(newSize.X * 0.33f, newSize.Y * 0.5f);

            // Reposition the bonus panel.
            bonusPanel.Layout(newSize);

            // Recenter the status line
            if( !statusLine.DisplayedString.Equals("") ){
                statusLine.UpdateCentering(newSize.X, newSize.Y);
                statusLine.SetYPosition(newSize.Y - 80);
            }

            // The score indicators fall right in the top left corner, which
            // happens to be the origin of coordinates, so no need to update ;)
        }

        public override void Update(float dt){
            // Account for time (for animations).
            time.Frame(dt);

            // Update the sound first, because any interruption in the sound will
            // be noticed immediately, whereas drops in framerate can go
            // unnoticed if they remain minor.
            sound.Update(dt);

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

            // Update the status line's animation
            if( !statusLine.DisplayedString.Equals("") ) {
                float w = 2 * MathF.PI * MusicBPM / 60.0f; // \omega = 2 * \pi * f
                float factor = (MathF.Sin(w * time.t) + 1.0f) / 2;

                Vector2f newScale = new Vector2f();
                newScale.X = 1.0f + (factor - 0.6f) + 0.6f;
                newScale.Y = 1.0f + (factor - 0.9f) + 0.9f;
                statusLine.Scale = newScale;
            }
        }

        public override void Render(RenderTarget rt){
            Layout(rt.Size);

            background.Render(rt);
            panelBackground.Render(rt);
            bonusPanel.Render(rt);
            rt.Draw(cookieButton);
            rt.Draw(scoreText);
            rt.Draw(perSecondText);

            if( !statusLine.DisplayedString.Equals("") )
                rt.Draw(statusLine);
        }

        public override void Exit(){
            // When we're leaving the scene, stop the music.
            sound.StopEverything();
        }

        public override void OnMouseDown(MouseButtonEventArgs evt){
            if( cookieButton.GetGlobalBounds().Contains(evt.X, evt.Y)){
                // If the user presses the mouse button on the cookie, make it
                // bigger to acknowledge the action.
                cookieButton.Scale = new Vector2f(.6f, .6f);
            } else if( bonusPanel.ContainsPoint(evt.X, evt.Y) ){
                // If the user presses a mouse button within the bonus panel,
                // forward that.
                bonusPanel.OnMouseDown(evt.Button, evt.X, evt.Y);
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs evt) {
            // When the player releases the mouse button over the cookie button,
            // count that as a click, and make a click sound.
            if( cookieButton.GetGlobalBounds().Contains(evt.X, evt.Y) ) {
                cookieButton.Scale = new Vector2f(.5f, .5f);
                state.Click();
                sound.PlayClick();
            }

            // FIX: A player might press the mouse button within the cell, then
            //      move the cursor outside the cell and release the button. The
            //      only clean way to handle this properly, is to let the
            //      bonus panel know about every MouseUp event.
            bonusPanel.OnMouseUp(evt.Button, evt.X, evt.Y);
        }

        public override void OnKeyUp(KeyEventArgs e) {
            if( e.Code == Keyboard.Key.Escape ){
                GetGame().GoToPause(state);
            }
        }

        private ClickerGame GetGame() {
            ClickerGame game = (ClickerGame) Instance.Game;
            return game;
        }
    }
}

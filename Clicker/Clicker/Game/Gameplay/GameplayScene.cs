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

        public const ulong SCORE_WHERE_MUSIC_CHANGES = 1000000;

        private BackgroundImage background;
        private BackgroundImage panelBackground;
        private TimeAccumulator time;
        private BonusPanel bonusPanel;

        private Sprite cookieButton;
        private Font font;
        private Text scoreText;
        private Text perSecondText;

        private GameState state;
        private SoundManager sound;

        private void OnScoreChange(ulong oldScore, ulong newScore){
            // Switch to track 2 when the player reaches the threshold.
            if( oldScore < SCORE_WHERE_MUSIC_CHANGES && newScore >= SCORE_WHERE_MUSIC_CHANGES ){
                sound.PlayTrack(1);
            }

            // Switch back to track 1 when the player falls back under it.
            if( oldScore >= SCORE_WHERE_MUSIC_CHANGES && newScore < SCORE_WHERE_MUSIC_CHANGES ){
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
            background = new BackgroundImage(BACKGROUND_IMAGE);
            panelBackground = new BackgroundImage(PANEL_BACKGROUND_IMAGE);

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

            // Prepare the bonus panel
            pr.ReportProgress(0.50f, "Préparation des bonus...");
            bonusPanel = new BonusPanel(state, font);

            pr.ReportProgress(0.75f, "Chargement des sons...");
            sound = new SoundManager();
            sound.PlayTrack(0);
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

        public override void Exit(){
            // When we're leaving the scene, stop the music.
            sound.StopEverything();
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

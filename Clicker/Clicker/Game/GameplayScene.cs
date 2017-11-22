using System;

using Clicker.Engine.Public;
using Clicker.GameKit;

using SFML.System;
using SFML.Graphics;

namespace Clicker.Game
{
    public class GameplayScene : Scene
    {
        public const string BACKGROUND_IMAGE = "Assets/Background.png";

        private BackgroundImage background;
        private TimeAccumulator time;

        // Score
        private Font scoreFont;
        private Text scoreText;
        private string score = "Score : 0";
        private Color scoreColor = Color.White;

        private const uint SCORE_SIZE = 60;
        private const uint SCORE_OFFSET = 10;


        public GameplayScene()
        {
        }

        public override void Load(IProgressReport pr)
        {
            // Load all images
            pr.ReportProgress(0, "Chargement des images...");
            background = new BackgroundImage(BACKGROUND_IMAGE);

            // Load the font used for the entire menu
            pr.ReportProgress(0, "Chargement texte");
            scoreFont = new Font("Assets/GenericFont.otf");

            // Prepare the title
            scoreText = new Text(score, scoreFont, SCORE_SIZE);
            scoreText.Color = scoreColor;

            // Prpeare the time accumulator
            time = new TimeAccumulator();
        }

        public override void Layout(Vector2u newSize)
        {
            background.Layout(newSize);

            scoreText.Position = new Vector2f(SCORE_OFFSET, SCORE_OFFSET);
            scoreText.CharacterSize = Math.Min((newSize.X * SCORE_SIZE) / 1920, (newSize.Y * SCORE_SIZE) / 1080);

        }

        public override void Update(float dt)
        {
            time.Frame(dt);
        }

        public override void Render(RenderTarget rt)
        {
            background.Render(rt);
            rt.Draw(scoreText);
        }
    }
}

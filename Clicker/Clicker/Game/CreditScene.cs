using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;
using SFML.System;
using SFML.Window;

namespace Clicker.Game {
    public class CreditScene: Scene {

        public const string BACKGROUND_IMAGE = "Assets/TitleBackground.png";
        public const string TITLE = "Credits";
        public const uint TITLE_SIZE = 100;
        public const uint TITLE_MARGIN = 200;
        public const string CREDITS = "Réalisé par l'équipe 'TLJ' pour Let's Dev\n\nEquipe composée de :\n"+
            "Jessy Vanderaugstraete\nThibaut Fenain\nLilian Petitpas";
        public const uint CREDITS_SIZE = 35;


        private Font font;
        private BackgroundImage background;
        private CenteredText titleText;
        private CenteredText creditsText;
        private TimeAccumulator time = new TimeAccumulator();

        public Color titleColor;


        public CreditScene() {
            
        }

        public override void Layout(Vector2u newSize)
        {
            background.Layout(newSize);

            // Update size
            titleText.CharacterSize = (TITLE_SIZE * Math.Min(newSize.X, newSize.Y)) / 1080;
            creditsText.CharacterSize = (CREDITS_SIZE * Math.Min(newSize.X, newSize.Y)) / 1080;

            // Center all the items and the title horizontally
            titleText.UpdateCentering(newSize);
            creditsText.UpdateCentering(newSize);

            // Center the entire thing vertically
            float fullHeight = titleText.Dimensions.Y + TITLE_MARGIN + creditsText.Dimensions.Y;
            float yOffset = (newSize.Y - fullHeight) / 2;

            titleText.SetYPosition(yOffset);

        }

        public override void Load(IProgressReport pr)
        {
            // Prepare the background image
            pr.ReportProgress(0, "Chargement image de fond");
            background = new BackgroundImage(BACKGROUND_IMAGE);

            // Load the font used for the entire scene
            pr.ReportProgress(0.5f, "Chargement texte");
            font = new Font("Assets/GenericFont.otf");

            // Prepare colors
            titleColor = new Color(253, 191, 86);

            // Prepare the title
            titleText = new CenteredText(TITLE, font, TITLE_SIZE);
            titleText.Color = titleColor;
            titleText.CenterX = true;

            // Prepare the credits
            creditsText = new CenteredText(CREDITS, font, CREDITS_SIZE);
            creditsText.Color = titleColor;
            creditsText.CenterX = true;
            creditsText.CenterY = true;
        }

        public override void Render(RenderTarget rt)
        {
            background.Render(rt);
            rt.Draw(titleText);
            rt.Draw(creditsText);

        }

        public override void Update(float dt)
        {
            time.Frame(dt);
        }

        private void OnMainMenu()
        {
            GetGame().GoToMainMenu();
        }

        private ClickerGame GetGame()
        {
            ClickerGame game = (ClickerGame)Instance.Game;
            return game;
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            OnMainMenu();
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            OnMainMenu();
        }
    }
}

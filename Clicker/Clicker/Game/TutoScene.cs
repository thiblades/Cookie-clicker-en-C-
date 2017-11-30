using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;
using SFML.System;
using SFML.Window;

namespace Clicker.Game {
    public class TutoScene: Scene {

        public const string BACKGROUND_IMAGE = "Assets/TitleBackground.png";
        public const string TITLE = "Tutoriel";
        public const uint TITLE_SIZE = 100;
        public const uint TITLE_MARGIN = 200;
        public const string CREDIT = "L'objectif du jeu est d'obtenir le meilleur score en\n\tcliquant sur le verre qui se présente au centre de l'écran.\n"+
            "Vous pouvez utiliser des bonus afin de parvenir plus rapidement à votre objectif.\n"+"" +
            "N'oubliez pas que la consommation d'alcool est dangeureuse pour la santé.\n"+
            "Ne doit pas être consommé avec modération.";
        public const uint CREDIT_SIZE = 35;


        private Font font;
        private BackgroundImage background;
        private CenteredText titleText;
        private CenteredText creditText;
        private TimeAccumulator time = new TimeAccumulator();

        public Color titleColor;


        public TutoScene() {
            
        }

        public override void Layout(Vector2u newSize)
        {
            background.Layout(newSize);

            // Center all the items and the title horizontally
            titleText.UpdateCentering(newSize);
            creditText.UpdateCentering(newSize);

            // Center the entire thing vertically
            float fullHeight = titleText.Dimensions.Y + TITLE_MARGIN + creditText.Dimensions.Y;
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

            // Prepare the credit
            creditText = new CenteredText(CREDIT, font, CREDIT_SIZE);
            creditText.Color = titleColor;
            creditText.CenterX = true;
            creditText.CenterY = true;
        }

        public override void Render(RenderTarget rt)
        {
            background.Render(rt);
            rt.Draw(titleText);
            rt.Draw(creditText);

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

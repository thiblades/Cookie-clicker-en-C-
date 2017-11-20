using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.GameKit {

    public class MenuScene : Scene {
        // Public interface
        public class Item {
            public delegate void OnSelect();

            public String title;
            public OnSelect onSelect;

            public Item(String title, OnSelect onSelect) {
                this.title = title;
                this.onSelect = onSelect;
            }
        };

        public string title;
        public string backgroundImage;
        public Item[] items;

        public Color titleColor = Color.White;
        public Color neutralColor = Color.White;
        public Color focusColor = Color.Blue;

        // Helper class
        private class MenuItemInfo {
            public CenteredText text;
            public Item item;

            public MenuItemInfo(Item menuItem, Font font, uint charSize) {
                item = menuItem;
                text = new CenteredText(item.title, font, charSize);
                text.CenterX = true;
            }

            public void Update(Vector2u newSize){
                text.UpdateCentering(newSize);
            }
        }

        private const uint TITLE_SIZE = 100;
        private const uint OPTION_SIZE = 50;
        private const float MARGIN = 30;
        private const float TITLE_MARGIN = 30;

        private BackgroundImage background;
        private Font font;
        private CenteredText titleText;
        private MenuItemInfo[] itemsInfo;
        private uint selectedItem = 0;
        private TimeAccumulator time = new TimeAccumulator();

        public MenuScene() {
        }

        public override void Load(IProgressReport pr) {
            // Prepare the background image and vertex array
            pr.ReportProgress(0, "Chargement image de fond");
            background = new BackgroundImage(backgroundImage);

            // Load the font used for the entire menu
            pr.ReportProgress(0, "Chargement texte");
            font = new Font("Assets/GenericFont.otf");

            // Prepare the title
            titleText = new CenteredText(title, font, TITLE_SIZE);
            titleText.Color = titleColor;
            titleText.CenterX = true;

            // Prepare the items
            itemsInfo = new MenuItemInfo[items.Length];
            for( int i = 0; i < items.Length; ++i ) {
                itemsInfo[i] = new MenuItemInfo(items[i], font, OPTION_SIZE);
                itemsInfo[i].text.Color = neutralColor;
            }
        }

        public override void Layout(Vector2u newSize) {
            background.Layout(newSize);

            // Center all the items and the title horizontally
            titleText.UpdateCentering(newSize);

            float combinedItemHeight = 0;

            for( int i = 0; i < items.Length; ++i ) {
                itemsInfo[i].Update(newSize);

                combinedItemHeight += itemsInfo[i].text.Dimensions.Y + MARGIN;
            }

            // Center the entire menu vertically
            float fullHeight = titleText.Dimensions.Y + TITLE_MARGIN + combinedItemHeight;
            float yOffset = (newSize.Y - fullHeight) / 2;

            titleText.SetYPosition(yOffset);

            float currentY = titleText.Position.Y + titleText.Dimensions.Y + TITLE_MARGIN;

            for( int i = 0; i < items.Length; ++i ) {
                itemsInfo[i].text.SetYPosition(currentY);
                currentY += itemsInfo[i].text.Dimensions.Y + MARGIN;
            }
        }

        public override void Update(float dt) {
            time.Frame(dt);

            for( int i = 0; i < items.Length; ++i ){
                if( i == selectedItem ){
                    itemsInfo[i].text.Color = focusColor;
                } else {
                    itemsInfo[i].text.Color = neutralColor;
                    itemsInfo[i].text.Scale = new Vector2f(1, 1);
                }
            }

            float scale = 1.0f + MathF.Sin(2 * MathF.PI * 1.0f * time.t) * 0.05f;
            itemsInfo[selectedItem].text.Scale = new Vector2f(scale, scale);
        }

        public override void Render(RenderTarget rt) {
            background.Render(rt);
            rt.Draw(titleText);

            for( int i = 0; i < items.Length; ++i )
                rt.Draw(itemsInfo[i].text);
        }

        public override void OnKeyDown(KeyEventArgs e){
            if( e.Code == Keyboard.Key.Up ){
                if( selectedItem > 0 )
                    selectedItem--;
            }

            if( e.Code == Keyboard.Key.Down ){
                if( selectedItem < items.Length - 1 )
                    selectedItem++;
            }

            if( e.Code == Keyboard.Key.Return ){
                items[selectedItem].onSelect();
            }
        }

        public override void OnMouseMove(MouseMoveEventArgs e) {
            for( uint i = 0; i < items.Length; ++i ){
                FloatRect boundingRect = itemsInfo[i].text.GetGlobalBounds();

                if( boundingRect.Contains(e.X, e.Y) )
                    selectedItem = i;
            }
        }
    }
}

/*
        private VertexArray     bgVertices;
        private RenderStates    bgStates;

        private Font font;
        private Text            titleText;
        private Text[]          itemText;
        
        public override void Load(IProgressReport pr) {
            // Prepare the background image and vertex array
            pr.ReportProgress(0, "Chargement image de fond");

            bgStates = new RenderStates();
            bgStates.Texture = new Texture(bgImage);

            // Load the font we'll use for the whole menu.
            pr.ReportProgress(0, "Chargement police de caractères");
            font = new Font("Assets/GenericFont.otf");

            titleText = new Text(title, font);
            titleText.CharacterSize = 100;
            titleText.Position = new Vector2f(0, 0);
            titleText.Color = Color.White;

            // Create all title texts. They'll be positioned in Layout()
            pr.ReportProgress(0, "Préparation des textes");
            itemText = new Text[items.Length];

            for( int i = 0; i < items.Length; ++i ) {
                itemText[i] = new Text(items[i].title, font);
                itemText[i].CharacterSize = 50;
                itemText[i].Color = Color.White;
            }
        }

        public override void Layout(Vector2u newSize) {
            float w = newSize.X, h = newSize.Y;

            bgVertices = new VertexArray(PrimitiveType.Triangles);
            bgVertices.Append(new Vertex(new Vector2f(0, 0), new Vector2f(0, 0)));
            bgVertices.Append(new Vertex(new Vector2f(newSize.X, 0), new Vector2f(bgStates.Texture.Size.X, 0)));
            bgVertices.Append(new Vertex(new Vector2f(newSize.X, newSize.Y), new Vector2f(bgStates.Texture.Size.X, bgStates.Texture.Size.Y)));

            //bgVertices.Append(new Vertex(new Vector2f(-w, +h), new Vector2f(0, 1)));

            return;
            FloatRect titleRect = titleText.GetLocalBounds();

            titleText.Position = new Vector2f(
                (newSize.X - titleRect.Width) / 2,
                0
            );

            float remainingSpace = newSize.Y - titleRect.Height;
        }

        public override void Update(float dt) {

        }

        public override void Render(RenderTarget rt) {
            rt.Clear(Color.Red);
            rt.Draw(bgVertices, bgStates);
        }
*/
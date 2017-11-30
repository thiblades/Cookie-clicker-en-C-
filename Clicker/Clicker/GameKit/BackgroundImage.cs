using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Clicker.GameKit {
    /// <summary>
    /// An image that automatically rescales to take up the entire screen.
    /// </summary>
    /// 
    /// Often, we want a particular image to take the entire screen, but SFML's
    /// only concept of resizing is through transforms. This class will
    /// recalculate the correct ratios on each layout to ensure the image
    /// always takes up the entire screen.
    public class BackgroundImage {
        public string imagePath;

        private Texture texture;
        private Sprite sprite;

        public BackgroundImage(string path) {
            imagePath = path;

            texture = new Texture(path);
            sprite = new Sprite(texture);
            sprite.Position = new Vector2f(0, 0);
        }

        public void Layout(Vector2u newSize){
            sprite.Scale = new Vector2f(
                newSize.X / (float) texture.Size.X,
                newSize.Y / (float) texture.Size.Y
            );
        }

        public void Render(RenderTarget rt){
            rt.Draw(sprite);
        }

        public void Render(RenderTarget rt, RenderStates rs){
            rt.Draw(sprite, rs);
        }
    }
}

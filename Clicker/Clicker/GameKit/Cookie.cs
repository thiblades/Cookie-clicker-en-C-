using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clicker.GameKit
{
    /// <summary>
    /// Oh a cookie !
    /// Seriously, this is the main "Cookie"
    /// </summary>
    public class Cookie
    {
        public string imagePath;

        private Texture texture;
        private Sprite sprite;
        private TimeAccumulator time;

        private bool animate;

        public bool Animate
        {
            get
            {
                return this.animate;
            }
            set
            {
                animate = value;
                time.Reset();
            }
        }

        public Cookie(string path)
        {
            imagePath = path;

            texture = new Texture(path);
            sprite = new Sprite(texture);
            time = new TimeAccumulator();
        }

        public void Layout(Vector2u newSize)
        {
            float x = (newSize.X - sprite.GetLocalBounds().Width) / 2;
            float y = (newSize.Y - sprite.GetLocalBounds().Height) / 2;

            sprite.Position = new Vector2f(x, y);
        }

        public void Update(float rt)
        {
            time.Frame(rt);

            // Make something more serious
            if (Animate && time.t < 0.5)
            {
                sprite.Color = Color.Green;
            }
            else
            {
                sprite.Color = Color.White;
            }

        }

        public void Render(RenderTarget rt)
        {
            rt.Draw(sprite);
        }

        public FloatRect GetGlobalBounds()
        {
            return sprite.GetGlobalBounds();
        }
    }
}

using System;

using SFML.Graphics;
using SFML.System;

namespace Clicker.GameKit {
    /// <summary>
    /// This class implements text that is centered within a given rectangle,
    /// either horizontally, vertically, or both.
    /// </summary>
    public class CenteredText: Text {
        public bool CenterX = false;
        public bool CenterY = false;

        public Vector2f Dimensions;

        public CenteredText(string text, Font font, uint charSize) : base(text, font, charSize){
            RecalculateSize();
            Origin = new Vector2f(Dimensions.X / 2, Dimensions.Y / 2);
        }

        public void RecalculateSize(){
            Dimensions = new Vector2f(
                GetLocalBounds().Width,
                GetLocalBounds().Height
            );
        }

        public void UpdateCentering(float width, float height){
            Vector2u newSize = new Vector2u((uint) width, (uint) height);
            this.UpdateCentering(newSize);
        }

        public void UpdateCentering(Vector2u newSize){
            RecalculateSize();

            float x = CenterX ? (float) newSize.X / 2 : Position.X;
            float y = CenterY ? (float) newSize.Y / 2 : Position.Y;
            Position = new Vector2f(x, y);
        }

        public void SetXPosition(float x) {
            this.Position = new Vector2f(x, this.Position.Y);
        }

        public void SetYPosition(float y){
            this.Position = new Vector2f(this.Position.X, y);
        }
    }
}

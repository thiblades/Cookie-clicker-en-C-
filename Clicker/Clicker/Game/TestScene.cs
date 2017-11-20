using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class TestScene : Scene {
        private Color bgColor = Color.Cyan;

        override public void Load(IProgressReport pr) {

        }

        override public void Update(float dt){
            
        }

        override public void Render(RenderTarget target){
            target.Clear(bgColor);
        }

        override public void Layout(Vector2u newSize){
            
        }

        override public void OnMouseDown(MouseButtonEventArgs e){
            bgColor = Color.Red;
        }

        override public void OnMouseUp(MouseButtonEventArgs e){
            bgColor = Color.Yellow;
        }
    }
}

using System;

using SFML.System;
using SFML.Graphics;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class TestScene : Scene {
        public TestScene() {
        }

        override public void Load(IProgressReport pr) {

        }

        override public void Update(float dt){
            
        }

        override public void Render(RenderTarget target){
            target.Clear(Color.Cyan);
        }

        override public void Layout(Vector2u newSize){
            
        }
    }
}

using System;

using Clicker.Engine;
using SFML.Graphics;

namespace Clicker.Game {
    public class TestScene : Scene {
        public TestScene() {
        }

        void Scene.Display(RenderTarget target) {
            target.Clear(Color.Cyan);
        }

        void Scene.Update(RenderTarget target) {
        }
    }
}

using System;

using SFML.Graphics;

namespace Clicker.Engine {
    public interface Scene {
        void Update(RenderTarget target);
        void Display(RenderTarget target);
    }
}

using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Clicker.Engine.Public {
    public abstract class Scene {
        public IInstance Instance { get; set;  }

        // Called when this scene is being loaded.
        // Use the given progress reporter to indicate what you're doing, so
        // it can be displayed on the loading screen.
        public abstract void Load(IProgressReport pr);

        // Called when the scene first opens, and when the window resizes to
        // let the scene know to recalculate its layout.
        public abstract void Layout(Vector2u newSize);

        // Called each game update, use that to make your stuff move.
        public abstract void Update(float dt);

        // Called each time a frame needs to be displayed.
        public abstract void Render(RenderTarget target);

        public virtual void Exit(){}

        // Events
        public virtual void OnKeyUp(KeyEventArgs e){}
        public virtual void OnKeyDown(KeyEventArgs e){}
        public virtual void OnTextEntered(TextEventArgs e) { }
        public virtual void OnMouseUp(MouseButtonEventArgs e){}
        public virtual void OnMouseDown(MouseButtonEventArgs e){}
        public virtual void OnMouseMove(MouseMoveEventArgs e){}
        public virtual void OnMouseWheel(MouseWheelEventArgs e){}
        public virtual void OnMouseEnter(){}
        public virtual void OnMouseLeave(){}
    }
}

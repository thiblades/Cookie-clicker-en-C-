using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Clicker.Engine.Public {
    /// <summary>
    /// A single game "screen".
    /// </summary>
    /// 
    /// In order to divide the game into manageable sections, each main "screen"
    /// is represented by a scene. A scene can respond to user interaction such as
    /// key presses and mouse input, and gets to update itself each frame.
    /// 
    /// Game should implement one scene per major interaction.
    public abstract class Scene {
        /// <summary>
        /// Gets or sets the engine instance running the scene.
        /// </summary>
        /// 
        /// <value>The instance.</value>
        public IInstance Instance { get; set;  }

        /// <summary>
        /// This method is called before the scene is presented, and is in charge
        /// of loading everything it needs to run, such as assets. The loading
        /// stage can also include tasks such as generating levels etc. It is
        /// run in a separate thread, while the loading screen is displayed.
        /// Use the provided object to report progress and display it on the
        /// loading screen.
        /// </summary>
        /// 
        /// <param name="pr">Report loading progress using this.</param>
        public abstract void Load(IProgressReport pr);

        // Called when the scene first opens, and when the window resizes to
        // let the scene know to recalculate its layout.

        /// <summary>
        /// This method is called before the first render, and every time the
        /// game window changes size. It is in charge of laying out the various
        /// elements of the scene.
        /// </summary>
        /// 
        /// <param name="newSize">New size of the game window.</param>
        public abstract void Layout(Vector2u newSize);

        /// <summary>
        /// This method is called each frame, and is in charge of updating the
        /// state of the game's model. Examples of tasks implemented in this method
        /// would be applying animations or updating physics simulations.
        /// </summary>
        /// 
        /// <param name="dt">The amount of time since the last update (discrete timestep).</param>
        public abstract void Update(float dt);

        /// <summary>
        /// This method is called each frame, after update, when the scene needs
        /// to be displayed to the screen.
        /// </summary>
        /// 
        /// <param name="target">The RenderTarget to render to.</param>
        public abstract void Render(RenderTarget target);

        /// <summary>
        /// This method is called when the scene is about to stop being presented.
        /// Use this to stop background music, network connections, etc.
        /// </summary>
        public virtual void Exit(){}

        // Events

        /// <summary>
        /// This method is called when a key on the keyboard is released.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnKeyUp(KeyEventArgs e){}

        /// <summary>
        /// This method is called when a key on the keyboard is pressed.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnKeyDown(KeyEventArgs e){}

        /// <summary>
        /// This method is called when a mouse button is released.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseUp(MouseButtonEventArgs e){}

        /// <summary>
        /// This method is called when a mouse button is pressed.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseDown(MouseButtonEventArgs e){}

        /// <summary>
        /// This method is called when the mouse cursor is moved.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseMove(MouseMoveEventArgs e){}

        /// <summary>
        /// This method is called when the mouse wheel is moved.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseWheel(MouseWheelEventArgs e){}

        /// <summary>
        /// This method is called when the mouse enters the game window.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseEnter(){}

        /// <summary>
        /// This method is called when the mouse leaves the game window.
        /// </summary>
        /// 
        /// <param name="e">The corresponding SFML event.</param>
        public virtual void OnMouseLeave(){}
    }
}

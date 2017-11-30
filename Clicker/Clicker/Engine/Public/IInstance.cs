using System;

using SFML.System;

namespace Clicker.Engine.Public {
    /// <summary>
    /// An engine instance.
    /// </summary>
    /// 
    /// This interface allows the game to communicate with the runnign engine.
    /// The game can use an IInstance to control the engine.
    public interface IInstance {
        /// <summary>
        /// Closes the game window and terminates the engine. An example usage
        /// of this method would be a "quit" option in a game's main menu.
        /// </summary>
        void Quit();

        /// <summary>
        /// Instructs the engine to stop presenting the current scene, and instead
        /// present the given scene. This is used to transition from one scene
        /// to the next.
        /// </summary>
        /// 
        /// <param name="newScene">New scene.</param>
        // Switch to a different scene
        void SwitchToScene(Scene newScene);

        /// <summary>
        /// Gets the current size (in pixels) of the window the game is running in.
        /// </summary>
        /// 
        /// <value>The size of the window.</value>
        /// 
        // Retrieve the size of the current display
        Vector2u TargetSize { get; }

        /// <summary>
        /// Gets the currently running game.
        /// </summary>
        /// 
        /// <value>The currently running game.</value>
        Public.Game Game { get;  }
    }
}

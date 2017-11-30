using System;

namespace Clicker.Engine.Public {
    /// <summary>
    /// Abstract class implemented by games.
    /// </summary>
    /// 
    /// The game object is in charge of providing scenes and driving
    /// navigation through the multiple scenes.
    public abstract class Game {
        /// <summary>
        /// Gets or sets the engine instance associated with this game.
        /// </summary>
        /// 
        /// <value>The engine instance that is currently running the game .</value>
        public IInstance Instance { get; set; }

        /// <summary>
        /// Gets the name of the game. Currently this is used for the window title.
        /// </summary>
        /// <value>The game's name.</value>
        public abstract string Name { get;  }

        /// <summary>
        /// Gets the path to the logo of the game. Currently this is used in the
        /// loading screens.
        /// </summary>
        /// <value>The path to the game's logo.</value>
        public abstract string Logo { get;  }

        /// <summary>
        /// This method is called by the engine immediately after creating the
        /// game, and before the initial scene is created. This method runs in
        /// a separate thread and can be used to load resource files, pre-compute
        /// data or other time-consuming tasks. Use the provided IProgressReport
        /// object to report progress that will be displayed in the loading
        /// screen, so the user knows what is going on.
        /// </summary>
        /// 
        /// <param name="progressReport">The object through which you report progress.</param>
        public abstract void InitialLoad(IProgressReport progressReport);

        /// <summary>
        /// This method creates the very first scene that your game presents. This
        /// is typically a title screen or main menu.
        /// </summary>
        /// 
        /// <returns>The initial scene.</returns>
        public abstract Scene CreateInitialScene();

        /// <summary>
        /// This method is called when the engine is about to close, typically
        /// because the main window was closed. Use it to save configuration,
        /// savegames, stop background music etc.
        /// </summary>
        public abstract void Quit();
    }
}

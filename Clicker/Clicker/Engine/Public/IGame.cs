using System;

namespace Clicker.Engine.Public {
    public interface IGame {
        // The name of the game, displayed in the title bar (if any).
        string Name { get; }

        // The path to the logo of the game, displayed on the loading screen.
        string Logo { get;  }

        // Called when initially loading. Do initial loading here.
        void InitialLoad(IProgressReport progressReport);

        // Returns the scene that is displayed right after the loading screen ends.
        Scene CreateInitialScene();

        // TODO: OnClose (so we can save the score on exit)
    }
}

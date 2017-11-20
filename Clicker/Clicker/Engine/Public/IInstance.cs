using System;

using SFML.System;

namespace Clicker.Engine.Public {
    public interface IInstance {
        // Quit the game.
        void Quit();

        // Switch to a different scene
        void SwitchToScene(Scene newScene);

        // Retrieve the size of the current display
        Vector2u TargetSize { get; }
        Public.Game Game { get;  }
    }
}

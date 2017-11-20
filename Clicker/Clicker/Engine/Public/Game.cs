using System;

namespace Clicker.Engine.Public {
    public abstract class Game {
        public IInstance Instance { get; set; }

        public abstract string Name { get;  }
        public abstract string Logo { get;  }

        public abstract void InitialLoad(IProgressReport progressReport);
        public abstract Scene CreateInitialScene();
        public abstract void Quit();
    }
}

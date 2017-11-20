using System;
using System.Threading;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class ClickerGame : IGame {
        public ClickerGame() {
        }

        void IGame.InitialLoad(IProgressReport pr) {
        }

        void IGame.Quit(){
            
        }

        Scene IGame.CreateInitialScene() {
            return new MainMenuScene();
        }

        string IGame.Name {
            get {
                return "Cookie Clicker";
            }
        }

        string IGame.Logo {
            get {
                return "Assets/Logo.png";
            }
        }
    }
}

using System;
using System.Threading;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class ClickerGame : Engine.Public.Game {
        public ClickerGame() {
        }

        public override void InitialLoad(IProgressReport pr) {
        }

        public override void Quit(){
            
        }

        public override Scene CreateInitialScene() {
            return new MainMenuScene();
        }

        public override string Name {
            get {
                return "Cookie Clicker";
            }
        }

        public override string Logo {
            get {
                return "Assets/Logo.png";
            }
        }

        public void StartNewGame(){
            Instance.SwitchToScene(new GameplayScene());
        }

        public void LoadLastGame(){
            Instance.SwitchToScene(new GameplayScene(true));
        }
    }
}

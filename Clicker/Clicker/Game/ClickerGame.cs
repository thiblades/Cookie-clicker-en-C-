using System;
using System.Threading;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class ClickerGame : Engine.Public.Game {
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

        public override void InitialLoad(IProgressReport pr) {
        }

        public override void Quit(){
            
        }

        public override Scene CreateInitialScene() {
            return new MainMenuScene();
        }

        public void GoToMainMenu(){
            Instance.SwitchToScene(new MainMenuScene());
        }

        public void GoToError(string msg){
            Instance.SwitchToScene(new ErrorMenuScene(msg));
        }

        public void StartNewGame(){
            GameState state = new GameState();
            Instance.SwitchToScene(new GameplayScene(state));
        }

        public void LoadLastGame(){
            SaveGame saveGame = SaveGame.Read();

            if( saveGame != null ){
                GameState state = saveGame.ToState();
                Instance.SwitchToScene(new GameplayScene(state));
            } else {
                GoToError("Échec du Chargement");
            }
        }
    }
}

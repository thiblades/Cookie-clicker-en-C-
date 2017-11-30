using System;
using System.Threading;

using Clicker.Engine.Public;

namespace Clicker.Game {
    /// <summary>
    /// Party Clicker's Game implementation.
    /// </summary>
    public class ClickerGame : Engine.Public.Game {
        public override string Name {
            get {
                return "Party Clicker";
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

        public void GoToGame(GameState state) {
            Instance.SwitchToScene(new GameplayScene(state));
        }

        public void GoToPause(GameState state){
            Instance.SwitchToScene(new PauseMenuScene(state));
        }

        public void StartNewGame(){
            GoToGame(new GameState());
        }

        public void LoadLastGame(){
            SaveGame saveGame = SaveGame.Read();

            if( saveGame != null )
                GoToGame(saveGame.ToState());
            else
                GoToError("Échec du Chargement");
        }

    }
}

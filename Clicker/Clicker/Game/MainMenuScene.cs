using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;

namespace Clicker.Game {
    public class MainMenuScene : MenuScene {
        public MainMenuScene() {
            backgroundImage = "Assets/TitleBackground.png";
            title = "Menu Principal";

            titleColor = new Color(253, 191, 86);
            focusColor = new Color(253, 191, 86);
            neutralColor = Color.White;

            items = new Item[]{
                new Item("Nouvelle Partie", OnNewGame),
                new Item("Charger Partie", OnLoadGame),
                new Item("Quitter", OnQuit),
            };
        }

        private void OnNewGame(){
            ClickerGame game = (ClickerGame) Instance.Game;
            game.StartNewGame();
        }

        private void OnLoadGame(){
            ClickerGame game = (ClickerGame) Instance.Game;
            game.LoadLastGame();
        }

        private void OnQuit(){
            this.Instance.Quit();
        }
    }
}

using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;

namespace Clicker.Game {
    /// <summary>
    /// The main menu.
    /// </summary>
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
            GetGame().StartNewGame();
        }

        private void OnLoadGame(){
            GetGame().LoadLastGame();
        }

        private void OnQuit(){
            this.Instance.Quit();
        }

        private ClickerGame GetGame() {
            ClickerGame game = (ClickerGame) Instance.Game;
            return game;
        }
    }
}

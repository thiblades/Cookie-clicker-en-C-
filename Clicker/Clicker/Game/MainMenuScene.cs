using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;

namespace Clicker {
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
            Console.WriteLine("new game selected");
        }

        private void OnLoadGame(){
            Console.WriteLine("load game selected");
        }

        private void OnQuit(){
            this.Instance.Quit();
        }
    }
}

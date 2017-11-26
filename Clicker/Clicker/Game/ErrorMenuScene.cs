using System;

using SFML.Graphics;

using Clicker.Engine.Public;
using Clicker.GameKit;

namespace Clicker.Game {
    public class ErrorMenuScene: MenuScene {
        public ErrorMenuScene(string msg) {
            backgroundImage = "Assets/TitleBackground.png";
            title = msg;

            titleColor = Color.Red;
            focusColor = Color.White;
            neutralColor = Color.White;

            items = new Item[]{
                new Item("Retour", OnBack),
            };
        }

        private void OnBack(){
            GetGame().GoToMainMenu();
        }

        private ClickerGame GetGame() {
            ClickerGame game = (ClickerGame) Instance.Game;
            return game;
        }
    }
}

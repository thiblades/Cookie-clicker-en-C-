using System;

using SFML.Graphics;
using SFML.Audio;

using Clicker.Engine.Public;
using Clicker.GameKit;

namespace Clicker.Game {
    /// <summary>
    /// The pause menu.
    /// </summary>
    public class PauseMenuScene: MenuScene {
        private GameState state;

        private Sound saveConfirmSound;

        public PauseMenuScene(GameState gameState) {
            state = gameState;
            saveConfirmSound = new Sound(new SoundBuffer("Assets/SFX/Confirm.wav"));

            backgroundImage = "Assets/TitleBackground.png";
            title = "En Pause";

            titleColor = new Color(253, 191, 86);
            focusColor = new Color(253, 191, 86);
            neutralColor = Color.White;

            items = new Item[]{
                new Item("Reprendre", OnResume),
                new Item("Sauvegarder", OnSave),
                new Item("Menu Principal", OnMainMenu),
            };
        }

        private void OnResume(){
            GetGame().GoToGame(state);
        }

        private void OnSave(){
            SaveGame sg = new SaveGame(state);
            sg.Write();
            saveConfirmSound.Play();
        }

        private void OnMainMenu(){
            GetGame().GoToMainMenu();
        }

        private ClickerGame GetGame(){
            ClickerGame game = (ClickerGame) Instance.Game;
            return game;
        }
    }
}

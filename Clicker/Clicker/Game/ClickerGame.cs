using System;
using System.Threading;

using Clicker.Engine.Public;

namespace Clicker.Game {
    public class ClickerGame : IGame {
        public ClickerGame() {
        }

        void IGame.InitialLoad(IProgressReport pr) {
            pr.ReportProgress(0.00f, "Doing stuff");
            Thread.Sleep(1000);
            pr.ReportProgress(0.25f, "You picked the wrong house fool!");
            Thread.Sleep(1000);
            pr.ReportProgress(0.50f, "Extra dip I'll have extra dip");
            Thread.Sleep(1000);
            pr.ReportProgress(0.75f, "Fucker Big Smoke");
            Thread.Sleep(1000);
            pr.ReportProgress(1.00f, "REMEMBER THAT NAME");
        }

        Scene IGame.CreateInitialScene() {
            return new TestScene();
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

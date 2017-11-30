using System;

using System.Collections.Generic;

namespace Clicker.Game {
    public class GameState {
        // Event triggered every time the score changes
        public delegate void ScoreChange(ulong oldScore, ulong newScore);
        public event ScoreChange OnScoreChange;

        // The current score.
        private ulong score;

        public ulong Score {
            get {
                return score;
            }

            set {
                // Trigger the event, but don't fail if no handlers were registered.
                try {
                    OnScoreChange(score, value);
                } catch(NullReferenceException){}

                score = value;
            }
        }

        // List of registered bonus types.
        private List<Bonus> bonuses;

        public List<Bonus> Bonuses {
            get {
                return bonuses;
            }
        }

        /// <summary>
        /// Gets the amount of cookies contributed by bonus per second.
        /// </summary>
        /// <value>The score per second.</value>
        public float ScorePerSecond {
            get {
                float total = 0;

                foreach(Bonus curr in bonuses){
                    // Find how much cookies are contributed per period.
                    float cookiesPerPeriod = (float) curr.CookiesPerPeriod * curr.Count;

                    // But periods aren't necessarily one second, so use
                    // proportionnality to get a value per second.
                    total += cookiesPerPeriod / (float) curr.Period;
                }

                return total;
            }
        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        /// <param name="dt">The time since the last update, in seconds</param>
        public void Update(float dt){
            // Just update all the bonuses in turn.
            foreach( Bonus curr in bonuses )
                curr.Update(dt, this);
        }

        /// <summary>
        /// Called when the user clicks a cookie.
        /// </summary>
        public void Click(){
            Score += 1;
        }

        /// <summary>
        /// Called when the user buys a bonus.
        /// </summary>
        /// <param name="b">The bonus bought.</param>
        public void BuyBonus(Bonus b){
            ulong bonusCost = b.Cost;

            // Don't let the user buy a bonus they can't afford.
            if( Score < bonusCost )
                return;

            // If they can afford it, perform the transaction.
            Score -= bonusCost; 
            b.Buy();

            //OnBuyBonus(b);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Clicker.Game.GameState"/> class.
        /// </summary>
        public GameState(){
            // Initialize stats.
            Score = 0;
            bonuses = new List<Bonus>();

            // Register bonuses.
            AddBonus(name: "Bonus 1", initialCost: 15, perPeriod: 1, period: 5.0f);
            AddBonus(name: "Bonus 2", initialCost: 30, perPeriod: 1, period: 1.0f);
            AddBonus(name: "Bonus 3", initialCost: 50, perPeriod: 14, period: 1.0f);
            AddBonus(name: "OO0HMAN", initialCost: 1, perPeriod: 1000000, period: 1.0f);
        }

        /// <summary>
        /// Registers a new bonus type.
        /// </summary>
        /// <param name="name">Name of the bonus.</param>
        /// <param name="initialCost">Initial cost of the bonus.</param>
        /// <param name="perPeriod">Points contirbuted per period.</param>
        /// <param name="period">Period length in seconds.</param>
        private void AddBonus(string name, uint initialCost, uint perPeriod, float period) {
            Bonus bonus = new Bonus(name, "", initialCost, perPeriod, period);
            bonuses.Add(bonus);
        }
    }
}

using System;

using Clicker.GameKit;

namespace Clicker.Game {
    public class Bonus {
        private string name;
        private string image;
        private uint initialCost;
        private uint cookiesPerPeriod;
        private float period;
        private ulong count;

        private TimeAccumulator time;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Clicker.Game.Bonus"/> class.
        /// </summary>
        /// <param name="name">The displayed name of the bonus.</param>
        /// <param name="image">Path to the image for this bonus.</param>
        /// <param name="initialCost">The initial cost of one unit.</param>
        /// <param name="cookiesPerPeriod">How many cookies this bonus generates per period.</param>
        /// <param name="period">The length of a period, in seconds.</param>
        public Bonus(string name, string image, uint initialCost, uint cookiesPerPeriod, float period) {
            this.name = name;
            this.image = image;
            this.initialCost = initialCost;
            this.cookiesPerPeriod = cookiesPerPeriod;
            this.period = period;
            this.count = 0;
            this.time = new TimeAccumulator();
        }

        public Bonus(string name, uint initialCost, uint perPeriod, float period, ulong count, float time){
            this.name = name;
            this.image = "";
            this.initialCost = initialCost;
            this.cookiesPerPeriod = perPeriod;
            this.period = period;
            this.count = count;
            this.time = new TimeAccumulator();
            this.time.t = time;
        }

        /// <summary>
        /// Add one unit of this bonus. Note that this doesn't subtract the price
        /// of the bonus from the current score, or check that the player has
        /// enough money to do so.
        /// </summary>
        public void Buy() {
            this.count++;
        }

        /// <summary>
        /// Call this once per frame with the correct dt. If the period has
        /// elapsed, the score is updated.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="dt">The amount of time since the last update, in seconds.</param>
        /// <param name="state">The current game state.</param>
        public void Update(float dt, GameState state){
            // Don't bother updating the bonus if the player hasn't it.
            if( count > 0 ){
                // Add the time delta to our counter.
                time.Frame(dt);

                // Check if any periods have elapsed, and update the score.
                while( time.t > period ) {
                    state.Score += cookiesPerPeriod * count;
                    time.t -= period;
                }
            }
        }

        /// <summary>
        /// Gets the name of the bonus.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Gets the path to the image representing this bonus.
        /// </summary>
        /// <value>The image.</value>
        public string Image {
            get {
                return image;
            }
        }

        /// <summary>
        /// Gets the initial cost of one bonus.
        /// </summary>
        /// <value>The initial cost.</value>
        public uint InitialCost {
            get {
                return initialCost;
            }
        }

        /// <summary>
        /// Gets the amount of cookies gained per period.
        /// </summary>
        /// <value>The cookies per period.</value>
        public uint CookiesPerPeriod {
            get {
                return cookiesPerPeriod;
            }
        }

        /// <summary>
        /// Gets the period length, in seconds.
        /// </summary>
        /// <value>The period.</value>
        public float Period {
            get {
                return period;
            }
        }

        /// <summary>
        /// Gets the count of bonuses the player currently has.
        /// </summary>
        /// <value>The bonus count.</value>
        public ulong Count {
            get {
                return count;
            }
        }

        public ulong Cost {
            get {
                // This is the formula used by the actual Cookie Clicker
                float actualCost = initialCost * MathF.Pow(1.15f, count);

                // Because scores are integer values, round this up.
                return (ulong) MathF.Ceiling(actualCost);
            }
        }

        public float Time {
            get {
                return time.t;
            }
        }
    }
}

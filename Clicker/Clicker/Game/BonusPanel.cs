using System;
using System.Collections.Generic;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Clicker.GameKit;

namespace Clicker.Game {
    // For some reason, SFML's color constants are not declared const, which
    // means these can't be const... *sigh*
    public class BonusPanel {
        private class BonusCell {
            internal FloatRect bounds;
            internal RectangleShape frame;
            internal Text title;
            internal Text price;
            internal Text count;
            internal bool pushed = false;

            public void ChangeColor(Color c){
                frame.OutlineColor = c;
                title.Color = c;
                price.Color = c;
                //count.Color = c;
            }
        }

        public interface IDelegate {
            void BuyBonus(Bonus b);
        };

        // At which point the panel starts. 
        // Must coincide with "Assets/BonusPanel.png", that's why it's hardcoded.
        public const float PANEL_SCREEN_RATIO     = (1200.0f / 1920.0f);
        public const float PADDING                = 30;
        public const float INNER_PADDING          = 10;
        public const float BONUS_CELL_HEIGHT      = 120;

        public const uint TITLE_FONT_SIZE         = 60;
        public const uint BONUS_TITLE_FONT_SIZE   = 60;
        public const uint BONUS_PRICE_FONT_SIZE   = 20;

        // For some reason, SFML's color constants are not declared const, which
        // means these can't be const... *sigh*
        public Color TEXT_COLOR     = Color.Black;
        public Color CONTRAST_COLOR = Color.White;
        public Color DISABLED_COLOR = new Color(146, 146, 146, 146);

        private GameState state;

        private Font font;
        private FloatRect dimensions;
        private CenteredText title;
        private List<BonusCell> bonusCells;

        public BonusPanel(GameState s, Font f) {
            // Keep a reference to the parameters.
            state = s;
            font = f;

            // Create the title text.
            title = new CenteredText("Bonus", font, TITLE_FONT_SIZE);
            title.CenterX = true;
            title.Color = TEXT_COLOR;
            title.Style = Text.Styles.Underlined | Text.Styles.Bold;

            // Then create the bonus cells.
            CreateBonusCells();

            // Do an update so that the bonus texts contain their actual text
            // when we do the layout calculations.
            Update();
        }

        private void CreateBonusCells(){
            bonusCells = new List<BonusCell>();

            foreach(Bonus curr in state.Bonuses){
                BonusCell cell = new BonusCell();

                // Create the bounds rect (to be filled in during layout).
                cell.bounds = new FloatRect();

                // The rectangle forming the bonus button.
                cell.frame = new RectangleShape();
                cell.frame.FillColor = Color.Transparent;
                cell.frame.OutlineColor = TEXT_COLOR;
                cell.frame.OutlineThickness = 4;

                // The text containing the bonus's title.
                cell.title = new Text();
                cell.title.DisplayedString = "";
                cell.title.Font = font;
                cell.title.Color = TEXT_COLOR;
                cell.title.CharacterSize = BONUS_TITLE_FONT_SIZE;

                // The text containing the price of the bonus.
                cell.price = new Text();
                cell.price.DisplayedString = "";
                cell.price.Font = font;
                cell.price.Color = TEXT_COLOR;
                cell.price.CharacterSize = BONUS_PRICE_FONT_SIZE;

                // The text containing the amount of bonuses the user has.
                cell.count = new Text();
                cell.count.DisplayedString = "";
                cell.count.Font = font;
                cell.count.Color = DISABLED_COLOR;
                cell.count.CharacterSize = BONUS_TITLE_FONT_SIZE;

                bonusCells.Add(cell);
            }
        }

        public void Layout(Vector2u newSize){
            // Calculate the dimensions of the panel.
            dimensions = new FloatRect(
                newSize.X * PANEL_SCREEN_RATIO, 0,
                newSize.X * (1 - PANEL_SCREEN_RATIO), newSize.Y
            );

            // Place the panel.
            title.UpdateCentering(dimensions.Width, dimensions.Height);
            title.Position = new Vector2f(title.Position.X + dimensions.Left, 30);

            // Place each bonus frame.
            float currentY = PADDING + title.Dimensions.Y + PADDING;

            // Because we need to modify the members of the BonusCell's in this
            // loop, we can't use a foreach, even though doing that would have
            // no unintended consequences. Yay C#.
            for(int i = 0; i < bonusCells.Count; ++i){
                BonusCell curr = bonusCells[i];

                // Calculate the bounds.
                curr.bounds = new FloatRect(
                    /* left   */ dimensions.Left + PADDING,
                    /* top    */ currentY,
                    /* width  */ dimensions.Width - 2 * PADDING,
                    /* height */ BONUS_CELL_HEIGHT
                );

                // Lay out the frame that surrounds the bonus.
                curr.frame.Position = new Vector2f(curr.bounds.Left, curr.bounds.Top);
                curr.frame.Size = new Vector2f(curr.bounds.Width, curr.bounds.Height);

                // Lay out the title.
                curr.title.Position = new Vector2f(
                    curr.bounds.Left + INNER_PADDING,
                    curr.bounds.Top + INNER_PADDING
                );

                // Lay out the cost text.
                FloatRect priceBounds = curr.price.GetGlobalBounds();

                curr.price.Position = new Vector2f(
                    curr.bounds.Left + INNER_PADDING,
                    curr.bounds.Top + BONUS_CELL_HEIGHT - priceBounds.Height - 2 * INNER_PADDING
                );

                // Lay out the count text.
                FloatRect countBounds = curr.count.GetGlobalBounds();

                curr.count.Position = new Vector2f(
                    curr.bounds.Left + curr.bounds.Width - countBounds.Width - 2 * INNER_PADDING,
                    curr.title.Position.Y
                );

                currentY += BONUS_CELL_HEIGHT + PADDING;
            }
        }

        public void Update(){
            List<Bonus> bonuses = state.Bonuses;

            for( int i = 0; i < bonusCells.Count; ++i ) {
                Bonus currBonus = bonuses[i];
                BonusCell currCell = bonusCells[i];

                // Update the displayed bonus title.
                currCell.title.DisplayedString = currBonus.Name;

                // Update the displayed bonus price.
                string amountString = String.Format(
                    "{0} {1}", NumberFormatter.FormatShort(currBonus.CookiesPerPeriod),
                    currBonus.CookiesPerPeriod < 2 ? "verre" : "verres"
                );

                string periodString = currBonus.Period < 2 
                    ? "par seconde" 
                    : String.Format("toutes les {0} secondes",
                                    NumberFormatter.FormatShort(currBonus.Period));

                currCell.price.DisplayedString = String.Format(
                    "{0} verres (+{1} {2})",
                    NumberFormatter.FormatShort(currBonus.Cost), amountString, periodString
                );

                // Update the amount of bonuses the player has.
                currCell.count.DisplayedString = String.Format("{0}", currBonus.Count);

                // Gray out the bonuses that the player can't afford
                if( currBonus.Cost > state.Score )
                    currCell.ChangeColor(DISABLED_COLOR);
                else
                    currCell.ChangeColor(TEXT_COLOR);
            }
        }

        public void Render(RenderTarget rt){
            rt.Draw(title);

            foreach(BonusCell curr in bonusCells){
                rt.Draw(curr.frame);
                rt.Draw(curr.title);
                rt.Draw(curr.price);
                rt.Draw(curr.count);
            }
        }

        /// <summary>
        /// Determines if a given point is contained within the window.
        /// </summary>
        /// <returns><c>true</c>, if point is inside the bonus panel, <c>false</c> otherwise.</returns>
        /// <param name="X">The X coordinate.</param>
        /// <param name="Y">The Y coordinate.</param>
        public bool ContainsPoint(int X, int Y){
            return this.dimensions.Contains((float) X, (float) Y);
        }

        /// <summary>
        /// Find which bonus the given mous eposition is on, if any.
        /// </summary>
        /// <returns>The bonus index, or -1 for none.</returns>
        /// <param name="X">The X coordinate.</param>
        /// <param name="Y">The Y coordinate.</param>
        private int BonusForMousePosition(int X, int Y) {
            // Just check the coordinates against the bounds of each bonus.
            for( int i = 0; i < bonusCells.Count; ++i ) {
                if( bonusCells[i].bounds.Contains(X, Y) )
                    return i;
            }

            return -1;
        }

        public void OnMouseDown(Mouse.Button btn, int X, int Y){
            int clickedBonus = BonusForMousePosition(X, Y);

            // If the user clicked a bonus, highlight it to provide visual
            // feedback to the user.
            if( clickedBonus != -1 && btn == Mouse.Button.Left ) {
                // If the user can't afford that bonus, don't bother.
                if( state.Score < state.Bonuses[clickedBonus].Cost )
                    return;

                bonusCells[clickedBonus].ChangeColor(CONTRAST_COLOR);
                bonusCells[clickedBonus].pushed = true;
            }
        }

        public void OnMouseUp(Mouse.Button btn, int X, int Y){
            int clickedBonus = BonusForMousePosition(X, Y);

            for( int i = 0; i < bonusCells.Count; ++i ){
                BonusCell curr = bonusCells[i];

                // If the cell was pushed, and the mouse was released over it,
                // then engage the action (e.g. buy the corresponding bonus).
                if( i == clickedBonus && curr.pushed ){
                    state.BuyBonus(state.Bonuses[clickedBonus]);
                    curr.pushed = false;
                }

                // Reset all bonuses to the non-pushed state, so the user can
                // still change their mind before they release the button by
                // releasing it outside the bonus's box.
                bonusCells[i].ChangeColor(TEXT_COLOR);
            }
        }
    }
}

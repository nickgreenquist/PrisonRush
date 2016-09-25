using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GSDIII_game
{
    //Eric Kipnis
    public class Score
    {
        // Score attributes                
        private int totalScore;
        public int levelScore;

        private SpriteFont scoreFont;

        // Score constructor
        public Score()
        {
            totalScore = 0;
            levelScore = 0;
        }

        /// <summary>
        /// Gets and Sets the score font
        /// </summary>
        public SpriteFont ScoreFont
        {
            get { return scoreFont; }
            set { scoreFont = value; }
        }

        /// <summary>
        /// Returns the score object
        /// </summary>
        public Score GetScoreObject
        {
            get { return this; }
        }

        public int ScoreAmount
        {
            get { return totalScore; }

            set { totalScore = value; }
        }

        public int LevelScoreAmount
        {
            get { return levelScore; }

            set { levelScore = value; }
        }

        /// <summary>
        /// // Increase the score by one
        /// </summary>
        public void IncrementScore()
        {
            totalScore++;
        }

        public void IncrementLevelScore()
    {
        levelScore++;
    }

        /// <summary>
        /// // Change the score by a positve or negative amount
        /// </summary>
        /// <param name="amount">the integer amount the score is to be changed by</param>
        public void ChangeScore(int amount)
        {
            totalScore += amount;
        }


        /// <summary>
        /// Draws the score to the screen
        /// </summary>
        /// <param name="spritebatch">The spritebatch used to draw the score</param>
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(scoreFont, "Money: $" + (totalScore + levelScore), new Vector2(135, 50), Color.Black);
        }
    }
}

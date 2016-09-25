using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GSDIII_game
{
    //Tim Cotanch
    class CountDown
    {
        double timeLimit;
        SpriteFont timeFont;

        public double TimeLimit
        {
            get { return timeLimit; }
            set { timeLimit = value; }
        }

        public SpriteFont TimeFont
        {
            get { return timeFont; }
            set { timeFont = value; }
        }

        public CountDown(double timer)
        {
            timeLimit = timer;
        }

        public void Update(GameTime gameTime)
        {
            timeLimit -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch s)
        {
            s.DrawString(timeFont, "Self Destruct: " + (int)TimeLimit, new Vector2(235, 50), Color.Black);
        }
    }
}


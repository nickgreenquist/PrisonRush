using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GSDIII_game
{
    //Dylan Coats
    class ScrollingBackground
    {
        public Texture2D bg;
        public Rectangle rct;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, rct, Color.White);
        }

        public ScrollingBackground(Texture2D txt, Rectangle rect)
        {
            bg = txt;
            rct = rect;
        }

        public void Update(int amount)
        {
            rct.X -= amount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GSDIII_game
{
    //Dylan Coats
    class Background
    {
        public ScrollingBackground fore;
        public ScrollingBackground mid;
        public ScrollingBackground back;
        public ScrollingBackground fore2;
        public ScrollingBackground mid2;
        public ScrollingBackground back2;

        public int currentPos;
        public int prevPos;
        public int changePos;

        public void ProcessInput(Level l)
        {
            //Scrolling Logic
            currentPos = l.tiles[0, 0].X;
            changePos = currentPos - prevPos;

            if (prevPos == 0)
            {
                back.Update(0);
                mid.Update(0);
                fore.Update(0);
                back2.Update(0);
                mid2.Update(0);
                fore2.Update(0);
            }
            else
            {
                back.Update((int)(-0.25 * changePos));
                mid.Update((int)(-.5 * changePos));
                fore.Update((int)(-1 * changePos));
                back2.Update((int)(-0.25 * changePos));
                mid2.Update((int)(-.5 * changePos));
                fore2.Update((int)(-1 * changePos));
            }
            //Right
            if (back.rct.X < -1200)
            {
                back.rct.X = 1185;
            }
            if (mid.rct.X < -1200)
            {
                mid.rct.X = 1185;
            }
            if (fore.rct.X < -1200)
            {
                fore.rct.X = 1185;
            }
            if (back2.rct.X < -1200)
            {
                back2.rct.X = 1185;
            }
            if (mid2.rct.X < -1200)
            {
                mid2.rct.X = 1185;
            }
            if (fore2.rct.X < -1200)
            {
                fore2.rct.X = 1185;
            }
            //Left
            if (back.rct.X > 1200)
            {
                back.rct.X = -1185;
            }
            if (mid.rct.X > 1200)
            {
                mid.rct.X = -1185;
            }
            if (fore.rct.X > 1200)
            {
                fore.rct.X = -1185;
            }
            if (back2.rct.X > 1200)
            {
                back2.rct.X = -1185;
            }
            if (mid2.rct.X > 1200)
            {
                mid2.rct.X = -1185;
            }
            if (fore2.rct.X > 1200)
            {
                fore2.rct.X = -1185;
            }

            prevPos = currentPos;
        }

        public void Draw(SpriteBatch s)
        {
            back.Draw(s);
            back2.Draw(s);
            mid.Draw(s);
            mid2.Draw(s);
            fore.Draw(s);
            fore2.Draw(s);
        }

        public void NextLevel(ContentManager Content)
        {
            fore.rct = new Rectangle(0, 0, 1200, 800);
            mid.rct = new Rectangle(0, 0, 1200, 800);
            back.rct = new Rectangle(0, 0, 1200, 800);
            fore2.rct = new Rectangle(fore.rct.X + 1200, 0, 1200, 800);
            mid2.rct = new Rectangle(mid.rct.X + 1200, 0, 1200, 800);
            back2.rct = new Rectangle(back.rct.X + 1200, 0, 1200, 800);
        }


    }
}


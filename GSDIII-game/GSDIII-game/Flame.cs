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
    //Tim Cotanch
    public class Flame
    {
        Texture2D texture;
        public double flameTime;
        public Rectangle flameRect;
        bool active;
        int flameSpeed;

        public Flame(bool active, Texture2D texture, int x, int y, int speed)
        {
            this.texture = texture;
            this.active = active;
            flameRect = new Rectangle(x, y - 80, 80, 80);
            flameSpeed = speed;
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Rectangle FlameRect
        {
            get { return flameRect; }
            set { flameRect = value; }
        }

        public void MoveFlame(GameTime gameTime)
        {
            flameTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (flameTime <= 1)
            {
                flameRect.Y -= flameSpeed;
            }
            else if (flameTime > 1)
            {
                flameRect.Y += flameSpeed;
                if (flameTime >= 2)
                {
                    flameTime = 0;
                }
            }
            
        }

        public void Draw(SpriteBatch s)
        {

            if (flameTime <= 1)
            {
                s.Draw(texture, new Vector2(flameRect.X, flameRect.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
                
            }
            if (flameTime >= 1)
            {
                s.Draw(texture, flameRect, Color.White);
            }
        }

    }
}

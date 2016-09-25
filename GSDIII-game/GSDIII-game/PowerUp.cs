using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GSDIII_game
{
    class PowerUp
    {
        public Rectangle pos;
        public Texture2D powerTexture;

        public PowerUp()
        {
            pos = new Rectangle(300, 400, 50, 122);
        }

        public void Update(int amount)
        {
            pos.X += amount;
        }
    }
}

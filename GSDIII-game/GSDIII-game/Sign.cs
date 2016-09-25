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
    // Created By Eric Kipnis
    public class Sign
    {
        public Rectangle signPosition;
        private Texture2D signTexture;
        private int signIndex;

        public Sign(Texture2D sprite, int x, int y, int width, int height, int index)
        {
            signPosition = new Rectangle(x, y, width, height);
            signTexture = sprite;
            signIndex = index;
        }

        public int XPos
        {
            get { return signPosition.X; }

            set { signPosition.X = value; }
        }

        public Rectangle SignPosition
        {
            get { return signPosition; }
        }

        public int SignIndex
        {
            get { return signIndex; }

            set { signIndex = value; }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(signTexture, signPosition, Color.White);
        }
    }
}

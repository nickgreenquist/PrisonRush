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
    public class Particle
    {
        public Rectangle rect;
        public Color color;
        public int xSpeed;
        public int ySpeed;
        public Texture2D texture;

        public Particle(Texture2D tex, int x, int y, int xs, int ys, Color c)
        {
            texture = tex;
            rect.X = x;
            rect.Y = y;
            xSpeed = xs;
            ySpeed = ys;
            color = c;
            rect.Width = 88;
            rect.Height = 88;
        }
    }
}

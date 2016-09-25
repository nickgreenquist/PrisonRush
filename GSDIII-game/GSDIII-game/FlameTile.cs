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
    public class FlameTile : Tile
    {
        Texture2D texture;
        public Flame flames;
        double flameTime;
        ContentManager content;

        public FlameTile(ContentManager content, Texture2D texture, int x, int y, TileType t, String name)
            : base(texture, x, y, t, name)
        {
            flames = new Flame(false, content.Load<Texture2D>("Fire"), this.X, this.Y, 5);
            this.content = content;
            this.texture = texture;
        }

        public void Draw(SpriteBatch s)
        {
            if (flames.Active)
            {
                flames.Draw(s);
            }
            s.Draw(texture, TileRect, Color.White);  
        }

        public void Update(GameTime gameTime)
        {
            
            if (flames.flameRect.Y > (this.Y - 80))
            {
                flames.flameRect.Y = this.Y - 80;
                flames.flameTime = 0;
                flames.Active = false;
                flameTime = 0;

            }
            shootFlame(gameTime);
            if (flames.Active)
            {
                flames.MoveFlame(gameTime);
            }
            
        }

        public void shootFlame(GameTime gameTime)
        {

            flameTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (flameTime > 2)
            {
                flames.Active = true;

                if (flameTime > 4)
                {
                    flames.Active = false;
                    flameTime = 0;
                }
            }


        }


    }
}
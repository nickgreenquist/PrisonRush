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
    //Nick Greenquist
    public class Bullet
    {
        Texture2D texture;
        public Rectangle bulletRect;
        int bulletSpeed;
        Vector2 movement;
        public Vector2 bulletPosition; //for player only
        public bool shootLeft;
        int scale;

        public Bullet(Texture2D tex, int  x, int y,int speed)
        {
            texture = tex;
            bulletRect = new Rectangle(x, y, 20, 10);
            bulletSpeed = speed;
            scale = 2;
        }

        public Bullet(Texture2D tex, int x, int y, int speed,Vector2 cursorPosition) //overrriden constructor for bullets that are shot in any direction
        {
            texture = tex;
            bulletRect = new Rectangle(x, y, 20, 15);
            bulletPosition.X = bulletRect.X;
            bulletPosition.Y = bulletRect.Y;
            bulletSpeed = speed;
            scale = 2;

            movement = cursorPosition - bulletPosition;
            if (movement != Vector2.Zero)
                movement.Normalize();
        }

        public Bullet(Texture2D tex, int x, int y, int speed, Vector2 cursorPosition, string ammoType, bool left) //overrriden constructor for bullets that are shot in any direction, with ammo name
        {
            scale = 2;
            texture = tex;
            if (ammoType.Equals("rpg")) { bulletRect = new Rectangle(x, y, 40, 30); }
            else if (ammoType.Equals("bullet")) { bulletRect = new Rectangle(x, y, 20, 15); }
            bulletPosition.X = bulletRect.X;
            bulletPosition.Y = bulletRect.Y;
            bulletSpeed = speed;
            if (ammoType.Equals("rpg"))
            {
                bulletSpeed -= 3;
                scale = 1;
            }

            movement = cursorPosition - bulletPosition;
            if (movement != Vector2.Zero)
                movement.Normalize();

            shootLeft = left;
            
        }



        public void MoveBullet()
        {
            bulletRect.X += bulletSpeed;
        }

        public void MoveBullet(GameTime gameTime) //overriden method for bullets that are shot at any direction
        {
            bulletPosition += movement * (float)((bulletSpeed * 60) * gameTime.ElapsedGameTime.TotalSeconds);
            bulletRect.X = (int)bulletPosition.X;
            bulletRect.Y = (int)bulletPosition.Y;
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(texture, bulletRect, Color.White);
        }
        public void PlayerDraw(SpriteBatch s) //this is bullets that can go anywhere
        {
            bulletRect.X = (int)bulletPosition.X;
            bulletRect.Y = (int)bulletPosition.Y;

            if (shootLeft)
            {
                s.Draw(texture, new Vector2(bulletRect.X, bulletRect.Y), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                s.Draw(texture, new Vector2(bulletRect.X, bulletRect.Y), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            }
        }
    }
}

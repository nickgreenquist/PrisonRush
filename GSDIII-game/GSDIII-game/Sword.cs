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
    //Dylan Coats
    public class Sword
    {
        bool active;
        int damage;
        public Texture2D swordTxtr;
        public Rectangle swordSpriteSheet;
        public Rectangle swordPos;
        const int WALK_FRAME_COUNT = 2;
        public int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        public Texture2D SwordTexture
        {
            get { return swordTxtr; }
            set { swordTxtr = value; }
        }

        public Sword(Texture2D sprite,bool pActive, int pDamage)
        {
            active = pActive;
            damage = pDamage;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            swordTxtr = sprite;
        }

        //Nick Greenquist and Dylan Coats
        public void Draw(SpriteBatch s, Player p)
        {
            
            swordSpriteSheet = new Rectangle(frame * 116, 0, 120, 120);
            if (p.playerState == PlayerState.faceRight || p.playerState == PlayerState.runRight)
            {
                swordPos = new Rectangle(p.X + 45, p.Y - 7, 120, 120);
                s.Draw(swordTxtr, new Vector2(p.X + 45, p.Y - 7), swordSpriteSheet, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.playerState == PlayerState.faceLeft || p.playerState == PlayerState.runLeft)
            {
                swordPos = new Rectangle(p.X - 75, p.Y - 7, 120, 120);
                s.Draw(swordTxtr, new Vector2(p.X - 75, p.Y - 7), swordSpriteSheet, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {

            }
        }
        public void Update(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;						// Adjust the frame
                if (frame > WALK_FRAME_COUNT)	// Check the bounds
                    frame = 0;					// Back to 1 (since 0 is the "standing" frame)
                timeCounter -= timePerFrame;	// Remove the time we "used"
            }
        }
    }
}

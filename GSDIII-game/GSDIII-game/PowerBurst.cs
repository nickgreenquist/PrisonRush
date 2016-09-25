using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GSDIII_game
{
    //Dylan Coats
    public class PowerBurst
    {
        bool active;
        int damage;
        Texture2D BurstTxtr;
        Rectangle BurstSpriteSheet;
        const int WALK_FRAME_COUNT = 2;
        public int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        public Texture2D BurstTexture
        {
            get { return BurstTxtr; }
            set { BurstTxtr = value; }
        }

        public PowerBurst(bool pActive, int pDamage)
        {
            active = pActive;
            damage = pDamage;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
        }

        public void Draw(SpriteBatch s, Player p)
        {
            BurstSpriteSheet = new Rectangle(frame * 150, 0, 150, 150);
            //s.Draw(swordTxtr, p.PlayerPos, Color.White);
            if (p.playerState == PlayerState.faceRight || p.playerState == PlayerState.runRight)
            {
                s.Draw(BurstTxtr, new Vector2(p.X - 18, p.Y - 30), BurstSpriteSheet, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.playerState == PlayerState.faceLeft || p.playerState == PlayerState.runLeft)
            {
                s.Draw(BurstTxtr, new Vector2(p.X - 45, p.Y - 30), BurstSpriteSheet, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {

            }
        }
        public void Draw(SpriteBatch s, FinalBoss b)
        {
            BurstSpriteSheet = new Rectangle(frame * 150, 0, 150, 150);
            s.Draw(BurstTxtr, new Vector2(b.EnemyPos.X - 70, b.EnemyPos.Y - 60), BurstSpriteSheet, Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
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

        public void CheckCollisions()
        {

        }
    }
}

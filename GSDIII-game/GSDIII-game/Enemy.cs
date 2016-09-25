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
    public enum EnemyState
    {
        faceLeft,
        faceRight,
        runLeft,
        runRight
    }

    //Nick Greenquist and Tim Cotanch
    public abstract class Enemy
    {

        //Attributes for enemy
        Texture2D enemySprite;
        Rectangle enemyPos;
        Rectangle enemySpriteSheet;
        public EnemyState enemyState;

        //Constant attributes for the enemy
        const int WALK_FRAME_COUNT = 2;
        public int ENEMY_RECT_Y_OFFSET = 90;
        public int ENEMY_RECT_HEIGHT = 88;
        public int ENEMY_RECT_WIDTH = 88;

        //these are for animating the enemy spritesheet
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        private Texture2D sprite;
        private int w;
        private int h;
        public bool isDead;
        public int deathAnimationTime;
        public Color color;
        public double shootSpeed;
        public double health;

        //Properties for the enemy 
        public Texture2D EnemySprite
        {
            get { return enemySprite; }
            set { enemySprite = value; }
        }
        public Rectangle EnemyPos
        {
            get { return enemyPos; }
            set { enemyPos = value; }
        }
        public int X
        {
            get { return enemyPos.X; }
            set { enemyPos.X = value; }
        }
        public int Y
        {
            get { return enemyPos.Y; }
            set { enemyPos.Y = value; }
        }
        

        //Construcotr
        public Enemy(Texture2D sprite, int x, int y, int w, int h,EnemyState state)
        {
            enemyPos = new Rectangle(x, y, w, h);
            enemySpriteSheet = new Rectangle(ENEMY_RECT_WIDTH, ENEMY_RECT_Y_OFFSET, ENEMY_RECT_WIDTH, ENEMY_RECT_HEIGHT);
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            enemyState = state;
            enemySprite = sprite;
            isDead = false;
            deathAnimationTime = 0;
            color = Color.White;
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
                    frame = 1;					// Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;	// Remove the time we "used"
            }

            if (isDead && deathAnimationTime < 20)
            {
                deathAnimationTime++;
            }
        }

        //draws the player sprite
        public void Draw(SpriteBatch s)
        {
            int tempWidth = ENEMY_RECT_WIDTH;
            int tempHeight = ENEMY_RECT_HEIGHT;
            //draw enemy depending on his state
            switch (enemyState)
            {
                case EnemyState.faceRight:
                    s.Draw(enemySprite, enemyPos, color);
                    break;
                case EnemyState.faceLeft:
                    s.Draw(enemySprite, new Vector2(X, Y), null, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                case EnemyState.runRight:
                    enemySpriteSheet = new Rectangle(frame * ENEMY_RECT_WIDTH, ENEMY_RECT_Y_OFFSET, ENEMY_RECT_WIDTH, ENEMY_RECT_HEIGHT);
                    s.Draw(enemySprite, new Vector2(X, Y), enemySpriteSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                case EnemyState.runLeft:
                    enemySpriteSheet = new Rectangle(frame * ENEMY_RECT_WIDTH, ENEMY_RECT_Y_OFFSET, ENEMY_RECT_WIDTH, ENEMY_RECT_HEIGHT);
                    s.Draw(enemySprite, new Vector2(X, Y), enemySpriteSheet, color);
                    break;
                default:
                    break;
            }
        }
    }
}

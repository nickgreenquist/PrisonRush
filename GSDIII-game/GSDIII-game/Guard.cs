using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
using System.IO;

namespace GSDIII_game
{
    //Nick Greenquist
    public class Guard : Enemy
    {
        public int walkDistance;
        public int walked;
        public List<Bullet> bullets;
        public ContentManager content;
        public double timeCounter;
        public double fps;
        public double timePerFrame;
        public string superGuard;
        public bool bonusRoom;


        public int WalkDistance
        {
            get { return walkDistance; }
            set { walkDistance = value; }
        }

        //Construcotr
        public Guard(Texture2D sprite, int x, int y, int w, int h, int walkD, ContentManager content)
            : base(sprite, x, y, w, h, EnemyState.runLeft)
        {
            walkDistance = walkD;
            walked = 0;
            bullets = new List<Bullet>();
            this.content = content;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            health = 9;
            ReadGuard();
            superGuard = "";
            bonusRoom = false;
        }

        //another construcor for SuperGuards
        public Guard(Texture2D sprite, int x, int y, int w, int h, int walkD, ContentManager content, string super)
            : base(sprite, x, y, w, h, EnemyState.runLeft)
        {
            walkDistance = walkD;
            walked = 0;
            bullets = new List<Bullet>();
            this.content = content;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            health = 30;
            ReadGuard();
            shootSpeed = .3;
            superGuard = super;
            bonusRoom = false;
        }

        //Nick Greenquist
        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            int walkSpeed = 1;
            if (superGuard != null)
            {
                walkSpeed = 2;
            }
            if (!isDead)
            {
                base.Update(gameTime);

                if (walked < walkDistance)
                {
                    walked += walkSpeed;
                    if (enemyState == EnemyState.runLeft)
                    {
                        X -= walkSpeed;
                    }
                    if (enemyState == EnemyState.runRight)
                    {
                        X += walkSpeed;
                    }
                }
                else
                {
                    walked = 0;
                    if (enemyState == EnemyState.runLeft)
                    {
                        enemyState = EnemyState.runRight;
                    }
                    else if (enemyState == EnemyState.runRight)
                    {
                        enemyState = EnemyState.runLeft;
                    }
                }
                if (!bonusRoom)
                {
                    FireBullets(gameTime, playerPos);
                }
            }
            //move bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet(gameTime);
            }
            if (isDead && deathAnimationTime < 20)
            {
                deathAnimationTime++;
            }
        }

        //Nick Greenquist
        public void Draw(SpriteBatch s)
        {
            if (superGuard != null)
            {
                if (superGuard.Equals("superguard"))
                {
                    ENEMY_RECT_WIDTH = 100;
                    ENEMY_RECT_HEIGHT = 110;
                    ENEMY_RECT_Y_OFFSET = 110;
                }
            }
            if (!isDead)
            {
                base.Draw(s);
            }
            if (isDead && deathAnimationTime < 19)
            {
                color = Color.Red;
                base.Draw(s);
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].PlayerDraw(s);
            }

        }

        //Nick Greenquist
        public void FireBullets(GameTime gameTime, Vector2 playerPos)
        {
            if (!isDead)
            {
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCounter >= shootSpeed)
                {
                    timeCounter = 0;
                    if (enemyState == EnemyState.runRight || enemyState == EnemyState.faceRight)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 10, playerPos, "bullet",false));
                    }
                    if (enemyState == EnemyState.runLeft || enemyState == EnemyState.faceLeft)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 10, playerPos, "bullet",false));
                    }
                }
            }

        }

        //Nick Greenquist - read player attributes from playerParam.txt
        public void ReadGuard()
        {
            //read in data froma file and set player variables
            StreamReader reader = new StreamReader("guardParam.txt");
            string line = "";

            line = reader.ReadLine();
            shootSpeed = Double.Parse(line);
            reader.Close();
        }
    }
}

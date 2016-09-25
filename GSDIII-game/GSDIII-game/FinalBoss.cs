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
    public class FinalBoss : Enemy
    {
        public Texture2D bossSprite;
        public Texture2D bossBoom;
        public List<Bullet> bullets;
        public ContentManager content;
        double timeCounter;
        double fps;
        double timePerFrame;
        int randomTimer;
        Random rnd;
        double animationTime;
        int frame;
        Rectangle bossSheet;
        int shootPos;
        int shootPos2;
        int randomAttack;
        int flash;
        Vector2 shootTowards;
        int shootOffset;
        public PowerBurst shield;


        public FinalBoss(Texture2D sprite, int x, int y, int w, int h, ContentManager content)
            : base(sprite, x, y, w, h, EnemyState.runLeft)
        {
            bullets = new List<Bullet>();
            this.content = content;
            fps = 10.0;
            timePerFrame = .2;
            health = 250;
            shootSpeed = 1;
            randomTimer = 0;
            rnd = new Random();
            bossSprite = sprite;
            isDead = false;
            frame = 0;
            animationTime = 0;
            shootPos = 100;
            randomAttack = 0;
            shootTowards = new Vector2(0, 0);
            shootOffset = 1;
            shield = new PowerBurst(true, 5);
            shield.BurstTexture = content.Load<Texture2D>("Burst");
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            shootTowards.Y = EnemyPos.Y;
            shootTowards.X = EnemyPos.X;

            if (!isDead)
            {
                animationTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTime >= timePerFrame)
                {
                    frame += 1;						// Adjust the frame
                    if (frame > 2)	// Check the bounds
                        frame = 0;					// Back to 1 (since 0 is the "standing" frame)

                    animationTime -= timePerFrame;	// Remove the time we "used"
                }
                FireBullets(gameTime, playerPos);
            }
            //move bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet(gameTime);
            }
            if (isDead && deathAnimationTime < 1000)
            {
                deathAnimationTime++;
                animationTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTime >= timePerFrame)
                {
                    frame += 1;
                    if (frame > 2)	// Check the bounds
                        frame = 0;
                    // Back to 1 (since 0 is the "standing" frame)

                    animationTime -= timePerFrame;	// Remove the time we "used"
                }
            }
            randomTimer++;
        }

        //Nick Greenquist
        public void DrawBoss(SpriteBatch s)
        {
            if (!isDead)
            {
                bossSheet = new Rectangle(frame * EnemyPos.Width, 0, EnemyPos.Width, EnemyPos.Height);
                s.Draw(bossSprite, new Vector2(X, Y), bossSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            }
            if (isDead && deathAnimationTime < 499)
            {
                if (deathAnimationTime < 250)
                {
                    timePerFrame = .2;
                }
                if (deathAnimationTime < 400)
                {
                    timePerFrame = .1;
                }
                if (deathAnimationTime < 999)
                {
                    timePerFrame = .01;
                }
                color = Color.Red;
                bossSheet = new Rectangle(frame * EnemyPos.Width, 0, EnemyPos.Width, EnemyPos.Height);
                s.Draw(bossSprite, new Vector2(X, Y), bossSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].PlayerDraw(s);
            }

        }

        //Nick Greenquist
        public void FireBullets(GameTime gameTime, Vector2 playerPos)
        {

                randomAttack = rnd.Next(0, 100);

                if (randomAttack == 42)
                {

                }
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;


                if (randomTimer < 500) //shoot towards the player slowly
                {
                    shootSpeed = 1;
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 10, playerPos,"bullet",false));

                        timeCounter = 0;

                    }

                }
                if (randomTimer > 500 && randomTimer < 600) //extreme burst
                {
                    if (flash >= 0 && flash < 11)
                    {
                        color = Color.BlanchedAlmond;
                        flash++;
                    }
                    else if (flash > 10 && flash < 20)
                    {
                        color = Color.LightBlue;
                        flash++;
                    }
                    else { flash = 0; }
                    if (timeCounter >= shootSpeed)
                    {
                        shootSpeed = .02;
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet",false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(rnd.Next(0, 1200), rnd.Next(200, 800)), "bullet", false));
                        timeCounter = 0;
                    }
                    
                }
                if (randomTimer > 600 && randomTimer < 800) //spinning shoots
                {
                    
                    if (randomTimer > 600 && randomTimer < 605) 
                    { 
                        shootPos = 300;
                        shootPos2 = 500;
                    }
                    color = Color.White;
                    shootSpeed = .02;
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2((EnemyPos.X + 300) + shootPos2, shootPos), "bullet", false));
                        timeCounter = 0;
                        shootPos = shootPos + (50 * shootOffset);;
                        if (shootPos > 1000)
                        {
                            shootOffset *= -1;
                            shootPos2 = -1000;
                        }
                        if (shootPos < 300)
                        {
                            shootOffset *= -1;
                            shootPos2 = 700;
                        }
                    }
                }
                if (randomTimer > 800 && randomTimer < 1000)
                {
                    shootSpeed = .25;
                    if (randomTimer > 800 && randomTimer < 805) { shootPos = 350; }
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos), "bullet",false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 50), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 100), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 150), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 200), "bullet", false));
                        timeCounter = 0;
                    }
                }
                if (randomTimer > 1000 && randomTimer < 1200)
                {
                    if (randomTimer > 1000 && randomTimer < 1005) { shootPos = 350; }
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 50), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 100), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 150), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 200), "bullet", false));
                        timeCounter = 0;
                    }
                }
                if (randomTimer > 1200 && randomTimer < 1400)
                {
                    shootSpeed = .5;
                    if (randomTimer > 1200 && randomTimer < 1205) { shootPos = 350; }
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 50), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 100), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 150), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 300, shootPos + 200), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 50), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 100), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 150), "bullet", false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 600, shootPos + 200), "bullet", false));
                        timeCounter = 0;
                    }
                }
                if (randomTimer > 1400 && randomTimer < 1800) //spinning shoots
                {
                    if (randomTimer > 1400 && randomTimer <1405) { shootPos = 350; }
                    color = Color.White;
                    shootSpeed = .05;
                    if (timeCounter >= shootSpeed)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X + 1000, shootPos), "bullet",false));
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 15, new Vector2(EnemyPos.X - 1000, shootPos), "bullet", false));
                        timeCounter = 0;
                        shootPos = shootPos + (75 * shootOffset);
                        if (shootPos > 1000)
                        {
                            shootOffset *= -1;
                        }
                        if (shootPos < 320)
                        {
                            shootOffset *= -1;
                        }
                    }
                }
                if (randomTimer > 1800) { randomTimer = 0; }
            }
   }
}

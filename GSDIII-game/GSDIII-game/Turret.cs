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
    public class Turret : Enemy
    {
        public List<Bullet> bullets;
        ContentManager content;
        double timeCounter;
        double fps;
        double timePerFrame;

         //Construcotr
        public Turret(Texture2D sprite, int x, int y, int w, int h,EnemyState state,ContentManager content) : base(sprite,x,y,w,h,state)
        {
            
            bullets = new List<Bullet>();
            this.content = content;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            health = 11;
            ReadTurret();
        }

        public void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                base.Update(gameTime);
                FireBullets(gameTime);               
            }
            //move bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet();
            }
            if (isDead && deathAnimationTime < 20)
            {
                deathAnimationTime++;
            }
        }

        //draws the player sprite
        public void Draw(SpriteBatch s,List<Particle> pars)
        {
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
                bullets[i].Draw(s);
            }
        }

        //Nick Greenquist
        public void FireBullets(GameTime gameTime)
        {
            if (!isDead)
            {
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCounter >= shootSpeed) //how fast he shoots shit
                {
                    timeCounter = 0;
                    if (enemyState == EnemyState.faceRight)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet2"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), 10));
                    }
                    if (enemyState == EnemyState.faceLeft)
                    {
                        bullets.Add(new Bullet(content.Load<Texture2D>("Bullet2"), EnemyPos.X + (EnemyPos.Width / 2), EnemyPos.Y + (EnemyPos.Height / 2), -10));
                    }
                }
            }
        }
        //Nick Greenquist - read player attributes from playerParam.txt
        public void ReadTurret()
        {
            //read in data froma file and set player variables
            StreamReader reader = new StreamReader("turretParam.txt");
            string line = "";

            line = reader.ReadLine();
            shootSpeed = Double.Parse(line);

            reader.Close();
        }
    }
}

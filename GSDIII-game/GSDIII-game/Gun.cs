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
    //Nick Greenquist
    public class Gun
    {
        public Texture2D gunSprite;
        public Texture2D ammoSprite;
        public Rectangle gunPos;
        public Vector2 gunRotationCoor;
        public int gunDamage;
        public float rotationAngleOrig;
        float rotationAngle;
        public Color gunColor;
        public string gunName;
        public string ammoType;
        public int bulletsPerShot;
        public double shootSpeed;

        public List<Rectangle> bulletCollisions;
        public List<double> bulletCollisionsTimer;

        public Gun(ContentManager content, string name, Player p)
        {
            gunColor = Color.White;
            gunName = name;
            bulletCollisions = new List<Rectangle>();
            bulletCollisionsTimer = new List<double>();

            if (gunName.Equals("pistol"))
            {
                gunSprite = content.Load<Texture2D>("pistol");
                ammoSprite = content.Load<Texture2D>("bullet");
                ammoType = "bullet";
                shootSpeed = p.pistolSpeed;
                bulletsPerShot = p.pistolSpread;
                gunDamage = 1;
            }
            if (gunName.Equals("shotgun"))
            {
                gunSprite = content.Load<Texture2D>("gun");
                ammoSprite = content.Load<Texture2D>("bullet");
                ammoType = "bullet";
                shootSpeed = p.shotgunSpeed;
                bulletsPerShot = p.shotgunSpread;
                gunDamage = 1;
            }
            if (gunName.Equals("bazooka"))
            {
                gunSprite = content.Load<Texture2D>("Bazooka");
                ammoSprite = content.Load<Texture2D>("Rpg bullet");
                ammoType = "rpg";
                shootSpeed = p.bazookaSpeed;
                bulletsPerShot = p.bazookaSpread;
                gunDamage = 10;
            }
            if (gunName.Equals("machinegun"))
            {
                gunSprite = content.Load<Texture2D>("Machine Gun");
                ammoSprite = content.Load<Texture2D>("bullet");
                ammoType = "bullet";
                shootSpeed = p.machinegunSpeed;
                bulletsPerShot = p.machinegunSpread;
                gunDamage = 1;
            }
        }

        public void Update(Player p, Vector2 mousePosition)
        {
            if (p.playerState == PlayerState.faceLeft || p.playerState == PlayerState.runLeft)
            {
                if (gunName.Equals("shotgun"))
                {
                    gunPos = new Rectangle(p.X + 35, p.Y + 70, 60, 60);
                    gunRotationCoor = new Vector2(70, 30);
                    gunDamage = p.shotgunDamage;
                    shootSpeed = p.shotgunSpeed;
                    bulletsPerShot = p.shotgunSpread;
                }
                if (gunName.Equals("pistol"))
                {
                    gunPos = new Rectangle(p.X + 40, p.Y + 70, 60, 60);
                    gunRotationCoor = new Vector2(45, 30);
                    gunDamage = p.pistolDamage;
                    shootSpeed = p.pistolSpeed;
                    bulletsPerShot = p.pistolSpread;
                }
                if (gunName.Equals("bazooka"))
                {
                    gunPos = new Rectangle(p.X + 35, p.Y + 70, 60, 60);
                    gunRotationCoor = new Vector2(40, 30);
                    gunDamage = p.bazookaDamage;
                    shootSpeed = p.bazookaSpeed;
                    bulletsPerShot = p.bazookaSpread;
                }
                if (gunName.Equals("machinegun"))
                {
                    gunPos = new Rectangle(p.X + 38, p.Y + 75, 60, 60);
                    gunRotationCoor = new Vector2(80, 30);
                    gunDamage = p.machinegunDamage;
                    shootSpeed = p.machinegunSpeed;
                    bulletsPerShot = p.machinegunSpread;
                }

                rotationAngleOrig = (float)Math.Atan((gunPos.Y - mousePosition.Y) / (gunPos.X - mousePosition.X));
                rotationAngle = rotationAngleOrig;
                if (rotationAngle > .75) { rotationAngle = .75f; }
                if (rotationAngle < -.75) { rotationAngle = -.75f; }

            }
            else if (p.playerState == PlayerState.faceRight || p.playerState == PlayerState.runRight)
            {
                if (gunName.Equals("shotgun"))
                {
                    gunPos = new Rectangle(p.X + 50, p.Y + 70, 60, 60);
                    gunRotationCoor = new Vector2(15, 30);
                    gunDamage = p.shotgunDamage;
                    shootSpeed = p.shotgunSpeed;
                    bulletsPerShot = p.shotgunSpread;
                }
                if (gunName.Equals("pistol"))
                {
                    gunPos = new Rectangle(p.X + 55, p.Y + 65, 60, 60);
                    gunRotationCoor = new Vector2(10, 25);
                    gunDamage = p.pistolDamage;
                    shootSpeed = p.pistolSpeed;
                    bulletsPerShot = p.pistolSpread;
                }
                if (gunName.Equals("bazooka"))
                {
                    gunPos = new Rectangle(p.X + 55, p.Y + 70, 60, 60);
                    gunRotationCoor = new Vector2(45, 30);
                    gunDamage = p.bazookaDamage;
                    shootSpeed = p.bazookaSpeed;
                    bulletsPerShot = p.bazookaSpread;
                }
                if (gunName.Equals("machinegun"))
                {
                    gunPos = new Rectangle(p.X + 45, p.Y + 75, 60, 60);
                    gunRotationCoor = new Vector2(15, 30);
                    gunDamage = p.machinegunDamage;
                    shootSpeed = p.machinegunSpeed;
                    bulletsPerShot = p.machinegunSpread;
                }
                rotationAngleOrig = (float)Math.Atan((gunPos.Y - mousePosition.Y) / (gunPos.X - mousePosition.X));
                rotationAngle = rotationAngleOrig;
                if (rotationAngle > .75) { rotationAngle = .75f; }
                if (rotationAngle < -.75) { rotationAngle = -.75f; }

            }
        }

        public void Draw(SpriteBatch s, Player p)
        {
            if (p.playerState == PlayerState.faceLeft || p.playerState == PlayerState.runLeft)
            {
                s.Draw(gunSprite, new Vector2(gunPos.X, gunPos.Y), null, gunColor, rotationAngle, gunRotationCoor, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (p.playerState == PlayerState.faceRight || p.playerState == PlayerState.runRight)
            {
                s.Draw(gunSprite, new Vector2(gunPos.X, gunPos.Y), null, gunColor, rotationAngle, gunRotationCoor, 1, SpriteEffects.None, 0);
            }
        }

        //Nick Greenquist - read player attributes from playerParam.txt
        public void ReadGuns(Player p)
        {
            //read in data froma file and set player variables
            StreamReader reader = new StreamReader("gunsParam.txt");
            string line = "";

            line = reader.ReadLine();
            p.pistolSpeed = Double.Parse(line);
            line = reader.ReadLine();
            p.pistolSpread = Int32.Parse(line);

            line = reader.ReadLine();
            p.shotgunSpeed = Double.Parse(line);
            line = reader.ReadLine();
            p.shotgunSpread = Int32.Parse(line);

            line = reader.ReadLine();
            p.machinegunSpeed = Double.Parse(line);
            line = reader.ReadLine();
            p.machinegunSpread = Int32.Parse(line);

            line = reader.ReadLine();
            p.bazookaSpeed = Double.Parse(line);
            line = reader.ReadLine();
            p.bazookaSpread = Int32.Parse(line);


            reader.Close();
        }
    }
}

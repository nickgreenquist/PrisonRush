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
    public class Level
    {
        public Tile[,] tiles;
        public ContentManager content;
        public int screenWidth;
        public int screenHeight;
        public int levelWidth; //number of tiles high
        public int levelHeight; //number of tiles wide
        public Player player;
        public List<Guard> guards;
        public List<Turret> turrets;
        public List<Button> buttons;
        public List<Sign> signs;
        public FinalBoss boss;
        Random rand;
        public List<Particle> particles;
        public Texture2D fire;
        public List<FlameTile> flameTiles;
        public int levelNum;
        public List<Rectangle> explosions;
        public List<double> explosionsTimer;
        public List<bool> explosionAOE;
        Rectangle explosionSpriteSheet;
        Texture2D explosion;
        public Rectangle explosionRadiusRectangle;
        public bool bossExplosion;
        public double bossExplosionTimer;
        public bool countdownExplosion;
        public bool shop;
        public Textbox textbox;
        bool finalLevel;
        public int timer;
        

        private List<String> signTextsList;
        public Dictionary<int, String> signsInDictionary;
        Sound sounds;
        int temp;

        int resurrectTimer;
        int randomTime;
        int randomGuardIndex;

        int signNum;
        int tileSpeed;
        bool bossButtonPressed;
        public Texture2D bossShield;
        public int bossShieldReset;
        public int totalDistanceMoved;
        public List<Tile> tilesMovedWithButtons;

        //properties
        public Tile[,] Tiles
        {
            get { return tiles; }
        }
        public List<Guard> Guards
        {
            get { return guards; }
            set { guards = value; }
        }

        //constructor
        public Level(Player pl, ContentManager contentGame, Textbox tBox, int lvlIndex, int screenWidth, int screenHeight)
        {
            content = contentGame;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            player = pl;
            guards = new List<Guard>();
            boss = new FinalBoss(content.Load<Texture2D>("Boss"), 0, 0, 0, 0, content);
            bossShield = content.Load<Texture2D>("Burst");
            turrets = new List<Turret>();
            buttons = new List<Button>();
            flameTiles = new List<FlameTile>();
            explosions = new List<Rectangle>();
            explosionsTimer = new List<double>();
            explosionAOE = new List<bool>();
            signs = new List<Sign>();
            fire = content.Load<Texture2D>("Fireball");
            explosion = content.Load<Texture2D>("Explosion");
            explosionSpriteSheet = new Rectangle(0, 0, 200, 200);
            sounds = new Sound(content, Game1.StateMachine.Game);
            temp = 0;
            bossExplosion = false;
            bossExplosionTimer = 0;
            rand = new Random();
            levelNum = lvlIndex;
            textbox = tBox;
            signTextsList = textbox.LoadTextFromFile("level" + levelNum + "/level" + levelNum + "text.txt");
            resurrectTimer = 0;
            randomTime = 0;
            finalLevel = false;

            signsInDictionary = textbox.SignPairs;
            timer = 0;
            signNum = 0;
            bossButtonPressed = false;
            bossShieldReset = 0;
            totalDistanceMoved = 0;
            tilesMovedWithButtons = new List<Tile>();
        }

        //Nick Greenquist - moves all tiles and enemies to simulate player movement
        public void MoveTiles(int x)
        {
            //move tiles
            for (int i = 0; i < levelHeight; i++)
            {
                for (int j = 0; j < levelWidth; j++)
                {
                    tiles[i, j].X += x;
                    
                }             
            }
            //move buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].buttonRect.X += x;
            }
            //move guards and bullets
            for (int i = 0; i < guards.Count; i++)
            {
                guards[i].X += x;
                for (int j = 0; j < guards[i].bullets.Count; j++)
                {
                    guards[i].bullets[j].bulletPosition.X += x;
                }
            }
            for (int j = 0; j < boss.bullets.Count; j++)
            {
                boss.bullets[j].bulletPosition.X += x;
            }
            boss.X += x;
            //move turrets and bullets
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].X += x;
                for (int j = 0; j < turrets[i].bullets.Count; j++)
                {
                    turrets[i].bullets[j].bulletRect.X += x;
                }
            }
            for (int i = 0; i < player.bullets.Count; i++)
            {
                player.bullets[i].bulletPosition.X += x;
            }
            //move flametiles and their flames
            for (int i = 0; i < flameTiles.Count; i++)
            {
                flameTiles[i].X += x;
                flameTiles[i].flames.flameRect.X += x;
            }
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i] = new Rectangle(explosions[i].X + x, explosions[i].Y, explosions[i].Width, explosions[i].Height);
            }
            for (int i = 0; i < signs.Count; i++)
            {
                signs[i].XPos += x;
            }
            totalDistanceMoved -= x;
        }

        public void scrollWorldY(int y)
        {
            //move tiles
            for (int i = 0; i < levelHeight; i++)
            {
                for (int j = 0; j < levelWidth; j++)
                {
                    tiles[i, j].Y += y;
                    
                }             
            }
            //move buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].buttonRect.Y += y;
            }
            //move guards and bullets
            for (int i = 0; i < guards.Count; i++)
            {
                guards[i].Y += y;
                for (int j = 0; j < guards[i].bullets.Count; j++)
                {
                    guards[i].bullets[j].bulletPosition.Y += y;
                }
            }
            for (int j = 0; j < boss.bullets.Count; j++)
            {
                boss.bullets[j].bulletPosition.Y += y;
            }
            boss.Y += 500;
            //move turrets and bullets
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].Y += y;
                for (int j = 0; j < turrets[i].bullets.Count; j++)
                {
                    turrets[i].bullets[j].bulletRect.Y += y;
                }
            }
            for (int i = 0; i < player.bullets.Count; i++)
            {
                player.bullets[i].bulletPosition.Y += y;
            }
            //move flametiles and their flames
            for (int i = 0; i < flameTiles.Count; i++)
            {
                flameTiles[i].Y += y;
                flameTiles[i].flames.flameRect.Y += y;
            }
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i] = new Rectangle(explosions[i].Y + y, explosions[i].Y, explosions[i].Width, explosions[i].Height);
            }
            for (int i = 0; i < signs.Count; i++)
            {
                signs[i].signPosition.Y += y;
            }
        }

        //Nick Greenquist - draws the tiles and enemies
        public void Draw(SpriteBatch s, bool shop)
        {
            //draw background tiles
            for (int i = 0; i < levelHeight; i++)
            {
                for (int j = 0; j < levelWidth; j++)
                {
                    
                    if (tiles[i, j].TileTexture != null)
                    {
                        s.Draw(tiles[i, j].TileTexture, tiles[i, j].TileRect, tiles[i,j].tileColor);
                    }
                }
            }
          //draw guards
            for (int i = 0; i < guards.Count; i++)
            {
                guards[i].Draw(s);
            }
            if (levelNum == 18)
            {
                if (bossButtonPressed)
                {
                    boss.DrawBoss(s);
                }
                else
                {
                    boss.shield.Draw(s, boss);
                    boss.DrawBoss(s);
                }
            }
            //draw turrets
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].Draw(s,particles);
            }
            //draw buttons
            for(int i = 0; i < buttons.Count; i++)
            {
                s.Draw(buttons[i].buttonTexture, buttons[i].buttonRect, Color.White);
            }
            //draw flametiles
            for (int i = 0; i < flameTiles.Count; i++)
            {
                flameTiles[i].Draw(s);
            }
            // Draws the signs
            for (int i = 0; i < signs.Count; i++)
            {
                signs[i].Draw(s);
            }
            //draw any explosions resulting from RPG bullet collisions
            for (int i = 0; i < explosions.Count; i++)
            {
                int explosionTime = 12;
                float scale = .25f;
                explosionRadiusRectangle = new Rectangle(explosions[i].X - (int)(explosionSpriteSheet.Width * (scale / 2)), explosions[i].Y - (int)(explosionSpriteSheet.Height * (scale / 2)), explosionSpriteSheet.Width, explosionSpriteSheet.Height); 
                if (player.playerGun.gunName.Equals("bazooka")) 
                {
                    if (temp == 0)
                    {
                        sounds.PlayBazookaExplode();
                    }
                    temp++;
                    scale = 1f;
                    explosionTime = 60;
                    explosionRadiusRectangle = new Rectangle(explosions[i].X - (explosionSpriteSheet.Width / 2), explosions[i].Y - (explosionSpriteSheet.Height / 2), explosionSpriteSheet.Width, explosionSpriteSheet.Height);
                }

                if (countdownExplosion)
                {
                    
                    scale = 15f;
                    explosionTime = 60;
                    explosionRadiusRectangle = new Rectangle(explosions[i].X - (explosionSpriteSheet.Width / 2), explosions[i].Y - (explosionSpriteSheet.Height / 2), explosionSpriteSheet.Width, explosionSpriteSheet.Height);
                }
                if (bossExplosion)
                {
                    scale = 6f;
                    explosionTime = 100;
                    explosionRadiusRectangle = new Rectangle(explosions[i].X - (explosionSpriteSheet.Width / 2), explosions[i].Y - (explosionSpriteSheet.Height / 2), explosionSpriteSheet.Width, explosionSpriteSheet.Height);
                }
                Vector2 explosionLocation = new Vector2(explosionRadiusRectangle.X, explosionRadiusRectangle.Y);

                if (explosionsTimer[i] > 0 && explosionsTimer[i] < (explosionTime / 10))
                {
                    explosionSpriteSheet.X = 0;
                    explosionSpriteSheet.Y = 0;
                    s.Draw(explosion, explosionLocation , explosionSpriteSheet, Color.White,0,Vector2.Zero,scale,SpriteEffects.FlipHorizontally,0);
                }
                if (explosionsTimer[i] > (explosionTime / 10) && explosionsTimer[i] < (explosionTime / 5))
                {
                    explosionSpriteSheet.X = 192;
                    explosionSpriteSheet.Y = 0;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 5) && explosionsTimer[i] < (explosionTime / 2) - ((explosionTime / 5)))
                {
                    explosionSpriteSheet.X = 384;
                    explosionSpriteSheet.Y = 0;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 2) - ((explosionTime / 5)) && explosionsTimer[i] < (explosionTime / 2) - ((explosionTime / 10)))
                {
                    explosionSpriteSheet.X = 576;
                    explosionSpriteSheet.Y = 0;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 2) - ((explosionTime / 10)) && explosionsTimer[i] < (explosionTime / 2))
                {
                    explosionSpriteSheet.X = 768;
                    explosionSpriteSheet.Y = 0;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 2) && explosionsTimer[i] < (explosionTime / 2) + ((explosionTime / 10)))
                {
                    explosionSpriteSheet.X = 0;
                    explosionSpriteSheet.Y = 192;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 2) + ((explosionTime / 10)) && explosionsTimer[i] < (explosionTime / 2) + ((explosionTime / 5)))
                {
                    explosionSpriteSheet.X = 192;
                    explosionSpriteSheet.Y = 192;
                    if (countdownExplosion)
                    {
                        player.Die();
                    }
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime / 2) + ((explosionTime / 5)) && explosionsTimer[i] < (explosionTime - (explosionTime / 5)))
                {
                    explosionSpriteSheet.X = 384;
                    explosionSpriteSheet.Y = 192;
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime - (explosionTime / 5)) && explosionsTimer[i] < (explosionTime - (explosionTime / 10)))
                {
                    explosionSpriteSheet.X = 576;
                    explosionSpriteSheet.Y = 192;
                    
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                if (explosionsTimer[i] > (explosionTime - (explosionTime / 10)) && explosionsTimer[i] < (explosionTime))
                {
                    explosionSpriteSheet.X = 768;
                    explosionSpriteSheet.Y = 192;
                    
                    s.Draw(explosion, explosionLocation, explosionSpriteSheet, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }
                else if (explosionsTimer[i] > 60)
                {
                    explosions.Remove(explosions[i]);
                    temp = 0;
                    explosionsTimer.Remove(explosionsTimer[i]);
                    explosionAOE.Remove(explosionAOE[i]);
                    if (countdownExplosion)
                    {
                        countdownExplosion = false;
                    }
                    if (bossExplosion)
                    {
                        bossExplosion = false;
                    }
                }
            }
        }

        //Nick Greenquist - updates the level
        public void Update(GameTime gameTime)
        {
            //update guards
            for (int i = 0; i < guards.Count; i++)
            {
                guards[i].Update(gameTime, new Vector2(player.PlayerPos.X + (player.PlayerPos.Width / 2), player.PlayerPos.Y + (player.PlayerPos.Height / 2)));
            }
            boss.Update(gameTime, new Vector2(player.PlayerPos.X + (player.PlayerPos.Width / 2), player.PlayerPos.Y + (player.PlayerPos.Height / 2)));
            boss.shield.Update(gameTime);
            //update turrets
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].Update(gameTime);
            }
            //update flametiles
            for (int i = 0; i < flameTiles.Count; i++)
            {
                flameTiles[i].Update(gameTime);
            }
            //update explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosionsTimer[i] += 1;
            }
            //for (int i = 0; i < player.playerGun.bulletCollisions.Count; i++)
            //{
            //    player.playerGun.bulletCollisionsTimer[i]++;
            //}

            CheckBulletCollisions();
            CheckButtonCollision();
            CheckExplosionCollisions();

            if (boss.isDead && boss.deathAnimationTime < 450)
            {
                bossExplosionTimer ++;
                if (bossExplosionTimer > 60)
                {
                    bossExplosion = true;
                    Explosion(boss.X - 350 - rand.Next(0,200), boss.Y - 300 - rand.Next(0,200));
                    explosionAOE.Add(false);
                    bossExplosionTimer = 0;
                    sounds.PlayBazookaExplode();
                }
            }

            if (finalLevel)
            {
                if (!boss.isDead)
                {
                    Resurrect();
                }
                FinalBossCycles();
                if (boss.isDead)
                {
                    player.health = 5;
                    player.hasGun = false;
                    for (int i = 0; i < guards.Count; i++)
                    {
                        guards[i].isDead = true;
                    }
                    for (int i = 0; i < turrets.Count; i++)
                    {
                        turrets[i].isDead = true;
                    }
                }
                if (player.cascioli)
                {
                    
                    if (player.startChrisSong)
                    {
                        boss.bossSprite = content.Load<Texture2D>("BossChris");
                        MediaPlayer.Stop();
                        sounds.Game = content.Load<Song>("22 The Old One");
                        sounds.PlayGame();
                        player.startChrisSong = false;
                    }
                }
            }
        }

        //Nick Greenquist and Eric Kipnis - reads chars from a text file and this builds each level
        public void ReadWorld(string fileName)
        {
            tilesMovedWithButtons = new List<Tile>();
            StreamReader reader = new StreamReader(fileName);
            string line = "";
            string lineX = "";
            char[] charArray;
            int y = 0;

            while (lineX != null)
            {
                lineX = reader.ReadLine();
                line += lineX;
                if (lineX != null)
                {
                    y++;
                }
            }

            charArray = line.ToCharArray();
            levelHeight = y;
            levelWidth = line.Length / levelHeight;
            tiles = new Tile[levelHeight, levelWidth];

            int i = 0; //this is index charArray
            for (y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    if (charArray[i] == 'P')
                    {
                        player.X = x * Tile.tileWidth;
                        player.Y = y * Tile.tileHeight - (player.PlayerPos.Height - Tile.tileHeight);

                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight,TileType.PassThrough); //make a blank tile behind the player
                    }
                    if (charArray[i] == '*')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("Floor Tile 1"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable);
                    }
                    if (charArray[i] == '+')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("doorTile"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable);
                        tilesMovedWithButtons.Add(tiles[y,x]);
                    }
                    if (charArray[i] == '.')
                    {
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                    }
                    if (charArray[i] == 'G')
                    {
                        guards.Add(new Guard(content.Load<Texture2D>("Enemy Spritesheet"), x * Tile.tileWidth, y * Tile.tileHeight - (88 - Tile.tileHeight), 88, 88, 180,content));
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);//make a blank tile behind the enemy
                    }
                    if (charArray[i] == '&') //super guard
                    {
                        guards.Add(new Guard(content.Load<Texture2D>("Super Enemy Spritesheet"), x * Tile.tileWidth, y * Tile.tileHeight - (110 - Tile.tileHeight), 100, 110, 180, content, "superguard"));
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);//make a blank tile behind the enemy
                    }
                    if (charArray[i] == 'T')
                    {
                        turrets.Add(new Turret(content.Load<Texture2D>("Turret"), x * Tile.tileWidth, y * Tile.tileHeight - (88 - Tile.tileHeight), 88, 88, EnemyState.faceLeft,content));
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);//make a blank tile behind the enemy
                    }
                    if (charArray[i] == 'S')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("Spikes"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Spike");
                    }
                    if (charArray[i] == 'F')
                    {
                        flameTiles.Add(new FlameTile(content, content.Load<Texture2D>("Flame Tile"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "FlameTurret"));
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable);//make a blank tile behind the flametile
                    }
                    else if (charArray[i] == 'o')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("chest1"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough, "Coin");
                    }
                    else if (charArray[i] == 'I')
                    {
                        //signNum++;
                        //
                        //// subtract the height of the sign for the y position
                        //signs.Add(new Sign(content.Load<Texture2D>("Sign"), x * Tile.tileWidth, y * Tile.tileHeight - 4, 85, 68, signNum));
                        //tiles[y, x] = new Tile(null, x * Tile.tileHeight, y * Tile.tileHeight, TileType.PassThrough, "Sign"); // blank sign tile behind the sign
                        //
                        //signsInDictionary.Add(signNum, signTextsList[signNum - 1]);
                        signNum++;

                        // subtract the height of the sign for the y position
                        if (levelNum < 10)
                        {
                            signs.Add(new Sign(content.Load<Texture2D>("Sign"), x * Tile.tileWidth, y * Tile.tileHeight - 4, 85, 68, signNum));
                        }
                        else
                        {
                            signs.Add(new Sign(content.Load<Texture2D>("FriendSprite"), x * Tile.tileWidth, y * Tile.tileHeight - 25, 72, 87, signNum));
                        }
                        tiles[y, x] = new Tile(null, x * Tile.tileHeight, y * Tile.tileHeight, TileType.PassThrough, "Sign"); // blank sign tile behind the sign

                        signsInDictionary.Add(signNum, signTextsList[signNum - 1]);
                    }
                    else if (charArray[i] == 'W')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("SwordGet"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Sword");
                    }
                    else if (charArray[i] == 'B')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Capsule"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "PowerBurst");
                    }
                    else if (charArray[i] == 'p')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Pistol Power"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Pistol");
                    }
                    else if (charArray[i] == 's')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Gun Power"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Shotgun");
                    }
                    else if (charArray[i] == 'm')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Machine Gun Power"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Machinegun");
                    }
                    else if (charArray[i] == 'A')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Bazooka Power"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Bazooka");
                    }
                    else if (charArray[i] == 'b')
                    {
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                        buttons.Add(new Button(content.Load<Texture2D>("Button Up"),content.Load<Texture2D>("Button Pressed"),new Rectangle(x * Tile.tileWidth, y * Tile.tileHeight,Tile.tileWidth,Tile.tileHeight)));
                    }
                    else if (charArray[i] == 'D')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("Door1"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough, "Door");
                        tiles[y, x].tileRect.Width = 120;
                        tiles[y, x].tileRect.Height = 120;
                        tiles[y, x].tileRect.Y -= 60;
                        tiles[y, x].tileRect.X += 8;
                    }
                    //Tim Cotanch
                    else if (charArray[i] == 'r')
                    {
                        tiles[y, x] = new PowerUpTile(content.Load<Texture2D>("Capsule 2"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "ShrinkRay");
                    }
                    else if (charArray[i] == 'R')
                    {
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough, "Revert");
                    }
                    else if (charArray[i] == '$')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("DoorShop"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Shop");
                        tiles[y, x].tileRect.Width = 120;
                        tiles[y, x].tileRect.Height = 120;
                        tiles[y, x].tileRect.Y -= 58;
                    }
                    else if (charArray[i] == '@')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("Door1"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "Bonus");
                        tiles[y, x].tileRect.Width = 120;
                        tiles[y, x].tileRect.Height = 120;
                        tiles[y, x].tileRect.Y -= 60;
                    }
                    else if (charArray[i] == '#')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("Door1"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.NonPassable, "EndBonus");
                        tiles[y, x].tileRect.Width = 120;
                        tiles[y, x].tileRect.Height = 120;
                        tiles[y, x].tileRect.Y -= 60;
                    }
                    else if (charArray[i] == 'X') // Final Boss
                    {
                        boss = (new FinalBoss(content.Load<Texture2D>("Boss"), (x * Tile.tileWidth) + 30, y * Tile.tileHeight - 215, 267, 286, content));
                        tiles[y, x] = new Tile(null, x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);//make a blank tile behind the enemy
                        //boss.EnemyPos.Width = 286;
                        //boss.EnemyPos.Height = 286;
                        //boss.EnemyPos.Y -= 226;
                        //boss.X = x;
                        //boss.Y = y;
                    }
                    if (charArray[i] == 't')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("timface"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                    }
                    if (charArray[i] == 'n')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("nickface"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                    }
                    if (charArray[i] == 'e')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("ericface"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                    }
                    if (charArray[i] == 'd')
                    {
                        tiles[y, x] = new Tile(content.Load<Texture2D>("dylanface"), x * Tile.tileWidth, y * Tile.tileHeight, TileType.PassThrough);
                    }
                    i++;
                }
            }
            reader.Close();

            ReadButtons();

            if (levelNum == 18)
            {
                tileSpeed = 2;
                finalLevel = true;
                randomTime = 1000;
                for (int k = 0; k < guards.Count; k++)
                {
                    guards[k].isDead = true;
                }
            }
            if (levelNum == 19)
            {
                for (int j = 0; j < guards.Count; j++)
                {
                    guards[j].bonusRoom = true;
                }
            }
        }

        //Nick Greenquist - read txt files containing info on enemies in the game
        public void ReadEnemies(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string line = "";
            int enemyWalk= 0;
            int i = 0;

            while ((line = reader.ReadLine()) != null)
            {               
                enemyWalk = Int32.Parse(line);
                guards[i].WalkDistance = enemyWalk;
                i++;
            }
            reader.Close();
        }

        public void ReadTurrets(string fileName)
        {
            //read guards
            StreamReader reader = new StreamReader(fileName);
            string line = ""; ;
            int i = 0;

            while ((line = reader.ReadLine()) != null)
            {
                switch (line)
                {
                    case "left":
                        turrets[i].enemyState = EnemyState.faceLeft;
                        break;
                    case "right":
                        turrets[i].enemyState = EnemyState.faceRight;
                        break;
                    default:
                        turrets[i].enemyState = EnemyState.faceLeft;
                        break;
                }

                i++;
            }
            reader.Close();
        }

        //Nick Greenquist - check bullet collision against walls and player
        public void CheckBulletCollisions()
        {
            int tileArrayIndex;
           //check each guard and their bullets
           for (int i = 0; i < guards.Count; i++)
           {
               //check each bullet of one guard
                for (int j = 0; j < guards[i].bullets.Count; j++)
                {
                    if (guards[i].bullets[j].bulletRect.Y <= 0 || guards[i].bullets[j].bulletRect.Y >= 785)
                    {
                        guards[i].bullets.Remove(guards[i].bullets[j]);
                        j--;
                        break;
                    }
                    tileArrayIndex = (guards[i].bullets[j].bulletRect.Y / Tile.tileHeight);
                    for (int k = 0; k < levelWidth; k++)
                    {
                        if (guards[i].bullets[j].bulletRect.Intersects(player.PlayerPos) && !player.shieldActive)
                        {
                            if (player.hasArmor > 0)
                            {
                                player.hasArmor--;
                            }
                            else
                            {
                                player.health--;
                                if (player.health < 0)
                                {
                                    player.Die();
                                }
                            }
                            guards[i].bullets.Remove(guards[i].bullets[j]);
                            j--;
                            break;
                        }
                        if (guards[i].bullets[j].bulletRect.Intersects(tiles[tileArrayIndex, k].TileRect) && tiles[tileArrayIndex, k].type == TileType.NonPassable)
                        {
                            guards[i].bullets.Remove(guards[i].bullets[j]);
                            j--;
                            break;
                        }
                    }
                }
           }
            //check each bullet of final boss
           for (int j = 0; j < boss.bullets.Count; j++)
           {
               if (boss.bullets[j].bulletRect.Y <= 0 || boss.bullets[j].bulletRect.Y >= 785)
               {
                   boss.bullets.Remove(boss.bullets[j]);
                   j--;
                   break;
               }
               tileArrayIndex = (boss.bullets[j].bulletRect.Y / Tile.tileHeight);
               for (int k = 0; k < levelWidth; k++)
               {
                   if (boss.bullets[j].bulletRect.Intersects(player.PlayerPos) && !player.shieldActive)
                   {
                       if (player.hasArmor > 0)
                       {
                           player.hasArmor--;
                       }
                       else
                       {
                           player.health--;
                           if (player.health < 0)
                           {
                               player.Die();
                           }
                       }
                       boss.bullets.Remove(boss.bullets[j]);
                       j--;
                       break;
                   }
                   if (boss.bullets[j].bulletRect.Intersects(tiles[tileArrayIndex, k].TileRect) && tiles[tileArrayIndex, k].type == TileType.NonPassable)
                   {
                       boss.bullets.Remove(boss.bullets[j]);
                       j--;
                       break;
                   }
               }
           }
            

           //check each turret and their bullets
           for (int i = 0; i < turrets.Count; i++)
           {
                //check each bullet of one turret
                for (int j = 0; j < turrets[i].bullets.Count; j++)
                {
                    if (turrets[i].bullets[j].bulletRect.Y <= 0 || turrets[i].bullets[j].bulletRect.Y >= 785)
                    {
                        turrets[i].bullets.Remove(turrets[i].bullets[j]);
                        j--;
                        break;
                    }
                    tileArrayIndex = (turrets[i].bullets[j].bulletRect.Y / Tile.tileHeight);
                    for (int k = 0; k < levelWidth; k++)
                    {
                        if (turrets[i].bullets[j].bulletRect.Intersects(player.PlayerPos) && !player.shieldActive)
                        {
                            if (player.hasArmor > 0)
                            {
                                player.hasArmor--;
                            }
                            else
                            {
                                player.health--;
                                if (player.health < 0)
                                {
                                    player.Die();
                                }
                            }
                            turrets[i].bullets.Remove(turrets[i].bullets[j]);
                            j--;
                            break;
                        }
                        if (turrets[i].bullets[j].bulletRect.Intersects(tiles[tileArrayIndex, k].TileRect) && tiles[tileArrayIndex, k].type == TileType.NonPassable)
                        {
                            turrets[i].bullets.Remove(turrets[i].bullets[j]);
                            j--;
                            break;
                        }
                    }
                }
            }
           //check player bullets
           for (int i = 0; i < player.bullets.Count; i++)
           {
               if (player.bullets[i].bulletRect.Y <= 0 || player.bullets[i].bulletRect.Y >= 785)
               {
                   player.bullets.Remove(player.bullets[i]);
                   i--;
                   break;
               }
               tileArrayIndex = (player.bullets[i].bulletRect.Y / Tile.tileHeight);
               //check enemy collision
               if (player.bullets.Count > 0)
               {
                   for (int j = 0; j < guards.Count; j++)
                   {
                       if (player.bullets[i].bulletRect.Intersects(guards[j].EnemyPos) && !guards[j].isDead)
                       {
                           Explosion(player.bullets[i].bulletRect.X + (player.bullets[i].bulletRect.Width / 2), player.bullets[i].bulletRect.Y + (player.bullets[i].bulletRect.Height / 2));
                           explosionAOE.Add(false);

                           player.bullets.Remove(player.bullets[i]);
                           i--;
                           if (i < 0) { i = 0; }
                           guards[j].health -= player.playerGun.gunDamage;
                           if (guards[j].health <= 0)
                           {
                               sounds.PlayEnemyDead();
                               guards[j].isDead = true;
                               if (guards[j].superGuard.Equals("superguard"))
                               {
                                   player.playerScore.LevelScoreAmount += 10;
                               }
                               else
                               {
                                   player.playerScore.LevelScoreAmount += 4;
                               }
                           }
                           
                           break;
                       }
                   }
               }
               if (player.bullets.Count > 0)
               {
                   for (int j = 0; j < turrets.Count; j++)
                   {
                       if (player.bullets[i].bulletRect.Intersects(turrets[j].EnemyPos) && !turrets[j].isDead)
                       {
                           Explosion(player.bullets[i].bulletRect.X + (player.bullets[i].bulletRect.Width / 2), player.bullets[i].bulletRect.Y + (player.bullets[i].bulletRect.Height / 2));
                           explosionAOE.Add(false);
                           player.bullets.Remove(player.bullets[i]);
                           i--;
                           if (i < 0) { i = 0; }
                           turrets[j].health -= player.playerGun.gunDamage;
                           if (turrets[j].health <= 0)
                           {
                               sounds.PlayEnemyDead();
                               turrets[j].isDead = true;
                               player.playerScore.LevelScoreAmount += 2;
                           }
                           
                           break;
                       }
                   }
               }
               //check collision with tiles or enemies (in order to delete them)
               if (player.bullets.Count > 0)
               {
                   for (int k = 0; k < levelWidth; k++)
                   {
                       if ((player.bullets[i].bulletRect.Intersects(tiles[tileArrayIndex, k].TileRect) && tiles[tileArrayIndex, k].type == TileType.NonPassable))
                       {
                           Explosion(player.bullets[i].bulletRect.X + (player.bullets[i].bulletRect.Width / 2), player.bullets[i].bulletRect.Y + (player.bullets[i].bulletRect.Height / 2));
                           if (player.playerGun.gunName.Equals("bazooka"))
                           {
                               explosionAOE.Add(true);
                           }
                           else
                           {
                               explosionAOE.Add(false);
                           }
                           player.bullets.Remove(player.bullets[i]);
                           i--;
                           if (i < 0) { i = 0; }
                           break;
                       }
                   }
               }
               //Check bullets for final boss
               if (player.bullets.Count > 0)
               {
                   if (player.bullets[i].bulletRect.Intersects(boss.EnemyPos) && !boss.isDead)
                   {
                       Explosion(player.bullets[i].bulletRect.X + (player.bullets[i].bulletRect.Width / 2), player.bullets[i].bulletRect.Y + (player.bullets[i].bulletRect.Height / 2));
                       explosionAOE.Add(false);
                       player.bullets.Remove(player.bullets[i]);
                       i--;
                       if (i < 0) { i = 0; }
                       if (bossButtonPressed)
                       {
                           boss.health -= player.playerGun.gunDamage;
                           if (boss.health <= 0)
                           {
                               boss.isDead = true;
                           }
                       }
                       break;
                   }
               }
           }
        }

        public void CheckExplosionCollisions()
        {

            for (int i = 0; i < explosions.Count; i++)
            {
                explosionRadiusRectangle = new Rectangle(explosions[i].X - (explosionSpriteSheet.Width / 2), explosions[i].Y - (explosionSpriteSheet.Height / 2), explosionSpriteSheet.Width, explosionSpriteSheet.Height);
                if (explosionAOE[i])
                {
                    for (int j = 0; j < guards.Count; j++)
                    {
                        if (explosionRadiusRectangle.Intersects(guards[j].EnemyPos))
                        {
                            explosionAOE[i] = false;
                            guards[j].health -= player.bazookaDamage;
                            if (guards[j].health <= 0)
                            {
                                guards[j].isDead = true;
                            }
                        }
                    }
                    for (int j = 0; j < turrets.Count; j++)
                    {
                        if (explosionRadiusRectangle.Intersects(turrets[j].EnemyPos))
                        {
                            explosionAOE[i] = false;
                            turrets[j].health -= player.bazookaDamage;
                            if (turrets[j].health <= 0)
                            {
                                turrets[j].isDead = true;
                            }
                        }
                    }
                    if (explosionRadiusRectangle.Intersects(boss.EnemyPos))
                    {
                        if (bossButtonPressed)
                        {
                            explosionAOE[i] = false;
                            boss.health -= player.bazookaDamage;
                            if (boss.health <= 0)
                            {
                                boss.isDead = true;
                            }
                        }
                    }
                }
            }
        }
        //Nick Greenquist
        public void CheckButtonCollision()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (player.PlayerPos.Intersects(buttons[i].buttonRect))
                {
                    buttons[i].buttonTexture = buttons[i].buttonTexturePressed;
                    if (buttons[i].colliding == false)
                    {
                        sounds.PlayButton();
                    }
                    buttons[i].colliding = true;

                    for (int j = 0; j < buttons[i].NumTiles + 1; j++)
                    {
                        if (!buttons[i].stopped[j])
                        {
                            if (buttons[i].direction[j].Equals("Y"))
                            {
                                buttons[i].currentDistance[j] += buttons[i].speed[j];
                                if (buttons[i].currentDistance[j] > buttons[i].maxDistance[j] || buttons[i].currentDistance[j] < (-1 * buttons[i].maxDistance[j]))
                                {
                                    buttons[i].currentDistance[j] = 0;
                                    buttons[i].speed[j] *= -1;
                                    if (!buttons[i].looped[j]) //buttons not marked as looped will only move once i.e. doors
                                    {
                                        buttons[i].stopped[j] = true;
                                    }
                                }
                                tiles[buttons[i].TileYCoor[j], buttons[i].TileXCoor[j]].tileRect.Y += buttons[i].speed[j];
                            }
                            if (buttons[i].direction[j].Equals("X"))
                            {
                                buttons[i].currentDistance[j] += buttons[i].speed[j];
                                if (buttons[i].currentDistance[j] > buttons[i].maxDistance[j] || buttons[i].currentDistance[j] < (-1 * buttons[i].maxDistance[j]))
                                {
                                    buttons[i].currentDistance[j] = 0;
                                    buttons[i].speed[j] *= -1;
                                    if (!buttons[i].looped[j]) //buttons not marked as looped will only move once i.e. doors
                                    {
                                        buttons[i].stopped[j] = true;
                                    }
                                }
                                tiles[buttons[i].TileYCoor[j], buttons[i].TileXCoor[j]].tileRect.X += buttons[i].speed[j];
                            }
                        }
                    }
                }
                else
                {
                    buttons[i].buttonTexture = buttons[i].buttonTextureUp;
                    buttons[i].colliding = false;
                }
            }
        }

        //Nick Greenquist
        public void ReadButtons()
        {
            //read in data froma file and set player variables
            // Create the variables
            StreamReader reader = new StreamReader("level" + levelNum + "/buttons" + levelNum + ".txt");
            string line = "";
            int i = 0;

            while (i < buttons.Count) //read as many buttons as there are
            {
                
                line = reader.ReadLine();
                buttons[i].TileYCoor[buttons[i].NumTiles]= Int32.Parse(line);
                line = reader.ReadLine();
                buttons[i].TileXCoor[buttons[i].NumTiles] = Int32.Parse(line);
                line = reader.ReadLine();
                buttons[i].speed[buttons[i].NumTiles] = Int32.Parse(line);
                line = reader.ReadLine();
                buttons[i].maxDistance[buttons[i].NumTiles] = Int32.Parse(line);
                buttons[i].currentDistance[buttons[i].NumTiles] = 0;
                line = reader.ReadLine();
                buttons[i].direction[buttons[i].NumTiles] = line;
                line = reader.ReadLine();
                if (line.Equals("true"))
                {
                    buttons[i].looped[buttons[i].NumTiles] = true;
                }
                
                //if this line says "same", you just add new tiles to the same button (so a button can move more than one buttons)
                line = reader.ReadLine();
                if (line.Equals("same"))
                {
                    buttons[i].NumTiles++;
                }
                else if (line.Equals("next") || line.Equals("end")) //start filling in info for a new buton
                {
                    i++;
                }
            }

            reader.Close();

        }

        public void Explosion(int x, int y)
        {
            explosions.Add(new Rectangle(x, y, 80, 80));
            explosionsTimer.Add(0.1);
        }

        public void Resurrect()
        {
            resurrectTimer++;

            if (resurrectTimer > randomTime)
            {
                resurrectTimer = 0;
                randomTime = rand.Next(1000, 1500);
                randomGuardIndex = rand.Next(0, guards.Count);
                if (guards[randomGuardIndex].isDead == false) //this just gives the boss one more chance in case the guard he is trying to revive is already alive
                {
                    randomGuardIndex = rand.Next(0, guards.Count);
                }
                if (guards[randomGuardIndex].isDead || turrets[0].isDead || turrets[1].isDead)
                {
                    sounds.PlayLaugh();
                }

                guards[randomGuardIndex].isDead = false;
                guards[randomGuardIndex].deathAnimationTime = 0;
                guards[randomGuardIndex].color = Color.White;
                guards[randomGuardIndex].health = 5;
                if (guards[randomGuardIndex].superGuard.Equals("superguard"))
                {
                    guards[randomGuardIndex].health = 30;
                }
                turrets[0].isDead = false;
                turrets[0].deathAnimationTime = 0;
                turrets[0].color = Color.White;
                turrets[0].health = 15;
                turrets[1].isDead = false;
                turrets[1].deathAnimationTime = 0;
                turrets[1].color = Color.White;
                turrets[1].health = 15;
            }
        }

        public void FinalBossCycles()
        {
            //dealing with turret movement
            if (timer < 200)
            {
                timer++;
                tiles[3, 22].Y += tileSpeed;
                turrets[0].Y += tileSpeed;
                turrets[1].Y -= tileSpeed;
                tiles[9, 21].Y -= tileSpeed;
                
            }
            else
            {
                timer = 0;
                tileSpeed *= -1;
            }
            //turing off the boss shield
            if (bossShieldReset < 2000 && bossButtonPressed)
            {
                bossShieldReset++;
            }
            else
            {
                bossButtonPressed = false;
                bossShieldReset = 0;
            }
            if (player.PlayerPos.Intersects(buttons[0].buttonRect) && !bossButtonPressed)
            {
                bossButtonPressed = true;
                bossShieldReset = 0;
            }
        }
    }
}

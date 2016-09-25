using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace GSDIII_game
{
    //Dylan Coats and Nick Greenquist
    class Menu
    {
        public Texture2D title;
        public Texture2D titleOptions;
        public Texture2D pointer;
        public Vector2 pointerPos;
        public Texture2D inGameMain;
        public Texture2D inGameOptions;
        public Texture2D gameOverBack;
        public Texture2D gameOverText;
        public Texture2D twoLives;
        public Texture2D oneLife;
        public Color clr;
        public Video credits;
        public VideoPlayer videoPlayer = new VideoPlayer();
        public Song creditMusic;
        public KeyboardState kbState;
        public KeyboardState prevKbState;
        public MouseState mState;
        public MouseState prevmState;
        public Game1.StateMachine state;
        public int levelNum;
        public Score gameScore;
        public Player p;
        SpriteFont f;
        SpriteFont f2;
        Sound sounds;
        public bool wantedToLoadGame;

        public Color clr2;
        public Rectangle mRect;
        public Texture2D cursor;
        public Texture2D shopMenu;
        Rectangle speedUp;
        Rectangle shieldTime;
        Rectangle swordDamageUp;
        Rectangle swordRangeUp;
        Rectangle pistolDamageUp;
        Rectangle pistolSpeedUp;
        Rectangle shotgunDamageUp;
        Rectangle shotgunSpeedUp;
        Rectangle machinegunDamageUp;
        Rectangle machinegunSpeedUp;
        Rectangle rpgDamageUp;
        Rectangle rpgSpeedUp;
        Rectangle forbiddenItem;
        public List<int> prices; //price for each upgrade is stored in each index
        public List<int> upgradeLevel;

        public Player playerForInputData;
        public ContentManager content;

        public bool SingleKeyPress(Keys ks)
        {
            if (kbState.IsKeyDown(ks) && prevKbState.IsKeyUp(ks))
            {
                return true;
            }
            else { return false; }
        }

        public bool SingleMousePress()
        {
            if (mState.LeftButton == ButtonState.Pressed && prevmState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else { return false; }
        }

        public void GetMousePresses(MouseState prev, MouseState ms)
        {
            prevmState = prev;
            mState = ms;
        }

        public void GetKeyPresses(KeyboardState prev, KeyboardState kbs)
        {
            prevKbState = prev;
            kbState = kbs;
        }

        public Menu(Game1.StateMachine s, int lNum, Score score, Player player, SpriteFont pF, SpriteFont pF2, ContentManager c)
        {
            clr = new Color(0, 0, 0, 0);
            pointerPos = new Vector2(450, 480);
            this.state = s;
            levelNum = lNum;
            gameScore = score;
            p = player;
            f = pF;
            clr2 = Color.White;
            f2 = pF2;
            sounds = new Sound(c, Game1.StateMachine.Game);
            upgradeLevel = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                upgradeLevel.Add(1);
            }

            title = c.Load<Texture2D>("Title Screen");
            titleOptions = c.Load<Texture2D>("Title Screen Options");
            pointer = c.Load<Texture2D>("Menu Pointer");
            gameOverBack = c.Load<Texture2D>("GameOverBack");
            gameOverText = c.Load<Texture2D>("Game Over Text");
            credits = c.Load<Video>("Credits");
            creditMusic = c.Load<Song>("creditsong");
            inGameMain = c.Load<Texture2D>("In Game Main");
            inGameOptions = c.Load<Texture2D>("In Game Options");
            shopMenu = c.Load<Texture2D>("Shop2");
            cursor = c.Load<Texture2D>("Cursor");
            twoLives = c.Load<Texture2D>("LivesLeft");
            oneLife = c.Load<Texture2D>("LivesLeft2");

            speedUp = new Rectangle(650, 135, 75, 50);
            shieldTime = new Rectangle(800, 135, 100, 50);
            swordDamageUp = new Rectangle(650, 210, 75, 50);
            swordRangeUp = new Rectangle(800, 210, 75, 50);
            pistolDamageUp = new Rectangle(650, 285, 75, 50);
            pistolSpeedUp = new Rectangle(800, 285, 75, 50);
            shotgunDamageUp = new Rectangle(650, 360, 75, 50);
            shotgunSpeedUp = new Rectangle(800, 360, 75, 50);
            machinegunDamageUp = new Rectangle(650, 435, 75, 50);
            machinegunSpeedUp = new Rectangle(800, 435, 75, 50);
            rpgDamageUp = new Rectangle(650, 510, 75, 50);
            rpgSpeedUp = new Rectangle(800, 510, 75, 50);
            forbiddenItem = new Rectangle(25, 550, 300, 300);


            prices = new List<int>();
            //create prices
            for (int i = 0; i < 12; i++)
            {
                prices.Add(5);
            }
            content = c;
            wantedToLoadGame = false;
        }

        public Game1.StateMachine ProcessMainMenuInput()
        {
            if (SingleKeyPress(Keys.Down))
            {
                if (pointerPos.Y == 600)
                {
                    pointerPos.Y = 480;
                }
                else { pointerPos.Y += 60; }
            }
            if (SingleKeyPress(Keys.Up))
            {
                if (pointerPos.Y == 480)
                {
                    pointerPos.Y = 600;
                }
                else { pointerPos.Y -= 60; }
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 480)
            {
                this.state = Game1.StateMachine.Game;
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 540)
            {
                //load game method from Game1
                this.state = Game1.StateMachine.Game;
                LoadGame();
                wantedToLoadGame = true;
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 600)
            {
                this.state = Game1.StateMachine.Credits;
                videoPlayer.Play(credits);
            }

            return this.state;

        }

        /*public Game1.StateMachine ProcessMainOptionsInput()
        {
            if (SingleKeyPress(Keys.Down))
            {
                if (pointerPos.Y == 630)
                {
                    pointerPos.Y = 510;
                }
                else { pointerPos.Y += 60; }
            }
            if (SingleKeyPress(Keys.Up))
            {
                if (pointerPos.Y == 510)
                {
                    pointerPos.Y = 630;
                }
                else { pointerPos.Y -= 60; }
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 630)
            {
                this.state = Game1.StateMachine.MainInput;
            }
            if (SingleKeyPress(Keys.Back))
            {
                this.state = Game1.StateMachine.MainMenu;
                pointerPos.Y = 480;
            }

            return this.state;
        }*/  //Removed due to lack of options implemented

        public void ProcessCredits()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(creditMusic);
            }
        }

        public Game1.StateMachine ProcessInGameMainInput(Player player)
        {
            p = player;
            this.state = Game1.StateMachine.InGameMenu;

            if (SingleKeyPress(Keys.Down))
            {
                if (pointerPos.Y == 405)
                {
                    pointerPos.Y = 285;
                }
                else { pointerPos.Y += 60; }
            }
            if (SingleKeyPress(Keys.Up))
            {
                if (pointerPos.Y == 285)
                {
                    pointerPos.Y = 405;
                }
                else { pointerPos.Y -= 60; }
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 285)
            {
                this.state = Game1.StateMachine.Game;
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 405)
            {
                this.state = Game1.StateMachine.MainMenu;
                pointerPos.X = 450;
                pointerPos.Y = 480;
                p.shop = false;
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 345)
            {
                SaveGame();
            }

            return this.state;
            //return Game1.StateMachine.InGameMenu;
        }

        /*public Game1.StateMachine ProcessInGameOptionsInput()
        {
            if (SingleKeyPress(Keys.Down))
            {
                if (pointerPos.Y == 435)
                {
                    pointerPos.Y = 315;
                }
                else { pointerPos.Y += 60; }
            }
            if (SingleKeyPress(Keys.Up))
            {
                if (pointerPos.Y == 315)
                {
                    pointerPos.Y = 435;
                }
                else { pointerPos.Y -= 60; }
            }
            if (SingleKeyPress(Keys.Enter) && pointerPos.Y == 435)
            {
                this.state = Game1.StateMachine.InGameInput;
            }
            if (SingleKeyPress(Keys.Back))
            {
                this.state = Game1.StateMachine.InGameMenu;
                pointerPos.Y = 285;
            }

            return this.state;
        }*/ //Removed due to lack of options implemented


        //Nick Greenquist and Dylan Coats
        public void ProcessShop(Vector2 m)
        {
            /*swordDamageUp = new Rectangle(650, 210, 100, 50);
            swordRangeUp = new Rectangle(800, 210, 100, 50);
            pistolDamageUp = new Rectangle(650, 285, 100, 50);
            pistolSpeedUp = new Rectangle(800, 285, 100, 50);
            shotgunDamageUp = new Rectangle(650, 360, 100, 50);
            shotgunSpeedUp = new Rectangle(800, 360, 100, 50);
            machinegunDamageUp = new Rectangle(650, 435, 100, 50);
            machinegunSpeedUp = new Rectangle(800, 435, 100, 50);
            rpgDamageUp = new Rectangle(650, 210, 510, 50);
            rpgSpeedUp = new Rectangle(800, 210, 510, 50);*/

            prevmState = mState;
            mState =  Mouse.GetState();
            mRect = new Rectangle((int)mState.X, (int)mState.Y, 40, 40);
            GetMousePresses(prevmState, mState);

            if (mRect.Intersects(speedUp) && SingleMousePress() && p.PlayerScore > prices[0] && upgradeLevel[0] <= 50 && p.health < 5)
            {
                    p.PlayerScore -= prices[0];
                    sounds.PlayPurchase();
                    p.health = 5;
                    sounds.PlayPurchase();
                    upgradeLevel[0]++;
                
            }
            else if (mRect.Intersects(shieldTime) && SingleMousePress() && p.PlayerScore > prices[1] && upgradeLevel[1] <= 5)
            {
                p.powerUpTimerDuration -= .2;
                p.PlayerScore -= prices[1];
                prices[1] += 3;
                sounds.PlayPurchase();
                upgradeLevel[1]++;
            }
            else if (mRect.Intersects(swordDamageUp) && SingleMousePress() && p.PlayerScore > prices[2] && upgradeLevel[2] < 50 && p.life < 3) //another life
            {
                p.life++;
                p.PlayerScore -= prices[2];
                //prices[2]++;
                sounds.PlayPurchase();
                upgradeLevel[2]++;
            }
            else if (mRect.Intersects(swordRangeUp) && SingleMousePress() && p.PlayerScore > prices[3] && upgradeLevel[3] < 50 && p.hasArmor < 6) //player armor
            {
                p.hasArmor++;
                p.PlayerScore -= prices[3];
                //prices[3]++;
                sounds.PlayPurchase();
                upgradeLevel[3]++;
            }
            else if (mRect.Intersects(pistolDamageUp) && SingleMousePress() && p.PlayerScore > prices[4] && upgradeLevel[4] <= 3 && p.gunList[0])
            {
                p.PlayerScore -= prices[4];
                prices[4]+=5;
                p.pistolDamage++;
                sounds.PlayPurchase();
                upgradeLevel[4]++;
            }
            else if (mRect.Intersects(pistolSpeedUp) && SingleMousePress() && p.PlayerScore > prices[5] && upgradeLevel[5] <= 5 && p.gunList[0])
            {
                p.pistolSpeed -= .08;
                p.PlayerScore -= prices[5];
                prices[5]+=3;
                sounds.PlayPurchase();
                upgradeLevel[5]++;
            }
            else if (mRect.Intersects(shotgunDamageUp) && SingleMousePress() && p.PlayerScore > prices[6] && upgradeLevel[6] <= 3 && p.gunList[1])
            {
                p.shotgunDamage++;
                p.PlayerScore -= prices[6];
                prices[6]+=5;
                sounds.PlayPurchase();
                upgradeLevel[6]++;
            }
            else if (mRect.Intersects(shotgunSpeedUp) && SingleMousePress() && p.PlayerScore > prices[7] && upgradeLevel[7] <= 5 && p.gunList[1])
            {
                p.shotgunSpeed -= .07;
                p.PlayerScore -= prices[7];
                prices[7]+=3;
                sounds.PlayPurchase();
                upgradeLevel[7]++;
            }
            else if (mRect.Intersects(machinegunDamageUp) && SingleMousePress() && p.PlayerScore > prices[8] && upgradeLevel[8] <= 3 && p.gunList[2])
            {
                p.machinegunDamage++;
                p.PlayerScore -= prices[8];
                prices[8]+=5;
                sounds.PlayPurchase();
                upgradeLevel[8]++;
            }
            else if (mRect.Intersects(machinegunSpeedUp) && SingleMousePress() && p.PlayerScore > prices[9] && upgradeLevel[9] <= 5 && p.gunList[2])
            {
                p.machinegunSpeed -= .045;
                p.PlayerScore -= prices[9];
                prices[9]+=3;
                sounds.PlayPurchase();
                upgradeLevel[9]++;

            }
            else if (mRect.Intersects(rpgDamageUp) && SingleMousePress() && p.PlayerScore > prices[10] && upgradeLevel[10] <= 3 && p.gunList[3])
            {
                p.bazookaDamage+=5;
                p.PlayerScore -= prices[10];
                prices[10]+=5;
                sounds.PlayPurchase();
                upgradeLevel[10]++;
            }
            else if (mRect.Intersects(rpgSpeedUp) && SingleMousePress() && p.PlayerScore > prices[11] && upgradeLevel[11] <= 5 && p.gunList[3])
            {
                p.bazookaSpeed -= .08;
                p.PlayerScore -= prices[11];
                prices[11]+=3;
                sounds.PlayPurchase();
                upgradeLevel[11]++;
            }
            else if (mRect.Intersects(forbiddenItem) && SingleMousePress() && p.PlayerScore >= 3000)
            {
                p.cascioli = true;
                p.playerScore.ScoreAmount -= 3000;
                sounds.PlayPurchase();
                p.startChrisSong = true;
            }

        }

        public void DrawMainMenu(SpriteBatch s)
        {
            s.Draw(title, new Rectangle(0, 0, title.Width, title.Height), Color.White);
            s.Draw(pointer, pointerPos, Color.White);
        }

        public void DrawMainOptions(SpriteBatch s)
        {
            s.Draw(titleOptions, new Rectangle(0, 0, title.Width, title.Height), Color.White);
            s.Draw(pointer, pointerPos, Color.White);
        }

        public void DrawCredits(SpriteBatch s)
        {
            s.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, credits.Width, credits.Height), Color.CornflowerBlue);

        }

        public void DrawInGameMenu(SpriteBatch s)
        {
            s.Draw(inGameMain, new Rectangle(0, 0, title.Width, title.Height), Color.White);
            s.Draw(pointer, pointerPos, Color.White);
        }

        public void DrawInGameOptions(SpriteBatch s)
        {
            s.Draw(inGameOptions, new Rectangle(0, 0, title.Width, title.Height), Color.White);
            s.Draw(pointer, pointerPos, Color.White);
        }

        public void DrawGameOver(SpriteBatch s, SpriteFont f, Player p)
        {
            if (p.life == 2)
            {
                s.Draw(twoLives, new Rectangle(475, 350, twoLives.Width, twoLives.Height), Color.White);
                s.DrawString(f, "Press Enter to Continue", new Vector2(525, 700), Color.White);
            }
            else if (p.life == 1)
            {
                s.Draw(oneLife, new Rectangle(475, 350, oneLife.Width, oneLife.Height), Color.White);
                s.DrawString(f, "Press Enter to Continue", new Vector2(525, 700), Color.White);
            }
            else
            {
                s.Draw(gameOverBack, new Rectangle(0, 0, gameOverBack.Width, gameOverBack.Height), Color.White);
                s.Draw(gameOverText, new Rectangle(0, 0, gameOverText.Width, gameOverText.Height), clr);
                if (clr.A < 255)
                {
                    clr.A += 1;
                    clr.R += 1;
                    clr.G += 1;
                    clr.B += 1;
                }
                else { s.DrawString(f, "Press Enter to Return to Main Menu", new Vector2(325, 700), Color.Crimson); }
            }
        }

        public void DrawShop(SpriteBatch s, Player p)
        {
            s.Draw(shopMenu, new Rectangle(0, 0, 1200, 800), Color.White);
            s.DrawString(f2, "Press Back to Exit", new Vector2(600, 50), Color.Black);
            s.DrawString(f2, "Money: $" + p.PlayerScore, new Vector2(600, 25), Color.Black);
            s.Draw(cursor, mRect, clr2);
            s.DrawString(f2, "Power Ups", new Vector2(450, 125), Color.Black);

            s.DrawString(f, "Health Vial $" + prices[0], new Vector2(650, 135), Color.Black);
            s.DrawString(f, "Shield Duration(" + upgradeLevel[1] + ") $" + prices[1], new Vector2(800, 135), Color.Black);
            s.DrawString(f2, "Survivability", new Vector2(450, 200), Color.Black);
            s.DrawString(f, "Extra Life" + " $" + prices[2], new Vector2(650, 210), Color.Black);
            s.DrawString(f, "Armor" + " $" + prices[3], new Vector2(800, 210), Color.Black);
            if (p.hasGun)
            {
                if (p.gunList[0])
                {
                    s.DrawString(f2, "Pistol", new Vector2(450, 275), Color.Black);
                    s.DrawString(f, "Damage Up(" + upgradeLevel[4] + ") $" + prices[4], new Vector2(650, 285), Color.Black);
                    s.DrawString(f, "Speed Up(" + upgradeLevel[5] + ") $" + prices[5], new Vector2(800, 285), Color.Black);
                }
                if (p.gunList[1])
                {
                    s.DrawString(f2, "Shotgun", new Vector2(450, 350), Color.Black);
                    s.DrawString(f, "Damage Up(" + upgradeLevel[6] + ") $" + prices[6], new Vector2(650, 360), Color.Black);
                    s.DrawString(f, "Speed Up(" + upgradeLevel[7] + ") $" + prices[7], new Vector2(800, 360), Color.Black);
                }
                if (p.gunList[2])
                {
                    s.DrawString(f2, "Machine Gun", new Vector2(450, 425), Color.Black);
                    s.DrawString(f, "Damage Up(" + upgradeLevel[8] + ") $" + prices[8], new Vector2(650, 435), Color.Black);
                    s.DrawString(f, "Speed Up(" + upgradeLevel[9] + ") $" + prices[9], new Vector2(800, 435), Color.Black);
                }
                if (p.gunList[3])
                {
                    s.DrawString(f2, "RPG", new Vector2(450, 500), Color.Black);
                    s.DrawString(f, "Damage Up(" + upgradeLevel[10] + ") $" + prices[10], new Vector2(650, 510), Color.Black);
                    s.DrawString(f, "Speed Up(" + upgradeLevel[11] + ") $" + prices[11], new Vector2(800, 510), Color.Black);
                }
            }

        }

        public int getLevelNum()
        {
            return levelNum;
        }

        //Nick Greenquist
        public void LoadGame()
        {
            StreamReader saveGameReader = new StreamReader("save.txt");


            string line = "";
            line = saveGameReader.ReadLine(); //levelnum
            levelNum = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //hassword
            if (line.Equals("True"))
            {
                p.hasSword = true;
            }
            else
            {
                p.hasSword = false;
            }
            line = saveGameReader.ReadLine(); //hasshield
            if (line.Equals("True"))
            {
                p.hasShield = true;
            }
            else
            {
                p.hasShield = false;
            }
            line = saveGameReader.ReadLine(); //hasgun
            if (line.Equals("True"))
            {
                p.hasGun = true;
            }
            else
            {
                p.hasGun = false;
            }
            line = saveGameReader.ReadLine(); //pistol?
            if (line.Equals("True"))
            {
                p.gunList[0] = true;
                p.playerGun = new Gun(content, "pistol", p);
            }
            else
            {
                p.gunList[0] = false;
            }
            line = saveGameReader.ReadLine(); //shotgun?
            if (line.Equals("True"))
            {
                p.gunList[1] = true;
                p.playerGun = new Gun(content, "shotgun", p);
            }
            else
            {
                p.gunList[1] = false;
            }
            line = saveGameReader.ReadLine(); //machinegun?
            if (line.Equals("True"))
            {
                p.gunList[2] = true;
                p.playerGun = new Gun(content, "machinegun", p);
            }
            else
            {
                p.gunList[2] = false;
            }
            line = saveGameReader.ReadLine(); //bazooka?
            if (line.Equals("True"))
            {
                p.gunList[3] = true;
                p.playerGun = new Gun(content, "bazooka", p);
            }
            else
            {
                p.gunList[3] = false;
            }
            line = saveGameReader.ReadLine(); //score
            p.playerScore.ScoreAmount = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //speed
            p.playerSpeed = Int32.Parse(line);
            p.playerSpeedOrig = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shield duration
            p.powerUpTimerDuration = Double.Parse(line);
            line = saveGameReader.ReadLine(); //life
            p.life = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //speed shop upgrade level
            upgradeLevel[0] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //speed shop price level
            prices[0] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shieldshop upgrade level
            upgradeLevel[1] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shield shop price level
            prices[1] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //lives shop upgrade level
            upgradeLevel[2] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //lives shop price level
            prices[2] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //armor shop upgrade level
            upgradeLevel[3] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //armor shop price level
            prices[3] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol damage shop upgrade level
            upgradeLevel[4] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol damage shop price level
            prices[4] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol speed shop upgrade level
            upgradeLevel[5] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol speed shop price level
            prices[5] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun damage shop upgrade level
            upgradeLevel[6] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun damage shop price level
            prices[6] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun speed shop upgrade level
            upgradeLevel[7] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun speed shop price level
            prices[7] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //machineG damage shop upgrade level
            upgradeLevel[8] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //machineG damage shop price level
            prices[8] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //machineG speed shop upgrade level
            upgradeLevel[9] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //machineG speed shop price level
            prices[9] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka damage shop upgrade level
            upgradeLevel[10] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka damage shop price level
            prices[10] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka speed shop upgrade level
            upgradeLevel[11] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka speed shop price level
            prices[11] = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol damage
            p.pistolDamage = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //pistol speed
            p.pistolSpeed = Double.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun damage
            p.shotgunDamage = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //shotgun speed
            p.shotgunSpeed = Double.Parse(line);
            line = saveGameReader.ReadLine(); //machineG damage
            p.machinegunDamage = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //machineG speed
            p.machinegunSpeed = Double.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka damage
            p.bazookaDamage = Int32.Parse(line);
            line = saveGameReader.ReadLine(); //bazooka speed
            p.bazookaSpeed = Double.Parse(line);


            saveGameReader.Close();

        }

        //Nick Greenquist
        public void SaveGame()
        {
            //add a save file, saves: levelnum, hasgun, hasshield, hassword, money, lives,
            StreamWriter saveGameWriter = new StreamWriter("save.txt");
            saveGameWriter.WriteLine(levelNum);
            saveGameWriter.WriteLine(p.hasSword);
            saveGameWriter.WriteLine(p.hasShield);
            saveGameWriter.WriteLine(p.hasGun);
            saveGameWriter.WriteLine(p.gunList[0]);
            saveGameWriter.WriteLine(p.gunList[1]);
            saveGameWriter.WriteLine(p.gunList[2]);
            saveGameWriter.WriteLine(p.gunList[3]);
            saveGameWriter.WriteLine(p.playerScore.ScoreAmount); //money
            saveGameWriter.WriteLine(p.playerSpeedOrig); //speed
            saveGameWriter.WriteLine(p.powerUpTimerDuration); //shield duration
            saveGameWriter.WriteLine(p.life); //lives left

            //save store stats
            saveGameWriter.WriteLine(upgradeLevel[0]); //playerspeed
            saveGameWriter.WriteLine(prices[0]);
            saveGameWriter.WriteLine(upgradeLevel[1]); //shield
            saveGameWriter.WriteLine(prices[1]);
            saveGameWriter.WriteLine(upgradeLevel[2]); //lives
            saveGameWriter.WriteLine(prices[2]);
            saveGameWriter.WriteLine(upgradeLevel[3]); //armor
            saveGameWriter.WriteLine(prices[3]);
            saveGameWriter.WriteLine(upgradeLevel[4]); //pistol
            saveGameWriter.WriteLine(prices[4]);
            saveGameWriter.WriteLine(upgradeLevel[5]);
            saveGameWriter.WriteLine(prices[5]);
            saveGameWriter.WriteLine(upgradeLevel[6]); //shotgun
            saveGameWriter.WriteLine(prices[6]);
            saveGameWriter.WriteLine(upgradeLevel[7]);
            saveGameWriter.WriteLine(prices[7]);
            saveGameWriter.WriteLine(upgradeLevel[8]); //machinegun
            saveGameWriter.WriteLine(prices[8]);
            saveGameWriter.WriteLine(upgradeLevel[9]);
            saveGameWriter.WriteLine(prices[9]);
            saveGameWriter.WriteLine(upgradeLevel[10]);//rpg
            saveGameWriter.WriteLine(prices[10]);
            saveGameWriter.WriteLine(upgradeLevel[11]);
            saveGameWriter.WriteLine(prices[11]);

            //save gun stats bc these are things that can be changed by buying upgrades
            saveGameWriter.WriteLine(p.pistolDamage);
            saveGameWriter.WriteLine(p.pistolSpeed);
            saveGameWriter.WriteLine(p.shotgunDamage);
            saveGameWriter.WriteLine(p.shotgunSpeed);
            saveGameWriter.WriteLine(p.machinegunDamage);
            saveGameWriter.WriteLine(p.machinegunSpeed);
            saveGameWriter.WriteLine(p.bazookaDamage);
            saveGameWriter.WriteLine(p.bazookaSpeed);

            saveGameWriter.Close();

        }
    }
}

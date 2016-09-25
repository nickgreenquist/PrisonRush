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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Attributes
        Player player;
        Player clone;
        Level level;
        SpriteFont font;
        SpriteFont font2;
        Texture2D background;
        Texture2D enemySprite;
        Menu menu;
        Background bg;
        StateMachine state;
        float FPS;
        float elapsedTime;
        int frameCounter;
        int levelNum;
        Sword sword;
        Gun gun;
        PowerBurst burst;
        PowerUp swordPowerUp;
        Sound sounds;
        UI userI;

        Texture2D cursorTexture;
        Vector2 cursorPosition;

        KeyboardState kbState;
        KeyboardState prevKbState;

        Score gameScore;
        CountDown countDown;

        Textbox textbox;
        int creditTimer;

        //dylan coats
        public enum StateMachine
        {
            Game, MainMenu, GameOver, MainOptions, InGameOptions, InGameMenu, MainInput, InGameInput, Credits, Shop, ShopMenu
        }

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// //Nick Greebquist and Dylan Coats
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();
            sounds = new Sound(Content, state);
            this.state = StateMachine.MainMenu;

            levelNum = 1;
            gameScore = new Score();
            countDown = new CountDown(100);

            textbox = new Textbox(Content);

            //level and player initilization
            player = new Player(GraphicsDevice.Viewport.Width / 2 - 100, 166, 88, 88, gameScore, textbox, Content, state);
            //clone = new Player(GraphicsDevice.Viewport.Width / 2 - 100 + 200, 166, 88, 88, gameScore);
            //clone.isClone = true;

            //level = new Level(player, Content, textbox, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //level.levelNum = levelNum;
            player.GameWorld = level;
            //clone.GameWorld = level;
            player.GroundY = GraphicsDevice.Viewport.Height + 10;
            //clone.GroundY = GraphicsDevice.Viewport.Height;

            //if (File.Exists("save.txt"))
            //{
            //    LoadGame("save.txt");
            //}

            //level.ReadWorld("level" + levelNum + "/level" + levelNum + ".txt");
            //level.ReadEnemies("level" + levelNum + "/guard" + levelNum + ".txt");
            //level.ReadTurrets("level" + levelNum + "/turrets" + levelNum + ".txt");

            font = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");

            //TODO move this to player class
            sword = new Sword(Content.Load<Texture2D>("Sword"), true, 5);
            player.playerSword = sword;
            gun = new Gun(Content,"bazooka",player);
            player.playerGun = gun;
            player.playerGun.ReadGuns(player);

            //clone.playerSword = sword;
            burst = new PowerBurst(true, 5);
            player.shield = burst;
            //clone.shield = burst;

            userI = new UI(Content, player);
            menu = new Menu(state, levelNum, gameScore, player, font, font2, Content);
            player.shop = false;


            //background initialization
            bg = new Background();

            bg.fore = new ScrollingBackground(Content.Load<Texture2D>("Foreground"), new Rectangle(0, 0, 1200, 800));
            bg.mid = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround"), new Rectangle(0, 0, 1200, 800));
            bg.back = new ScrollingBackground(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 1200, 800));
            bg.fore2 = new ScrollingBackground(Content.Load<Texture2D>("Foreground"), new Rectangle(1200, 0, 1200, 800));
            bg.mid2 = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround"), new Rectangle(1200, 0, 1200, 800));
            bg.back2 = new ScrollingBackground(Content.Load<Texture2D>("Background"), new Rectangle(1200, 0, 1200, 800));
            
            base.Initialize();

            levelNum--;
            creditTimer = 0;
            NextLevel();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()

        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player.PlayerSprite = this.Content.Load<Texture2D>("Character Moving Sprites");
            //clone.PlayerSprite = this.Content.Load<Texture2D>("Character Moving Sprites");
            background = Content.Load<Texture2D>("All In One BG");
            enemySprite = this.Content.Load<Texture2D>("Enemy Spritesheet");
            burst.BurstTexture = Content.Load<Texture2D>("Burst");
            gameScore.ScoreFont = Content.Load<SpriteFont>("SpriteFont1");
            cursorTexture = Content.Load<Texture2D>("cursor");
            countDown.TimeFont = Content.Load<SpriteFont>("SpriteFont1");
        }

        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        //    //add a save file, saves: levelnum, hasgun, hasshield, hassword, money, lives,
        //    StreamWriter saveGameWriter = new StreamWriter("save.txt");
        //    saveGameWriter.WriteLine(levelNum);
        //    saveGameWriter.WriteLine(player.hasSword); 
        //    saveGameWriter.WriteLine(player.hasShield);
        //    saveGameWriter.WriteLine(player.hasGun);
        //    saveGameWriter.WriteLine(player.gunList[0]);
        //    saveGameWriter.WriteLine(player.gunList[1]);
        //    saveGameWriter.WriteLine(player.gunList[2]);
        //    saveGameWriter.WriteLine(player.gunList[3]);          
        //    saveGameWriter.WriteLine(player.playerScore.ScoreAmount); //money
        //    saveGameWriter.WriteLine(player.playerSpeedOrig); //speed
        //    saveGameWriter.WriteLine(player.powerUpTimerDuration); //shield duration
        //
        //    //save store stats
        //    saveGameWriter.WriteLine(menu.upgradeLevel[0]); //playerspeed
        //    saveGameWriter.WriteLine(menu.prices[0]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[1]); //shield
        //    saveGameWriter.WriteLine(menu.prices[1]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[2]); //lives
        //    saveGameWriter.WriteLine(menu.prices[2]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[3]); //armor
        //    saveGameWriter.WriteLine(menu.prices[3]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[4]); //pistol
        //    saveGameWriter.WriteLine(menu.prices[4]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[5]); 
        //    saveGameWriter.WriteLine(menu.prices[5]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[6]); //shotgun
        //    saveGameWriter.WriteLine(menu.prices[6]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[7]); 
        //    saveGameWriter.WriteLine(menu.prices[7]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[8]); //machinegun
        //    saveGameWriter.WriteLine(menu.prices[8]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[9]);
        //    saveGameWriter.WriteLine(menu.prices[9]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[10]);//rpg
        //    saveGameWriter.WriteLine(menu.prices[10]);
        //    saveGameWriter.WriteLine(menu.upgradeLevel[11]);
        //    saveGameWriter.WriteLine(menu.prices[11]);
        //
        //    //save gun stats bc these are things that can be changed by buying upgrades
        //    saveGameWriter.WriteLine(player.pistolDamage);
        //    saveGameWriter.WriteLine(player.pistolSpeed);
        //    saveGameWriter.WriteLine(player.shotgunDamage);
        //    saveGameWriter.WriteLine(player.shotgunSpeed);
        //    saveGameWriter.WriteLine(player.machinegunDamage);
        //    saveGameWriter.WriteLine(player.machinegunSpeed);
        //    saveGameWriter.WriteLine(player.bazookaDamage);
        //    saveGameWriter.WriteLine(player.bazookaSpeed);
        //    saveGameWriter.Close();
        //    
        
            //stop all sound and spritebatches
            MediaPlayer.Stop();
            sounds.StopEverything();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// //Nick Greenquist and Dylan Coats
        protected override void Update(GameTime gameTime)
        {
            cursorPosition.X = Mouse.GetState().X;
            cursorPosition.Y = Mouse.GetState().Y;

            //Nick Greenquist and Dylan Coats - check for win of the level
            if (player.wonLevel)
            {
                if (levelNum == 19)
                {
                    levelNum = 16;
                    NextLevel();
                    countDown.TimeLimit = 200;
                }
                else if (levelNum >= 10)
                {
                    countDown.TimeLimit = 200;
                    NextLevel();
                }
                else
                {
                    countDown.TimeLimit = 100;
                    NextLevel();
                }
            }

            if (player.bonusLevel)
            {
                levelNum = 19;
                player.bonusLevel = false;
                BonusLevel();
            }

            if (countDown.TimeLimit <= 0)
            {
                level.Explosion(-800, -1000 );
                level.explosionAOE.Add(false);
                level.countdownExplosion = true;
                countDown.TimeLimit = 600;
            }

            if (player.isDead)
            {
                countDown.TimeLimit = 100;
            }

            

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            prevKbState = kbState;
            kbState = Keyboard.GetState();

            menu.GetKeyPresses(prevKbState,kbState);

            if (state == StateMachine.MainMenu) 
            {
                state = menu.ProcessMainMenuInput();
                if (menu.wantedToLoadGame)
                {
                    if (level.textbox.SignTexts != null && level.textbox.SignTexts.Count > 0)
                    {
                        level.textbox.SignTexts.Clear();
                    }
                    if (level.textbox.SignPairs != null && level.textbox.SignPairs.Count > 0)
                    {
                        level.textbox.SignPairs.Clear();
                    }
                    if (level.signsInDictionary != null && level.signsInDictionary.Count > 0)
                    {
                        level.signsInDictionary.Clear();
                    }
                    levelNum = menu.levelNum;
                    levelNum--;
                    NextLevel();
                    menu.wantedToLoadGame = false;
                }

                if (state == StateMachine.MainMenu)
                {
                    sounds.PlayMainMenu();
                }
                else { MediaPlayer.Stop(); }              
            }
            else if (state == StateMachine.Credits) { menu.ProcessCredits(); }
            //else if (state == StateMachine.MainOptions) { state = menu.ProcessMainOptionsInput(); }

            //The good stuff!
            else if (state == StateMachine.Game)
            {
                if (SingleKeyPress(Keys.P))
                {
                    menu.pointerPos.Y = 285;
                    menu.pointerPos.X = 300;
                    //menu.kbState = new KeyboardState(Keys.PageDown);
                    state = StateMachine.InGameMenu;
                }

                sounds.PlayGame();

                if (player.isDead && player.deathAnimationTime > 28)
                {
                    state = StateMachine.GameOver;
                    player.color = Color.White;
                    menu.pointerPos.X = 450;
                    menu.pointerPos.Y = 480;
                    player.deathAnimationTime = 0;
                    sounds.PlayGameOver();
                }

                //check fps
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameCounter++;
                if (elapsedTime > 1)
                {
                    FPS = frameCounter;
                    frameCounter = 0;
                    elapsedTime = 0;
                }

                bg.ProcessInput(level);
                //update the level and the player
                level.Update(gameTime);
                player.Update(gameTime,cursorPosition);

                textbox.Update(gameTime);

                //clone.Update(gameTime);
                if (!player.inTextBox)
                {
                    countDown.Update(gameTime);
                }
                //base.Update(gameTime);

                if (player.shop == true)
                {
                    MediaPlayer.Stop();
                    state = StateMachine.Shop;
                }

                if (level.boss.deathAnimationTime > 600 && level.boss.isDead)
                {
                    MediaPlayer.Stop();
                    menu.state = StateMachine.Credits;
                    menu.videoPlayer.Play(menu.credits);
                    state = StateMachine.Credits;
                }
                

            }

            else if (state == StateMachine.Shop)
            {
                sounds.PlayShop();
                if (SingleKeyPress(Keys.P))
                {
                    menu.pointerPos.Y = 285;
                    menu.pointerPos.X = 300;
                    //menu.kbState = new KeyboardState(Keys.PageDown);
                    state = StateMachine.InGameMenu;
                }
                if (SingleKeyPress(Keys.Back))
                {
                    player.shop = false;
                    MediaPlayer.Stop();
                    state = Game1.StateMachine.Game;
                }

                menu.ProcessShop(new Vector2(cursorPosition.X, cursorPosition.Y));
            }

            else if (state == StateMachine.InGameMenu)
            {
                menu.levelNum = levelNum;
                state = menu.ProcessInGameMainInput(player);
            }

            /*else if (state == StateMachine.InGameOptions)
            {
                state = menu.ProcessInGameOptionsInput();
            }*/

            else if (state == StateMachine.GameOver)
            {
                player.isDead = false;

                //restart current level

                if (menu.SingleKeyPress(Keys.Enter) && player.life > 0)
                {
                    levelNum--;
                    NextLevel();
                    MediaPlayer.Stop();
                    state = StateMachine.Game;
                    menu.state = StateMachine.Game;
                    menu.clr = new Color(0, 0, 0, 0); 
                }
                else if (menu.SingleKeyPress(Keys.Enter))
                {
                    //levelNum--;
                    //NextLevel();
                    //MediaPlayer.Stop();
                    //state = StateMachine.MainMenu;
                    //menu.state = StateMachine.MainMenu;
                    //menu.clr = new Color(0, 0, 0, 0);
                    Initialize();
                }
            }
        }

        public bool SingleKeyPress(Keys ks)
        {
            if (kbState.IsKeyDown(ks) && prevKbState.IsKeyUp(ks))
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// //Dylan Coats
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (state == StateMachine.MainMenu) { menu.DrawMainMenu(spriteBatch); }

            else if (state == StateMachine.MainOptions) { menu.DrawMainOptions(spriteBatch); }

            else if (state == StateMachine.Credits)
            { 
                menu.DrawCredits(spriteBatch);
                creditTimer++;
                if (creditTimer > 500 && creditTimer < 800)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Nick Greenquist", new Vector2(475, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("nickface"), new Rectangle(400,250,300,300), Color.White);
                }
                if (creditTimer > 800 && creditTimer < 1100)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Dylan Coats", new Vector2(475, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("dylanface"), new Rectangle(400, 250, 300, 300), Color.White);
                }
                if (creditTimer > 1100 && creditTimer < 1400)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Tim Cotanch", new Vector2(475, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("timface"), new Rectangle(400, 250, 300, 300), Color.White);
                }
                if (creditTimer > 1400 && creditTimer < 1700)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Eric Kipnis", new Vector2(475, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("ericface"), new Rectangle(400, 250, 300, 300), Color.White);
                }
                if (creditTimer > 1700 && creditTimer < 2300)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Concept Art:", new Vector2(475, 200),Color.White,0,Vector2.Zero,2,SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("stick"), new Rectangle(400, 250, 300, 300), Color.White);
                }
                if (creditTimer > 2300 && creditTimer < 2900)
                {
                    spriteBatch.DrawString(countDown.TimeFont, "Concept Art:", new Vector2(475, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(Content.Load<Texture2D>("stick2"), new Rectangle(400, 250, 300, 300), Color.White);
                }
                if (creditTimer > 2900 && creditTimer < 5400)
                {
                    spriteBatch.Draw(Content.Load<Texture2D>("thanks"), new Rectangle(150, 0, 1000, 1000), Color.White);
                }
                if (creditTimer > 5400)
                {
                    menu.videoPlayer.Stop();
                    state = StateMachine.MainMenu;
                    menu.state = StateMachine.MainMenu;
                    creditTimer = 0;
                }
            }

            //Draw background
            else if (state == StateMachine.Game)
            {
                DrawGame();
                textbox.Draw(spriteBatch);
            }

            else if (state == StateMachine.Shop)
            {
                menu.DrawShop(spriteBatch, player);
            }

            else if (state == StateMachine.InGameMenu)
            {
                if (player.shop == false)
                {
                    DrawGame();
                }
                else
                {
                    spriteBatch.DrawString(font, "" + FPS, new Vector2(10, 10), Color.Black);
                    //Draw Background Layers
                    bg.Draw(spriteBatch);

                    level.Draw(spriteBatch, true);
                    player.Draw(spriteBatch, cursorPosition);
                    
                    //clone.Draw(spriteBatch);
                    userI.Draw(spriteBatch, player);
                    gameScore.Draw(spriteBatch);
                    spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
                }
                menu.DrawInGameMenu(spriteBatch);
            }

            else if (state == StateMachine.InGameOptions)
            {
                if (player.shop == false)
                {
                    DrawGame();
                }
                else
                {
                    spriteBatch.DrawString(font, "" + FPS, new Vector2(10, 10), Color.Black);
                    //Draw Background Layers
                    bg.Draw(spriteBatch);

                    level.Draw(spriteBatch, true);
                    player.Draw(spriteBatch, cursorPosition);
                    //clone.Draw(spriteBatch);
                    userI.Draw(spriteBatch, player);
                    gameScore.Draw(spriteBatch);
                    spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
                }
                menu.DrawInGameOptions(spriteBatch);
            }

            else if (state == StateMachine.GameOver) { menu.DrawGameOver(spriteBatch, font, player); }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Nick Greenquist and Dylan Coats
        private void NextLevel()
        {
            
            player.health = 5;
            player.playerScore.ScoreAmount += player.playerScore.LevelScoreAmount;
            player.playerScore.LevelScoreAmount = 0;
            levelNum++;
            level = new Level(player, Content, textbox, levelNum, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            level.ReadWorld("level" + levelNum + "/level" + levelNum + ".txt");
            level.ReadEnemies("level" + levelNum + "/guard" + levelNum + ".txt");
            level.ReadTurrets("level" + levelNum + "/turrets" + levelNum + ".txt");
            player.GameWorld = level;
            player.wonLevel = false;
            player.bullets.Clear();
            bg.NextLevel(Content);
            bg.currentPos = 0;
            bg.prevPos = 0;
            bg.changePos = 0;
            if (levelNum >= 5 && levelNum < 10)
            {
                sounds.Game = Content.Load<Song>("305 Dynamite Rider");
                MediaPlayer.Stop();
                sounds.PlayGame();
                if (levelNum == 5)
                {
                    sounds.PlayExplosion2();
                }
            }
            if (levelNum >= 10 && levelNum < 18)
            {
                sounds.Game = Content.Load<Song>("11. Trailblazer");
                MediaPlayer.Stop();
                sounds.PlayGame();
                if (levelNum == 12)
                {
                    sounds.PlayExplosion2();
                }
            }
            if (levelNum == 18)
            {
                sounds.Game = Content.Load<Song>("201 THE KING OF LEGEND");
                MediaPlayer.Stop();
                sounds.PlayGame();
                if (levelNum == 18)
                {
                    sounds.PlayExplosion2();
                }
            }
            if (levelNum >= 5 && levelNum < 10)
            {
                bg.fore = new ScrollingBackground(Content.Load<Texture2D>("Foreground 2"), new Rectangle(0, 0, 1200, 800));
                bg.mid = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 2"), new Rectangle(0, 0, 1200, 800));
                bg.back = new ScrollingBackground(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 1200, 800));
                bg.fore2 = new ScrollingBackground(Content.Load<Texture2D>("Foreground 2"), new Rectangle(1200, 0, 1200, 800));
                bg.mid2 = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 2"), new Rectangle(1200, 0, 1200, 800));
                bg.back2 = new ScrollingBackground(Content.Load<Texture2D>("Background"), new Rectangle(1200, 0, 1200, 800));
            }
            if (levelNum >= 10 && levelNum < 18)
            {
                bg.fore = new ScrollingBackground(Content.Load<Texture2D>("Foreground 3"), new Rectangle(0, 0, 1200, 800));
                bg.mid = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 3"), new Rectangle(0, 0, 1200, 800));
                bg.back = new ScrollingBackground(Content.Load<Texture2D>("Background2"), new Rectangle(0, 0, 1200, 800));
                bg.fore2 = new ScrollingBackground(Content.Load<Texture2D>("Foreground 3"), new Rectangle(1200, 0, 1200, 800));
                bg.mid2 = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 3"), new Rectangle(1200, 0, 1200, 800));
                bg.back2 = new ScrollingBackground(Content.Load<Texture2D>("Background2"), new Rectangle(1200, 0, 1200, 800));
            }
            if (levelNum == 18)
            {
                bg.fore = new ScrollingBackground(Content.Load<Texture2D>("Foreground 4"), new Rectangle(0, 0, 1200, 800));
                bg.mid = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 4"), new Rectangle(0, 0, 1200, 800));
                bg.back = new ScrollingBackground(Content.Load<Texture2D>("Background3"), new Rectangle(0, 0, 1200, 800));
                bg.fore2 = new ScrollingBackground(Content.Load<Texture2D>("Foreground 4"), new Rectangle(1200, 0, 1200, 800));
                bg.mid2 = new ScrollingBackground(Content.Load<Texture2D>("MiddleGround 4"), new Rectangle(1200, 0, 1200, 800));
                bg.back2 = new ScrollingBackground(Content.Load<Texture2D>("Background3"), new Rectangle(1200, 0, 1200, 800));
            }

            if (levelNum >= 10)
            {
                countDown.TimeLimit = 200;
            }
            else
            {
                countDown.TimeLimit = 100;
            }
            if (levelNum == 18)
            {
                countDown.TimeLimit = 1000;
            }
            
        }

        private void DrawGame()
        {
            spriteBatch.DrawString(font, "" + FPS, new Vector2(10, 10), Color.Black);
            //Draw Background Layers
            bg.Draw(spriteBatch);

            level.Draw(spriteBatch, false);
            player.Draw(spriteBatch, cursorPosition);
            //clone.Draw(spriteBatch);
            userI.Draw(spriteBatch, player);
            gameScore.Draw(spriteBatch);
            countDown.Draw(spriteBatch);
            spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
            spriteBatch.DrawString(countDown.TimeFont, "Level: " + levelNum, new Vector2(50, 50), Color.Black);
        }

        private void BonusLevel()
        {
                bg.mid = new ScrollingBackground(Content.Load<Texture2D>("wall1"), new Rectangle(0, 0, 1200, 800));
                bg.mid2 = new ScrollingBackground(Content.Load<Texture2D>("wall2"), new Rectangle(0, 0, 1200, 800));
                sounds.Game = Content.Load<Song>("221_electronical_parade");
                MediaPlayer.Stop();
                sounds.PlayGame();
            player.playerScore.ScoreAmount += player.playerScore.LevelScoreAmount;
            player.playerScore.LevelScoreAmount = 0;
            //levelNum++;
            level = new Level(player, Content, textbox, 19, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            level.levelNum = 19;
            level.ReadWorld("level" + levelNum + "/level" + levelNum + ".txt");
            level.ReadEnemies("level" + levelNum + "/guard" + levelNum + ".txt");
            level.ReadTurrets("level" + levelNum + "/turrets" + levelNum + ".txt");
            player.GameWorld = level;
            player.wonLevel = false;
            player.bullets.Clear();
            bg.NextLevel(Content);
            bg.currentPos = 0;
            bg.prevPos = 0;
            bg.changePos = 0;
            player.powerUpTimerDuartionTemp = player.powerUpTimerDuration;
            player.powerUpTimerDuration = .01;
            player.bazookaSpeedTemp = player.bazookaSpeed;
            player.bazookaSpeed = .1;
            player.bazookaSpread = 15;
        }

    }
}

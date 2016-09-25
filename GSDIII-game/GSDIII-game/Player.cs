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
    public enum PlayerState
    {
        faceLeft, 
        faceRight,
        runLeft,
        runRight
    }

    //Nick Greenquist
    public class Player
    {
        //lots and lots of attributes

        public bool bonusLevel;
        Texture2D playerSprite;
        Rectangle playerPos;
        Rectangle playerSpriteSheet;
        Rectangle playerPrevPos;
        public Sword playerSword;
        public PowerBurst shield;
        public Gun playerGun;
        bool onGround;
        bool isJumping;
        bool isCollide;
        bool isOnGround;
        public PlayerState playerState;
        public int playerSpeed;
        public int playerSpeedOrig;
        int jumpSpeed;
        int jumpSpeedOrig;
        double gravity;
        double gravityFall;
        public bool hasSword;
        public bool swordActive;
        public bool isDead;
        public bool hasGun;
        public bool hasShield;
        public bool shieldActive;
        public bool isClone;
        public bool hasRay;
        public double deathAnimationTime;
        public double shootSpeed;
        public bool shooting;
        public Color color;
        public int capsuleTime;
        public Score playerScore;
        public List<Bullet> bullets;
        double bulletTimeCounter;
        public double powerUpTimer;
        public double powerUpTimerDuration;
        public double powerUpTimerDuartionTemp;
        public int life;
        Sound sounds;
        public bool wonLevel;
        Level gameWorld;
        const int JUMP_FRAME_COUNT = 7;
        const int WALK_FRAME_COUNT = 2;
        const int PLAYER_RECT_Y_OFFSET = 90;
        public const int PLAYER_RECT_HEIGHT = 88;
        const int PLAYER_RECT_WIDTH = 88;
        const float MAX_JUMP_TIME = 8f;
        int groundY;
        double jumpTime;
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        KeyboardState kState;
        MouseState mState;
        Vector2 bulletMovement;
        public Vector2 cursorPosition;
        public List<bool> gunList;
        public bool shop;

        //gun damages
        public int pistolDamage;
        public int shotgunDamage;
        public int machinegunDamage;
        public int bazookaDamage;

        //indicidual guns stats, read from, external tool
        public double pistolSpeed;
        public int pistolSpread;
        public double shotgunSpeed;
        public int shotgunSpread;
        public double machinegunSpeed;
        public int machinegunSpread;
        public double bazookaSpeed;
        public int bazookaSpread;

        int temp;
        public int hasArmor;

        public Textbox textbox;
        public bool inTextBox;
        public int health;
        public double bazookaSpeedTemp;
        public bool cascioli;
        public bool startChrisSong;
       
        //properties
        public Texture2D PlayerSprite
        {
            get { return playerSprite; }
            set { playerSprite = value; }
        }
        public Rectangle PlayerPos
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        public int X
        {
            get { return playerPos.X; }
            set { playerPos.X = value; }
        }
        public int Y
        {
            get { return playerPos.Y; }
            set { playerPos.Y = value; }
        }
        public Rectangle PlayerPrevPos
        {
            get { return playerPrevPos; }
            set { playerPrevPos = value; }
        }
        public bool OnGround
        {
            get { return onGround; }
        }
        public bool IsJumping
        {
            get { return isJumping; }
        }
        public Level GameWorld
        {
            get { return gameWorld; }
            set { gameWorld = value; }
        }
        public int GroundY
        {
            get { return groundY; }
            set { groundY = value - ((int)(1.5 * PLAYER_RECT_HEIGHT)); }
        }
        public int PlayerSpeed
        {
            get { return playerSpeed; }
            set { playerSpeed = value; }
        }
        public int JumpSpeed
        {
            get { return jumpSpeed; }
            set { jumpSpeed = value; }
        }
        public Boolean HasSword
        {
            get { return hasSword; }
        }

        public Boolean HasGun
        {
            get { return hasGun; }
        }

        public int PlayerScore
        {
            get { return playerScore.ScoreAmount; }

            set { playerScore.ScoreAmount = value; }

        }

        //Constructor - sets a lot of player's variables up. The rest is done in ReadPlayer()
        //Nick Greenquist
        public Player(int x, int y, int w, int h, Score score, Textbox txtBox, ContentManager pC, Game1.StateMachine pS)
        {
            playerPos = new Rectangle(x, y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
            playerSpriteSheet = new Rectangle(PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
            onGround = true;
            isJumping = false;          
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            playerState = PlayerState.faceRight;          
            hasSword = false;
            hasShield = false;
            hasGun = false;
            hasArmor = 0;
            shieldActive = false;
            isDead = false;
            playerScore = score;
            playerScore.ScoreAmount = 0;
            deathAnimationTime = 0;
            color = Color.White;
            capsuleTime = 0;
            isClone = false;
            bullets = new List<Bullet>();
            sounds = new Sound(pC, pS);
            life = 3;
            wonLevel = false;
            powerUpTimer = 0;
            gunList = new List<bool>();
            shop = false;
            temp = 0;
            powerUpTimerDuration = 2;

            for (int i = 0; i < 4; i++)
            {
                gunList.Add(false);
            }

            pistolDamage = 2;
            shotgunDamage = 1;
            machinegunDamage = 1;
            bazookaDamage = 10;
            pistolSpeed = .6;
            pistolSpread = 1;
            shotgunSpeed = .8;
            shotgunSpread = 5;
            machinegunSpeed = .1;
            machinegunSpread = 1;
            bazookaSpeed = 1;
            bazookaSpread = 1;

            textbox = txtBox;

            //read values from txt file
            ReadPlayer();
            inTextBox = false;
            health = 5;
            cascioli = false;
            startChrisSong = false;
        }

        //Nick Greenquist - updates the player
        public void Update(GameTime gameTime,Vector2 mousePosition)
        {
            //if (playerPos.Y < 0)
            //{
            //    gameWorld.scrollWorldY(660);
            //    playerPos.Y = 660;
            //}
            //if (playerPos.Y > 660)
            //{
            //    gameWorld.scrollWorldY(-660);
            //    playerPos.Y = 0;
            //}


            cursorPosition = mousePosition; //get the most recent cursor position, needed for the player to shoot bullets
            if (!isDead) //player can't do much id he is dead
            {
                // Handle animation timing
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCounter >= timePerFrame)
                {
                    frame += 1;						// Adjust the framex
                    if (frame > WALK_FRAME_COUNT)	// Check the bounds
                        frame = 1;					// Back to 1 (since 0 is the "standing" frame)

                    if (isJumping)
                    {
                        frame = 5;
                    }
                    timeCounter -= timePerFrame;	// Remove the time we "used"
                }

                //get a copy of player's previous position
                playerPrevPos = playerPos;
                ProcessInput();
                DoPhysicsStuff();

                //check collisions once more after doing physics in case anything messed up
                CheckCollisions();
                if (isCollide) //if something messed up, this will fix it
                {
                    playerPos.Y = playerPrevPos.Y; //revert his position back to where it was before moving him            
                }
                CheckEnemyCollision();

                
                //deal with any things the player has(weapons, powerups, etc.)
                if (shieldActive)
                {
                    
                    shield.Update(gameTime);
                    if (temp == 0)
                    {
                        sounds.PlayBurstSound();
                    }
                    temp++;
                    if (temp == 30)
                    {
                        temp = 0;
                    }
                }
                if (swordActive)
                {
                    playerSword.Update(gameTime);
                }
                if (hasGun)
                {
                    playerGun.Update(this, mousePosition);

                    bulletTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;//even if he is not shooting, he can still be reloading

                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        if (hasRay) { }
                        else
                        {
                            FireBullets();
                        }
                    }
                }
                //move bullets
                for (int i = 0; i < bullets.Count; i++)
                {
                    
                    bullets[i].MoveBullet(gameTime);
                }
            }

            //now, if he is dead, play this half second animation
            if (isDead && deathAnimationTime < 30)
            {
                sounds.PlayDie();
                deathAnimationTime++;
            }

            if (playerScore.ScoreAmount < 0)
            {
                playerScore.ScoreAmount = 0;
            }
            if (hasRay)
            {
                shieldActive = false;
            }
        }

        //Nick Greenquist and Dylan Coats - draws the player sprite
        public void Draw(SpriteBatch s,Vector2 mousePosition)
        {
            //he turns red if he is dead and the animation timer hasnt run out yet
            if (isDead && deathAnimationTime < 29)
            {
                color = Color.Red;
            }

            //draw the shield first(behind the player)
            if (shieldActive && hasShield)
            {
                shield.Draw(s, this);
            }

            //draw player depending on his state
            switch (playerState)
            {
                case PlayerState.faceRight:
                    if (isJumping)
                    {
                        playerSpriteSheet = new Rectangle(PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    else
                    {
                        playerSpriteSheet = new Rectangle(0, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    break;
                case PlayerState.faceLeft:
                    if (isJumping)
                    {
                        playerSpriteSheet = new Rectangle(PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);

                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    else
                    {
                        playerSpriteSheet = new Rectangle(0, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    break;
                case PlayerState.runRight:
                    if (isJumping)
                    {
                        playerSpriteSheet = new Rectangle(PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    else
                    {
                        playerSpriteSheet = new Rectangle(frame * PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    break;
                case PlayerState.runLeft:
                    if (isJumping)
                    {
                        playerSpriteSheet = new Rectangle(PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    else
                    {
                        playerSpriteSheet = new Rectangle(frame * PLAYER_RECT_WIDTH, PLAYER_RECT_Y_OFFSET, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        if (hasRay)
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH / 2, PLAYER_RECT_HEIGHT / 2);
                        }
                        else
                        {
                            s.Draw(playerSprite, new Vector2(X, Y), playerSpriteSheet, color);
                            playerPos = new Rectangle(X, Y, PLAYER_RECT_WIDTH, PLAYER_RECT_HEIGHT);
                        }
                    }
                    break;
                default:
                    break;
            }
            //draw weapons over the player(in front of him)
            if (hasSword == true)
            {
                if (hasRay) { }
                else
                {
                    if (mState.MiddleButton == ButtonState.Pressed)
                    {
                        playerSword.Draw(s, this);
                    }
                }
            }
            if (hasGun)
            {

                if (hasRay) { }
                else
                {
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].PlayerDraw(s);
                    }
                    playerGun.Draw(s, this);
                }
            }
            
            
        }

        //Nick Greenquist
        public void ProcessInput()
        {
            //Gets the current state of the keyboard
            kState = Keyboard.GetState();
            mState = Mouse.GetState();

            switch (playerState)
            {
                case PlayerState.faceLeft:
                    if (kState.IsKeyDown(Keys.D)) {playerState = PlayerState.faceRight; }
                    if (kState.IsKeyDown(Keys.A)) { playerState = PlayerState.runLeft; }
                    if (kState.IsKeyDown(Keys.W)) { isJumping = true;}
                    break;

                case PlayerState.faceRight:
                    if (kState.IsKeyDown(Keys.A)) {playerState = PlayerState.faceLeft; }
                    if (kState.IsKeyDown(Keys.D)) { playerState = PlayerState.runRight; }
                    if (kState.IsKeyDown(Keys.W)) { isJumping = true; }
                    break;
                case PlayerState.runLeft:                   
                    if (kState.IsKeyDown(Keys.W))      { isJumping = true; }
                    if (kState.IsKeyDown(Keys.A)) { 
                        gameWorld.MoveTiles(playerSpeed);
                        playerState = PlayerState.runLeft;
                    }
                    else { playerState = PlayerState.faceLeft; }
                    break;

                case PlayerState.runRight:                   
                    if (kState.IsKeyDown(Keys.W)) { isJumping = true; }
                    if (kState.IsKeyDown(Keys.D)) {
                        gameWorld.MoveTiles(-1 * playerSpeed);
                        playerState = PlayerState.runRight;
                    }
                    else { playerState = PlayerState.faceRight; 
                    }
                    break;
                default:
                    break;
            }

            //deal with input that either fires weapons or activates powerups
            if (hasShield)
            {
                //Tim Cotanch
                if (hasRay) { }
                else
                {


                    if (mState.RightButton == ButtonState.Pressed)
                    {
                        powerUpTimer -= powerUpTimerDuration;
                        if (powerUpTimer > 0)
                        {
                            shieldActive = true;
                        }
                        else
                        {
                            shieldActive = false;
                        }
                        if (powerUpTimer < -120)
                        {
                            powerUpTimer = -120;
                        }
                    }
                    else if (mState.RightButton != ButtonState.Pressed)
                    {
                        powerUpTimer += 2;
                        if (powerUpTimer >= 240)
                        {
                            powerUpTimer = 240;
                        }
                        shield.frame = 0;
                        shieldActive = false;
                    }
                }
            }
            if (hasSword)
            {
                if (hasRay) { }
                else
                {
                    if (mState.MiddleButton == ButtonState.Pressed)
                    {
                        swordActive = true;
                        playerGun.gunColor = new Color(0, 0, 0, 0);
                    }
                    if (mState.MiddleButton != ButtonState.Pressed)
                    {
                        playerSword.frame = 0;
                        swordActive = false;
                        playerGun.gunColor = Color.White;
                    }
                }
            }

            //switching weapons
            if (!(bullets.Count > 0) && hasGun)
            {
                if (kState.IsKeyDown(Keys.D1) && gunList[0]) { playerGun = new Gun(gameWorld.content, "pistol", this); gameWorld.explosions.Clear(); gameWorld.explosionsTimer.Clear(); gameWorld.explosionAOE.Clear(); }
                if (kState.IsKeyDown(Keys.D2) && gunList[1]) { playerGun = new Gun(gameWorld.content, "shotgun", this); gameWorld.explosions.Clear(); gameWorld.explosionsTimer.Clear(); gameWorld.explosionAOE.Clear(); }
                if (kState.IsKeyDown(Keys.D3) && gunList[2]) { playerGun = new Gun(gameWorld.content, "machinegun", this); gameWorld.explosions.Clear(); gameWorld.explosionsTimer.Clear(); gameWorld.explosionAOE.Clear(); }
                if (kState.IsKeyDown(Keys.D4) && gunList[3]) { playerGun = new Gun(gameWorld.content, "bazooka", this); gameWorld.explosions.Clear(); gameWorld.explosionsTimer.Clear(); gameWorld.explosionAOE.Clear(); }
            }

            //cheat commands, comment out for final game
            if(kState.IsKeyDown(Keys.L)) { life = 3; }
            if (kState.IsKeyDown(Keys.K)) { playerScore.ScoreAmount +=100; }
            if (kState.IsKeyDown(Keys.B)) 
            {
                powerUpTimerDuration = .001; 
            }
            if (kState.IsKeyDown(Keys.V))
            {  
                powerUpTimerDuration = 2;
            }
            
        }

        //Nick Greenquist - this method checks player's collision against all of the game's tiles
        //use a square tree if we have time but game is smooth so far
        public void CheckCollisions()
        {
            isCollide = false;
            inTextBox = false;
            gameWorld.textbox.signCollision = false;
            Rectangle playerTempPos = new Rectangle(playerPos.X + 15, playerPos.Y + 8, playerPos.Width - 30, playerPos.Height - 8); //give him a skinnier collision hitbox

            int startX = 540;
            startX += gameWorld.totalDistanceMoved;
            startX = startX / 60;

            //check movable tile collision
            for (int i = 0; i < gameWorld.tilesMovedWithButtons.Count; i++)
            {
                //bottom half of rectangle
                if (playerTempPos.Intersects(new Rectangle(gameWorld.tilesMovedWithButtons[i].TileRect.X + playerSpeed, gameWorld.tilesMovedWithButtons[i].TileRect.Y + (Tile.tileHeight / 2), gameWorld.tilesMovedWithButtons[i].TileRect.Width - (2 * playerSpeed), gameWorld.tilesMovedWithButtons[i].TileRect.Height / 2)))
                {
                    //he hit the bottom of a tile so he "hit his head"
                    isOnGround = false;
                    isJumping = true;
                    jumpTime = MAX_JUMP_TIME; //makes him start falling back down
                    isCollide = true;
                }
                //top half of rectangle
                if (playerTempPos.Intersects(new Rectangle(gameWorld.tilesMovedWithButtons[i].TileRect.X + playerSpeed, gameWorld.tilesMovedWithButtons[i].TileRect.Y, gameWorld.tilesMovedWithButtons[i].TileRect.Width - (2 * playerSpeed), gameWorld.tilesMovedWithButtons[i].TileRect.Height / 2)))
                {
                    //he landed on a tile so he can jump again, revert jump time
                    isOnGround = true;
                    jumpTime = 0;
                    isJumping = false;
                    isCollide = true;
                    gravity = 1;
                }

                //collides with samll portions of the sides of the rectangles - left
                if (playerTempPos.Intersects(new Rectangle(gameWorld.tilesMovedWithButtons[i].TileRect.X, gameWorld.tilesMovedWithButtons[i].TileRect.Y + jumpSpeed, playerSpeed, gameWorld.tilesMovedWithButtons[i].TileRect.Height - 10)))
                {
                    gameWorld.MoveTiles(playerSpeed);
                    isCollide = true;
                }
                //collides with samll portions of the sides of the rectangles - right
                if (playerTempPos.Intersects(new Rectangle(gameWorld.tilesMovedWithButtons[i].TileRect.X + gameWorld.tilesMovedWithButtons[i].TileRect.Width - playerSpeed, gameWorld.tilesMovedWithButtons[i].TileRect.Y + jumpSpeed, playerSpeed, gameWorld.tilesMovedWithButtons[i].TileRect.Height - 10)))
                {
                    gameWorld.MoveTiles(-1 * playerSpeed);
                    isCollide = true;
                }
            }

            for (int i = 0; i < gameWorld.levelHeight; i++)
            {
                for (int j = 0; j < (startX + 5); j++)
                {
                    if (gameWorld.tiles[i, j].FileName.Equals("Door") && playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect)) //should he move on to the next level?
                    {
                        sounds.PlayDoor();
                        wonLevel = true;
                        if (gameWorld.textbox.SignTexts != null && gameWorld.textbox.SignTexts.Count > 0)
                        {
                            gameWorld.textbox.SignTexts.Clear();
                        }
                        if (gameWorld.textbox.SignPairs != null && gameWorld.textbox.SignPairs.Count > 0)
                        {
                            gameWorld.textbox.SignPairs.Clear();
                        }
                        if (gameWorld.signsInDictionary != null && gameWorld.signsInDictionary.Count > 0)
                        {
                            gameWorld.signsInDictionary.Clear();
                        }
                        break;
                    }
                    else if (gameWorld.tiles[i, j].FileName.Equals("Bonus") && playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect)) //advance to bonus level
                    {
                        if (gameWorld.textbox.SignTexts != null && gameWorld.textbox.SignTexts.Count > 0)
                        {
                            gameWorld.textbox.SignTexts.Clear();
                        }
                        if (gameWorld.textbox.SignPairs != null && gameWorld.textbox.SignPairs.Count > 0)
                        {
                            gameWorld.textbox.SignPairs.Clear();
                        }
                        if (gameWorld.signsInDictionary != null && gameWorld.signsInDictionary.Count > 0)
                        {
                            gameWorld.signsInDictionary.Clear();
                        }
                        sounds.PlayDoor();
                        bonusLevel = true;
                        break;
                    }
                    else if (gameWorld.tiles[i, j].FileName.Equals("EndBonus") && playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect)) //advance to bonus level
                    {
                        if (gameWorld.textbox.SignTexts != null && gameWorld.textbox.SignTexts.Count > 0)
                        {
                            gameWorld.textbox.SignTexts.Clear();
                        }
                        if (gameWorld.textbox.SignPairs != null && gameWorld.textbox.SignPairs.Count > 0)
                        {
                            gameWorld.textbox.SignPairs.Clear();
                        }
                        if (gameWorld.signsInDictionary != null && gameWorld.signsInDictionary.Count > 0)
                        {
                            gameWorld.signsInDictionary.Clear();
                        }
                        sounds.PlayDoor();
                        powerUpTimerDuration = powerUpTimerDuartionTemp;
                        bazookaSpeed = bazookaSpeedTemp;
                        bazookaSpread = 1;
                        wonLevel = true;
                        break;
                    }
                    else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Shop") && gameWorld.Tiles[i, j].TileTexture != null) //gun pickup
                    {
                        playerScore.ScoreAmount += playerScore.LevelScoreAmount;
                        playerScore.LevelScoreAmount = 0;
                        sounds.PlayDoor();
                        shop = true;
                    }
                    else if (gameWorld.Tiles[i, j].FileName.Equals("Revert") && playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect))
                    {
                        hasRay = false;
                    }
                    //Eric Kipnis
                    else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Coin") && gameWorld.Tiles[i, j].TileTexture != null ) //coin?
                    {
                        gameWorld.Tiles[i, j].type = TileType.PassThrough;
                        gameWorld.Tiles[i, j].fileName = "";
                        gameWorld.Tiles[i, j].TileTexture = gameWorld.content.Load<Texture2D>("chest4");
                        playerScore.LevelScoreAmount++;
                        sounds.PlayCoin();
                        break;
                    }
                    if (gameWorld.Tiles[i, j].type == TileType.NonPassable)//only deal with tiles that are not passable (its a Tile state)
                    {
                        //Tim Cotanch
                        if (gameWorld.Tiles[i, j].FileName.Equals("Spike") && playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect)) //spike?
                        {
                            Die();
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Sword") && gameWorld.Tiles[i, j].TileTexture != null) //sword pickup
                        {
                            gameWorld.Tiles[i, j].type = TileType.PassThrough;
                            gameWorld.Tiles[i, j].TileTexture = null;
                            hasSword = true;
                            sounds.PlaySwordPower();
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Pistol") && gameWorld.Tiles[i, j].TileTexture != null) //gun pickup
                        {
                            gameWorld.Tiles[i, j].type = TileType.PassThrough;
                            gameWorld.Tiles[i, j].TileTexture = null;
                            hasGun = true;
                            playerGun = new Gun(gameWorld.content, "pistol", this);
                            gunList[0] = true;
                            sounds.PlayGunPickup();
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Shotgun") && gameWorld.Tiles[i, j].TileTexture != null) //gun pickup
                        {
                            gameWorld.Tiles[i, j].type = TileType.PassThrough;
                            gameWorld.Tiles[i, j].TileTexture = null;
                            hasGun = true;
                            playerGun = new Gun(gameWorld.content, "shotgun", this);
                            gunList[1] = true;
                            sounds.PlayGunPickup();
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Machinegun") && gameWorld.Tiles[i, j].TileTexture != null) //gun pickup
                        {
                            gameWorld.Tiles[i, j].type = TileType.PassThrough;
                            gameWorld.Tiles[i, j].TileTexture = null;
                            hasGun = true;
                            playerGun = new Gun(gameWorld.content, "machinegun", this);
                            gunList[2] = true;
                            sounds.PlayGunPickup();
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("Bazooka") && gameWorld.Tiles[i, j].TileTexture != null) //bazooka pickup
                        {
                            gameWorld.Tiles[i, j].type = TileType.PassThrough;
                            gameWorld.Tiles[i, j].TileTexture = null;
                            hasGun = true;
                            playerGun = new Gun(gameWorld.content, "bazooka", this);
                            gunList[3] = true;
                            sounds.PlayGunPickup();
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("PowerBurst") && gameWorld.Tiles[i, j].TileTexture != null) //power burst pickup
                        {
                            capsuleTime++;
                            gameWorld.tiles[i, j].TileTexture = gameWorld.content.Load<Texture2D>("Capsule Full");
                            //these lines make the gun and player invisible for the animation duration
                            color = new Color(0, 0, 0, 0);
                            playerGun.gunColor = new Color(0, 0, 0, 0);
                            playerSpeed = 0;

                            //ends a cool little capsule animation
                            if (capsuleTime > 100) //reset everything to make things right again
                            {
                                playerSpeed = playerSpeedOrig;
                                gameWorld.Tiles[i, j].type = TileType.PassThrough;
                                gameWorld.tiles[i, j].tileColor = Color.Red;
                                gameWorld.Tiles[i, j].TileTexture = gameWorld.content.Load<Texture2D>("Capsule");
                                hasShield = true;
                                color = Color.White;
                                capsuleTime = 0;
                                sounds.PlayBurstPower();
                                playerGun.gunColor = Color.White;
                            }
                            break;
                        }
                        else if (playerTempPos.Intersects(gameWorld.tiles[i, j].TileRect) && gameWorld.Tiles[i, j].FileName.Equals("ShrinkRay") && gameWorld.Tiles[i, j].TileTexture != null) //shrink ray
                        {
                            capsuleTime++;
                            gameWorld.tiles[i, j].TileTexture = gameWorld.content.Load<Texture2D>("Capsule Full2");
                            color = new Color(0, 0, 0, 0);
                            playerGun.gunColor = new Color(0, 0, 0, 0);
                            playerSpeed = 0;

                            if (capsuleTime > 100)
                            {
                                playerSpeed = playerSpeedOrig;
                                gameWorld.Tiles[i, j].type = TileType.PassThrough;
                                gameWorld.tiles[i, j].tileColor = Color.Red;
                                gameWorld.Tiles[i, j].TileTexture = gameWorld.content.Load<Texture2D>("Capsule");
                                color = Color.White;
                                hasRay = true;
                                capsuleTime = 0;
                                sounds.PlayShrink();
                                playerGun.gunColor = Color.White;
                            }
                            break;
                        }
                        //Nick Greenquist - Collision with Normal Tiles, they are split up in order for the game to know if he istanding on tiles, face planting into to them, etc.
                        //bottom half of rectangle
                        if (playerTempPos.Intersects(new Rectangle(gameWorld.Tiles[i, j].TileRect.X + playerSpeed, gameWorld.Tiles[i, j].TileRect.Y + (Tile.tileHeight / 2), gameWorld.Tiles[i, j].TileRect.Width - (2 * playerSpeed), gameWorld.Tiles[i, j].TileRect.Height / 2)))
                        {
                            //he hit the bottom of a tile so he "hit his head"
                            isOnGround = false;
                            isJumping = true;
                            jumpTime = MAX_JUMP_TIME; //makes him start falling back down
                            isCollide = true;
                            break;
                        }
                        //top half of rectangle
                        if (playerTempPos.Intersects(new Rectangle(gameWorld.Tiles[i, j].TileRect.X + playerSpeed, gameWorld.Tiles[i, j].TileRect.Y, gameWorld.Tiles[i, j].TileRect.Width - (2 * playerSpeed), gameWorld.Tiles[i, j].TileRect.Height / 2)))
                        {
                            //he landed on a tile so he can jump again, revert jump time
                            isOnGround = true;
                            jumpTime = 0;
                            isJumping = false;
                            isCollide = true;
                            gravity = 1;
                            break;
                        }

                        //collides with samll portions of the sides of the rectangles - left
                        if (playerTempPos.Intersects(new Rectangle(gameWorld.Tiles[i, j].TileRect.X, gameWorld.Tiles[i, j].TileRect.Y + jumpSpeed, playerSpeed, gameWorld.Tiles[i, j].TileRect.Height - 10)))
                        {
                            gameWorld.MoveTiles(playerSpeed);
                            isCollide = true;
                            break;
                        }
                        //collides with samll portions of the sides of the rectangles - right
                        if (playerTempPos.Intersects(new Rectangle(gameWorld.Tiles[i, j].TileRect.X + gameWorld.Tiles[i, j].TileRect.Width - playerSpeed, gameWorld.Tiles[i, j].TileRect.Y + jumpSpeed, playerSpeed, gameWorld.Tiles[i, j].TileRect.Height - 10)))
                        {
                            gameWorld.MoveTiles(-1 * playerSpeed);
                            isCollide = true;
                            break;
                        }
                    } 
                }
            }

            

            for (int i = 0; i < gameWorld.signs.Count; i++)
            {
                if (playerTempPos.Intersects(gameWorld.signs[i].SignPosition))
                {
                    textbox.ChangeText(i + 1);
                    textbox.signCollision = true;
                    inTextBox = true;
                }
            }
        }

        //Nick Greenquist - checks enemy collision
        public void CheckEnemyCollision()
        {
            Rectangle playerTempPos = new Rectangle(playerPos.X + 15, playerPos.Y + 8, playerPos.Width - 30, playerPos.Height - 8); //give him a smaller hit box

            //player vs enemy
            for (int i = 0; i < gameWorld.guards.Count; i++)
            {
                if (playerTempPos.Intersects(gameWorld.guards[i].EnemyPos) && gameWorld.guards[i].isDead == false) //player hits guard
                {
                    Die();
                }
                if (playerSword.swordPos.Intersects(gameWorld.guards[i].EnemyPos) && swordActive) //sword hits guard
                {
                    if (gameWorld.guards[i].isDead == false)
                    {
                        sounds.PlaySwordAttack();
                        if (gameWorld.guards[i].superGuard.Equals("superguard"))
                        {
                            playerScore.ScoreAmount += 5;
                        }
                        else
                        {
                            playerScore.ScoreAmount += 2;
                        }
                    }
                    gameWorld.guards[i].isDead = true;
                }
            }
            //Boss collision
            if (playerTempPos.Intersects(gameWorld.boss.EnemyPos) && gameWorld.boss.isDead == false) //player hits boss
            {
                Die();
            }
            //if (playerSword.swordPos.Intersects(gameWorld.boss.EnemyPos) && swordActive) //sword hits boss
            //{
            //    if (gameWorld.boss.isDead == false)
            //    {
            //        sounds.PlaySwordAttack();
            //    }
            //    gameWorld.boss.health -= 2;
            //    if (gameWorld.boss.health <= 0)
            //    {
            //        gameWorld.boss.isDead = true;
            //    }
            //}

            for (int i = 0; i < gameWorld.turrets.Count; i++)
            {
                if (playerTempPos.Intersects(gameWorld.turrets[i].EnemyPos) && gameWorld.turrets[i].isDead == false) //player hits turret
                {
                    Die();
                }
                if (playerSword.swordPos.Intersects(gameWorld.turrets[i].EnemyPos) && swordActive) //sword hits turret
                {
                    if (gameWorld.turrets[i].isDead == false)
                    {
                        sounds.PlaySwordAttack();
                        playerScore.ScoreAmount++;
                    }
                    gameWorld.turrets[i].isDead = true;
                }
            }
            //check collisions with the flames 
            for (int i = 0; i < gameWorld.flameTiles.Count; i++)
            {
                if (playerTempPos.Intersects(gameWorld.flameTiles[i].flames.flameRect) && gameWorld.flameTiles[i].flames.Active)
                {
                    Die();
                }
            }
        }

        //Nick Greenquist - physics code
        public void DoPhysicsStuff()
        {
            CheckCollisions(); //check collisions again to get the most recent "physics state" of the player

            //if he is above ground level, he is not on the ground
            if (Y < groundY - jumpSpeed)
            {
                isOnGround = false;
            }


            //falling code
            if (!isOnGround && !isJumping)
            {
                //code for falling here, might be a little slow later on
                gravityFall += .023; //change this to make gravity effect more apparent
                jumpSpeed = (int)(jumpSpeedOrig * gravityFall);
                Y += jumpSpeed;

                CheckCollisions(); //unfortunately we have to check collisions AGAIN in case something went wrong
                if (isCollide)
                {
                    Y -= jumpSpeed;
                    isOnGround = true;
                    gravityFall = .5;
                    jumpSpeed = jumpSpeedOrig;
                }
                else
                {
                    jumpTime = MAX_JUMP_TIME;
                } //so he can't jump after falling off a platform
            }

            //a check to make sure he didn't fall too much - this code could probably be taken out
            if (Y >= groundY)
            {
                Y = groundY;
                isOnGround = true;
            }

            //make him jmup
            if (isJumping)
            {
                frame = 0;
                if (jumpTime < MAX_JUMP_TIME / 2)
                {
                    gravity -= .023;
                    jumpSpeed = (int)(jumpSpeedOrig * gravity);
                    Y -= jumpSpeed;
                    jumpTime += timePerFrame;
                }
                else if (jumpTime > MAX_JUMP_TIME / 2)
                {
                    gravity += .023;
                    jumpSpeed = (int)(jumpSpeedOrig * gravity);
                    Y += jumpSpeed;
                    jumpTime += timePerFrame;
                }
                if (isOnGround) //if he is on ground level, he can't be jumping
                {
                    isJumping = false;
                    gravity = 1;
                    jumpTime = 0;
                }
            }
            else
            {
                gravity = 1;
            }
        }

        //Nick Greenquist - read player attributes from playerParam.txt
        public void ReadPlayer()
        {
            //read in data froma file and set player variables
            StreamReader reader = new StreamReader("playerParam.txt");
            string line = "";

            line = reader.ReadLine();
            playerSpeed = Int32.Parse(line);
            playerSpeedOrig = playerSpeed;

            line = reader.ReadLine();
            jumpSpeed = Int32.Parse(line);
            jumpSpeedOrig = jumpSpeed;

            line = reader.ReadLine();
            gravity = Double.Parse(line);
            gravityFall = gravity / 2;

            reader.Close();
        }

        //Nick Greenquist
        //anything that changes when he dies - lose weapons? lose money? goes in here
        public void Die()
        {
            if (!isDead)
            {
                playerScore.ScoreAmount -= 5;
                life--;
            }
            isDead = true;
            hasRay = false;
            bullets.Clear();
            gameWorld.textbox.SignPairs.Clear();
            gameWorld.signsInDictionary.Clear();
        }

        //Nick Greenquist
        public void FireBullets()
        {
            int yOffset = 20;
            bool add = true;
            bool sub = false;
            Vector2 bulletShootTowards = cursorPosition;

             if (bulletTimeCounter >= playerGun.shootSpeed || bulletTimeCounter == 0) //how fast he shoots shit, can always shoot once at first right away
             {
                 bulletTimeCounter = 0;
                 if (!(playerGun.rotationAngleOrig > 1 || playerGun.rotationAngleOrig < -.85))
                 {
                     if ((playerState == PlayerState.faceLeft || playerState == PlayerState.runLeft) && cursorPosition.X < playerPos.X)
                     {
                         for (int i = 0; i < playerGun.bulletsPerShot; i++)
                         {
                             bullets.Add(new Bullet(playerGun.ammoSprite, playerPos.X + (playerPos.Width / 2), playerPos.Y + (playerPos.Height / 2), 10, bulletShootTowards, playerGun.ammoType,true));
                             //this is code for bullet spread, probably more complicated than it could be
                             if (add)
                             {
                                 bulletShootTowards.Y = cursorPosition.Y + yOffset;
                             }
                             if (sub)
                             {
                                 bulletShootTowards.Y = cursorPosition.Y - yOffset;  
                             }
                             add = !add;
                             sub = !sub;
                             if ((i + 1) % 2 == 0)
                             {
                                 yOffset += 20;
                             }
                         }
                         if (playerGun.gunName == "bazooka")
                         {
                             sounds.PlayBazookaLaunch();
                         }
                         else { sounds.PlayGunShoot(); }
                     }
                     if ((playerState == PlayerState.faceRight || playerState == PlayerState.runRight) && cursorPosition.X > playerPos.X)
                     {
                         for (int i = 0; i < playerGun.bulletsPerShot; i++)
                         {
                             bullets.Add(new Bullet(playerGun.ammoSprite, playerPos.X + (playerPos.Width / 2), playerPos.Y + (playerPos.Height / 2), 10, bulletShootTowards, playerGun.ammoType,false));
                             //this is code for bullet spread, probably more complicated than it could be
                             if (add)
                             {
                                 bulletShootTowards.Y = cursorPosition.Y + yOffset;
                             }
                             if (sub)
                             {
                                 bulletShootTowards.Y = cursorPosition.Y - yOffset;
                             }
                             add = !add;
                             sub = !sub;
                             if ((i + 1) % 2 == 0)
                             {
                                 yOffset += 20;
                             }
                         }
                         if (playerGun.gunName == "bazooka")
                         {
                             sounds.PlayBazookaLaunch();
                         }
                         else { sounds.PlayGunShoot(); }
                     }
                     
                 }
             }

             
        }
    }
}
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
    //Dylan Coats
    public class UI
    {
        Texture2D UIBar;
        Texture2D lives;
        Texture2D energyBar;
        Texture2D healthBar;
        Texture2D energyBarOutside;
        Texture2D armor;
        Rectangle lifeSpriteSheet;
        public Rectangle energyBarRec;
        public Rectangle healthBarRec;
        Player p;
        int flash;

        public UI(ContentManager c, Player pP)
        {
            UIBar = c.Load<Texture2D>("UI");
            lives = c.Load<Texture2D>("Lives");
            energyBarOutside = c.Load<Texture2D>("BurstMeter");
            energyBar = c.Load<Texture2D>("BurstMeterInside");
            healthBar = c.Load<Texture2D>("greenBox");
            energyBarRec = new Rectangle(130, 10, 240, 20);
            healthBarRec = new Rectangle(129, 31, 20, 16);
            armor = c.Load<Texture2D>("Armor");
            p = pP;
            flash = 0;
        }

        //Nick Greenquist
        public void Draw(SpriteBatch s, Player p)
        {

            s.Draw(UIBar, new Rectangle(0, 0, UIBar.Width + 100, UIBar.Height + 33), Color.White);
            energyBarRec.Width = (int)(p.powerUpTimer);
            s.Draw(energyBar, energyBarRec, Color.Blue);

            if (p.powerUpTimer < 0)
            {
                s.Draw(energyBarOutside, new Rectangle(130, 10, 240, 20), Color.Red);
            }
            else
            {
                s.Draw(energyBarOutside, new Rectangle(130, 10, 240, 20), Color.White);
            }
            
            lifeSpriteSheet = new Rectangle(0, 0, p.life * 40, 35);
            s.Draw(lives, new Vector2(10, 7), lifeSpriteSheet, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            if (p.hasArmor == 1)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.hasArmor == 2)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.hasArmor == 3)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(475, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.hasArmor == 4)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(475, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(375, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.hasArmor == 5)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(475, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(375, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (p.hasArmor == 6)
            {
                s.Draw(armor, new Vector2(375, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(475, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(375, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(425, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                s.Draw(armor, new Vector2(475, 53), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            //for (int i = 0; i < p.health; i++)
            //{
            //    healthBarRec.X += 50;
            //    s.Draw(healthBar, healthBarRec, Color.White);
            //}
            if (p.health == 0)
            {
                flash++;
            }
            healthBarRec.Width = (int)(p.health * 49);
            s.Draw(healthBar, healthBarRec, Color.White);
            if (p.health > 0)
            {
                s.Draw(energyBarOutside, new Rectangle(130, 30, 240, 20), Color.White);
            }
            else if(flash > 15 && flash <= 30 && p.health == 0)
            {
                s.Draw(energyBarOutside, new Rectangle(130, 30, 240, 20), Color.Red);
            }
            else if(flash > 30)
            {
                flash = 0;
            }
            
        }

    }
}

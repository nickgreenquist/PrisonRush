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
    public class PowerUpTile : Tile
    {
        public const int tileWidth = 60;
        public const int tileHeight = 90;

        public PowerUpTile(Texture2D texture, int x, int y, TileType t, String name) : base(texture,x,y,t,name)
        {
            tileRect = new Rectangle(x, y, tileWidth, tileHeight);
            if (fileName.Equals("PowerBurst") || fileName.Equals("ShrinkRay"))
            {
                tileRect = new Rectangle(x, y - 90, tileWidth + 20, tileHeight + 60);
            }
            if (fileName.Equals("Sword"))
            {
                tileRect = new Rectangle(x, y - 30, tileWidth, tileHeight);
            }
            if (fileName.Equals("Bazooka"))
            {
                tileRect = new Rectangle(x, y, tileWidth, tileHeight - 45);
            }
            if (fileName.Equals("Machinegun"))
            {
                tileRect = new Rectangle(x, y, tileWidth, tileHeight - 45);
            }
            if (fileName.Equals("Shotgun"))
            {
                tileRect = new Rectangle(x, y, tileWidth, tileHeight - 55);
            }
            if (fileName.Equals("Pistol"))
            {
                tileRect = new Rectangle(x + 15, y, tileWidth - 30, tileHeight - 55);
            }
        }
    }
}

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

namespace GSDIII_game
{
    // The possible states of the tile created used in collision detection
    public enum TileType
    {
        PassThrough, // The player is not affected by the tile, can jump, fall, climb through it, etc
    
        NonPassable, // The player can not pass through the tile, used for floor, walls, and ceiling
    
        Platform, // The player can pass through the tile from the bottom only, when landing, the player lands on the tile. 
    }

    //Eric Kipnis and Nick Greenquist
    public class Tile
    {
        public Texture2D tileTexure;
        public Rectangle tileRect;
        public TileType type;
        public const int tileWidth = 60;
        public const int tileHeight = 60;
        public String fileName;
        public Color tileColor;

        public Texture2D TileTexture
        {
            get { return tileTexure; }
            set { tileTexure = value; }
        }

        public Rectangle TileRect
        {
            get { return tileRect; }
            set { tileRect = value; }
        }

        public int X
        {
            get { return tileRect.X; }
            set { tileRect.X = value; }
        }

        public int Y
        {
            get { return tileRect.Y; }
            set { tileRect.Y = value; }
        }

        public String FileName
        {
            get { return fileName; }
        } 

        public Tile(Texture2D texture, int x, int y,TileType t)
        {
            tileColor = Color.White;
            tileTexure = texture;
            tileRect = new Rectangle(x, y, tileWidth, tileHeight);
            type = t;
            fileName = "";
            
        }

        public Tile(Texture2D texture, int x, int y, TileType t, String name)
        {
            tileColor = Color.White;
            tileTexure = texture;         
            type = t;
            fileName = name;
            tileRect = new Rectangle(x, y, tileWidth, tileHeight);
        }

        public virtual void Move(GameTime gameTime)
        {
        }
    }   
}

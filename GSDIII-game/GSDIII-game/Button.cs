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
    public class Button
    {
        public int[] TileXCoor;
        public int[] TileYCoor;
        public Texture2D buttonTextureUp;
        public Texture2D buttonTexturePressed;
        public Texture2D buttonTexture;
        public Rectangle buttonRect;
        public string[] direction;
        public int[] speed;
        public int[] maxDistance;
        public int[] currentDistance;
        public int NumTiles;
        public bool[] stopped;
        public bool[] looped;
        public bool colliding;

        public Button(Texture2D texU, Texture2D texP, Rectangle rect)
        {
            buttonTexture = texU;
            buttonTextureUp = texU;
            buttonTexturePressed = texP;
            buttonRect = rect;
            TileXCoor = new int[20];
            TileYCoor = new int[20];
            direction = new string[20];
            speed = new int[20];
            maxDistance = new int[20];
            currentDistance = new int[20];
            NumTiles = 0;
            stopped = new bool[20];
            looped = new bool[20];
            colliding = false;
        }        
    }
}

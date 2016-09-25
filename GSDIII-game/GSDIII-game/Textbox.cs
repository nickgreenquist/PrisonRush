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
    // Created by Eric Kipnis
    public class Textbox
    {
        // Textbox attributes
        private String fullText;
        private String currentText;
        private int stringIndex;
        private float timeElapsed;
        private Texture2D textboxTexture;
        private SpriteFont textFont;

        public Boolean signCollision;

        private List<String> signTexts;
        public  Dictionary<int, String> signPairs;

        // Textbox Constructor
        public Textbox(ContentManager content)
        {
            fullText = "";
            currentText = "";
            stringIndex = 0;
            timeElapsed = 0;
            textboxTexture = content.Load<Texture2D>("textbox");
            textFont = content.Load<SpriteFont>("SpriteFont2");
            signTexts = new List<string>();
            signPairs = new Dictionary<int, string>();


            signCollision = false;
        }

        public List<String> SignTexts
        {
            get { return signTexts; }
        }

        public Dictionary<int, String> SignPairs
        {
            get { return signPairs; }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (stringIndex < fullText.Length && timeElapsed >= 17)
            {
                currentText += fullText[stringIndex];
                stringIndex++;
                timeElapsed = 0;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (signCollision == true)
            {
                //if (currentText.Equals(fullText) == false)
                //{
                spritebatch.Draw(textboxTexture, new Rectangle(100, 600, 1000, 200), Color.White);
                spritebatch.DrawString(textFont, currentText, new Vector2(140, 610), Color.Black);
                //}
                //else
                //{

                //}
            }
            else
            {
                ClearTextBox();
            }
        }

        public void ChangeText(int index)
        {
            fullText = ModifyString(signPairs[index]);
        }

        public List<String> LoadTextFromFile(String pathName)
        {
            StreamReader fileReader = new StreamReader(pathName);
            String line = "";
            if (signTexts.Count > 0)
            {
                signTexts.Clear();
            }
            while ((line = fileReader.ReadLine()) != null)
            {
                signTexts.Add(line);
            }

            return signTexts;
        }

        public String ModifyString(String txt)
        {
            String currentLine = "";
            String currentString = "";
            String[] stringArray = txt.Split(' ');

            foreach (String word in stringArray)
            {
                if (textFont.MeasureString(currentLine + word).Length() > textboxTexture.Width - 350)
                {
                    currentString = currentString + currentLine + '\n';
                    currentLine = "";
                }

                currentLine = currentLine + word + ' ';
            }

            return currentString + currentLine;
        }

        public void ClearTextBox()
        {
            currentText = "";
            fullText = "";
            stringIndex = 0;
        }
    }
}

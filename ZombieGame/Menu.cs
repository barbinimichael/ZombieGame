using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ScrollingPlatform
{
    class Menu
    {
        //Button
        Texture2D button;

        //Title
        string titleText = "Zombies";
        public Rectangle rectangleTitle;
        int titleRectWidth;
        int titleRectHeight;

        //Start button
        string startText = "Start";
        public Rectangle rectangleStart;
        int startRectWidth;
        int startRectHeight;

        //font
        public SpriteFont timesNewRoman;
        
        //Music
        public Song music;

        public Menu()
        {
        }

        public void Load(ContentManager contentManager, int frameWidth, int frameHeight)
        {
            //Button
            button = contentManager.Load<Texture2D>("Button");

            //Title
            titleRectWidth = frameWidth / 2;
            titleRectHeight = frameHeight / 5;
            rectangleTitle = new Rectangle(frameWidth / 2 - titleRectWidth / 2, frameHeight / 3, titleRectWidth, titleRectHeight);

            //Start button
            startRectWidth = frameWidth / 3;
            startRectHeight = frameHeight / 6;
            rectangleStart = new Rectangle(frameWidth / 2 - startRectWidth / 2, frameHeight * 2 / 3, startRectWidth, startRectHeight);

            //Font
            timesNewRoman = contentManager.Load<SpriteFont>("TimesNewRoman");

            //Music
            music = contentManager.Load<Song>("arcadeMusic");

            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
        }

        public void Draw(SpriteBatch spriteBatch, int frameWidth, int frameHeight)
        {
            //Title
            spriteBatch.Draw(button, rectangleTitle, Color.White);
            spriteBatch.DrawString(timesNewRoman, titleText, new Vector2(frameWidth / 2 - titleRectWidth  / 8,
                                                                         frameHeight / 3 + titleRectHeight / 5), 
                                                                         Color.Red);
            //Start Button
            spriteBatch.Draw(button, rectangleStart, Color.White);
            spriteBatch.DrawString(timesNewRoman, startText, new Vector2(frameWidth / 2 - startRectWidth / 8,
                                                                         frameHeight * 2 / 3 + startRectHeight / 5),
                                                                         Color.Red);
        }
  
    }
}

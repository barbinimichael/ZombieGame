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
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Game state
        int gameState;

        //Frame
        int frameWidth = 1000;
        int frameHeight = 600;

        //Menu object
        Menu menu = new Menu();
        //Game object
        PlayingGame playingGame = new PlayingGame();
        //Songs
        Song[] song = new Song[2];

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = frameWidth;
            graphics.PreferredBackBufferHeight = frameHeight;
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Menu
            menu.Load(Content, frameWidth, frameHeight);
            //Playing game
            playingGame.Load(frameWidth, frameHeight, Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch(gameState)
            {
                case 0: //Menu
                   
                    
                    MouseState mouse = new MouseState();
                    mouse = Mouse.GetState();
                    var mousePosition = new Point(mouse.X, mouse.Y);

                    if (mouse.LeftButton == ButtonState.Pressed && menu.rectangleStart.Contains(mousePosition))
                        {
                            gameState = 1;
                        }
                    break;

                case 1: //Playing game
                    if (gameState == 1)
                        playingGame.Update(frameWidth, frameHeight, gameTime, Content);

                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            switch (gameState)
            {
                case 0:

                    menu.Draw(spriteBatch, frameWidth, frameHeight);

                    break;

                case 1:

                    playingGame.Draw(spriteBatch, menu.timesNewRoman, frameWidth, frameHeight, Content);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

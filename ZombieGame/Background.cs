using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingPlatform
{
    class Background
    {
        //Lava 
        Texture2D lava;
        public int lavaNumber = 5; 
        public Rectangle[] RectangleLava = new Rectangle[5];
        Random rand = new Random();

        //Background elements 
        public Rectangle RectangleGround, RectangleBackground, RectangleSecondFloor, RectangleLadder, RectangleDoor;
        public int groundHeight;
        Texture2D ground, background, ladder, doorClosed, doorOpen;
        public int scrollX, scrollWidth;
        public bool LeftLimit, RightLimit;
        
        //Weapon paint 
        Texture2D doublebarrelchalk, mFourchalk;
        public Rectangle Rectangledoublebarrelchalk, RectanglemFourchalk;

        public Background()
        {
        }

        public void load(int frameHeight, int frameWidth, ContentManager contentManager, Rectangle RectanglePlayer)
        {
            groundHeight = frameHeight / 10;
            scrollX = 0 - frameWidth / 2;
            scrollWidth = frameWidth * 2;
            RectangleGround = new Rectangle(scrollX, frameHeight - groundHeight, scrollWidth, groundHeight);
            RectangleSecondFloor = new Rectangle(scrollX, (frameHeight - groundHeight) / 2, scrollWidth, groundHeight / 10);

            RectangleLadder = new Rectangle(75, (frameHeight - groundHeight) / 2, 100, frameHeight - RectangleGround.Height - RectangleSecondFloor.Y);
            RectangleBackground = new Rectangle(scrollX, 0, scrollWidth, frameHeight - groundHeight);

            //Loading random lava locations
            for (int i = 0; i < lavaNumber; i++)
            {
                int randomLocation = rand.Next(5, scrollWidth);
                RectangleLava[i] = new Rectangle(scrollWidth - randomLocation, frameHeight - groundHeight - 1,
                                             25, groundHeight / 2);
            }
            //Loading background elements
            ground = contentManager.Load<Texture2D>("Ground Texture");
            background = contentManager.Load<Texture2D>("Background");
            lava = contentManager.Load<Texture2D>("Lava");
            ladder = contentManager.Load<Texture2D>("Ladder"); 

            //Loading weapon chalks 
            doublebarrelchalk = contentManager.Load<Texture2D>("DoubleBarrelChalk");
            mFourchalk = contentManager.Load<Texture2D>("mFourChalk");

            int chalkWidth = 200;
            int chalkHeight = 50;
            int chalkYPlacement = frameHeight - frameHeight / 3;

            Rectangledoublebarrelchalk = new Rectangle(-150, chalkYPlacement, chalkWidth, chalkHeight);
            RectanglemFourchalk = new Rectangle(frameWidth + 200, chalkYPlacement - 250, chalkWidth, chalkHeight);
        }

        public void update(bool backgroundMoveLeft, bool backgroundMoveRight, int frameWidth)
        {
            if (backgroundMoveLeft 
                && RectangleBackground.X + RectangleBackground.Width > frameWidth)
            {
                RectangleBackground.X -= 4;
                RectangleGround.X -= 4;
                RectangleSecondFloor.X -= 4;
                RectangleLadder.X -= 4;
                Rectangledoublebarrelchalk.X -= 4;
                RectanglemFourchalk.X -= 4;

                LeftLimit = false;

                for (int i = 0; i < lavaNumber; i++)
                {
                    RectangleLava[i].X -= 4;
                }

            }
            if (backgroundMoveRight
                && RectangleBackground.X < 0)
            {
                RectangleBackground.X += 4;
                RectangleGround.X += 4;
                Rectangledoublebarrelchalk.X += 4;
                RectanglemFourchalk.X += 4;
                RectangleSecondFloor.X += 4;
                RectangleLadder.X += 4;

                RightLimit = false;

                for (int i = 0; i < lavaNumber; i++)
                {
                    RectangleLava[i].X += 4;
                }
            }

            if (RectangleBackground.X + RectangleBackground.Width <= frameWidth)
            {
                RightLimit = true;
            }
            else if (RectangleBackground.X >= 0)
            {
                LeftLimit = true;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, RectangleBackground, Color.White);
            spriteBatch.Draw(ground, RectangleSecondFloor, Color.White);
            spriteBatch.Draw(ground, RectangleGround, Color.White);
            spriteBatch.Draw(doublebarrelchalk, Rectangledoublebarrelchalk, Color.White);
            spriteBatch.Draw(mFourchalk, RectanglemFourchalk, Color.White);
            spriteBatch.Draw(ladder, RectangleLadder, Color.White);

            for (int i = 0; i < lavaNumber; i++)
            {
                spriteBatch.Draw(lava, RectangleLava[i], Color.White);
            }
        }
    }
}

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
    class Player
    {
        public Rectangle RectanglePlayer;

        //Scrolling the background
        public bool backgroundMoveRight = false;
        public bool backgroundMoveLeft = false;

        //Player dimensions
        public int playerWidth, playerHeight;

        //Player direction / movement
        public bool Left;
        bool hasJumped = false;
        int jumpTime = 500;
        int jumpHeight; int jumpHeightperSec = 5;
        bool fall = false;
        int fallDistance = 0;

        //Player health
        public bool playerAlive = true;
        int playerHealth = 100;
        int healthLossTime = 1000;
        int healthGainTime = 2500;
        Texture2D healthBar, healthLevel, healthSign, playerDead, playerDeadOnFire, fire;
        public Rectangle RectangleHealthBar;
        Rectangle RectangleHealthLevel, RectangleHealthSign, RectangleFire;

        //Player firing weapon
        public bool fireing = false;
        public bool hasFired = false;

        //Time
        int timeElapsed = 0;
        public int gunTime = 0;
        int healthTime = 0;

        public Player()
        {
        }

        public void CreatingPlayerRectangle(Rectangle RectangleGround, int frameWidth, int frameHeight)
        {
            playerWidth = frameHeight / 3;
            playerHeight = frameHeight / 3;
            
            RectanglePlayer = new Rectangle(playerWidth, RectangleGround.Y - playerHeight + 10, playerWidth, playerHeight);

            RectangleHealthBar = new Rectangle(8, 7, 205, 32);
            RectangleHealthLevel = new Rectangle(10, 10, 200, 25);
            RectangleHealthSign = new Rectangle(1, 15, 15, 15);
            RectangleFire = new Rectangle(RectanglePlayer.X + RectanglePlayer.Width / 2,
                                          RectanglePlayer.Y,
                                          50, 50);
        }

        public void Load(ContentManager contentManager)
        {
            healthBar = contentManager.Load<Texture2D>("HealthBar");
            healthLevel = contentManager.Load<Texture2D>("HealthLevel");
            healthSign = contentManager.Load<Texture2D>("HealthSign");
            playerDead = contentManager.Load<Texture2D>("MikeDead");
            fire = contentManager.Load<Texture2D>("Fire");
        }

        public void DrawHealthBar(SpriteBatch spriteBatch, int frameHeight, Rectangle RectangleGround, int lavaNumber, 
                                  Rectangle[] RectangleLava)
        {
            spriteBatch.Draw(healthBar, RectangleHealthBar, Color.White);
            spriteBatch.Draw(healthLevel, RectangleHealthLevel, Color.White);
            spriteBatch.Draw(healthSign, RectangleHealthSign, Color.White);

            if (playerAlive == false)
            {
                RectanglePlayer.Height = 50;
                RectanglePlayer.Y = frameHeight - RectangleGround.Height - RectanglePlayer.Height;
                spriteBatch.Draw(playerDead, RectanglePlayer, Color.White);
            }
            for (int i = 0; i < lavaNumber; i++)
            {
                if (RectanglePlayer.Intersects(RectangleLava[i]))
                {
                    if(Left) RectangleFire.X = RectanglePlayer.X + RectanglePlayer.Width / 2;
                    else RectangleFire.X = RectanglePlayer.X + RectanglePlayer.Width / 4;
                    RectangleFire.Y = RectanglePlayer.Y;
                    spriteBatch.Draw(fire, RectangleFire, Color.White);
                }
            }
        }

        public void Move(int frameWidth, int frameHeight, Rectangle RectangleBackground, Rectangle RectangleLadder, GameTime gameTime, int groundHeight)
        {

            Console.WriteLine(RectanglePlayer.Y);
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            gunTime += gameTime.ElapsedGameTime.Milliseconds;
            healthTime += gameTime.ElapsedGameTime.Milliseconds;
            if (playerAlive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    Left = false;

                    if (RectanglePlayer.X + RectanglePlayer.Width != frameWidth)
                    {
                        RectanglePlayer.X = RectanglePlayer.X + 4;
                    }
                    if (RectanglePlayer.X + RectanglePlayer.Width >= (frameWidth - 5) &&
                        Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        backgroundMoveRight = true;
                    }
                }
                else
                {
                    backgroundMoveRight = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    Left = true;
                    if (RectanglePlayer.X != 0)
                    {
                        RectanglePlayer.X = RectanglePlayer.X - 4;
                    }
                    if (RectanglePlayer.X <= 0 &&
                        Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        backgroundMoveLeft = true;
                    }
                }
                else
                {
                    backgroundMoveLeft = false;
                }

                Jump();
                if (timeElapsed >= jumpTime && hasJumped == true)
                {
                    RectanglePlayer.Y -= jumpHeightperSec;
                    fallDistance += jumpHeightperSec;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q) && RectanglePlayer.Intersects(RectangleLadder) && RectanglePlayer.Y > RectangleLadder.Y)
                {
                    RectanglePlayer.Y = (frameHeight - groundHeight) / 2  - groundHeight - RectanglePlayer.Height;
                    fall = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.E) && RectanglePlayer.Intersects(RectangleLadder) && RectanglePlayer.Y < RectangleLadder.Y)
                {
                    RectanglePlayer.Y = frameHeight - groundHeight - RectanglePlayer.Height;
                }
            }
        }

        public void Jump()
        {
            jumpHeight = RectanglePlayer.Height / 3;
            if (Keyboard.GetState().IsKeyDown(Keys.W) && hasJumped == false)
            {
                hasJumped = true;
            }
            if (fallDistance >= jumpHeight)
            {
                fallDistance = 0;
                fall = true;
                timeElapsed = 0;
            }
            if (fall == true)
            {
                RectanglePlayer.Y += jumpHeightperSec;
                fallDistance += jumpHeightperSec;
                if (fallDistance >= jumpHeight)
                {
                    fall = false;
                    hasJumped = false;
                    fallDistance = 0;
                }

            }
        }

        public void fireWeapon(int currentFireRate, int currentAmmo)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && (gunTime >= currentFireRate) && currentAmmo > 0)
            {
                fireing = true;
                gunTime = 0;
            }
            else
            {
                fireing = false;
            }
        }

        public void playerHealthUpdate(int lavaNumber, Rectangle[] RectangleLava, Rectangle RectangleZombie, bool zombieAlive)
        {
            for (int i = 0; i < lavaNumber; i++)
            {
                if (RectanglePlayer.Intersects(RectangleLava[i]) && healthTime >= healthLossTime)
                {
                    healthTime = 0;
                    playerHealth -= 10;
                    RectangleHealthLevel.Width = RectanglePlayer.Width * playerHealth / 100;
                    RectangleHealthLevel.X = 10;
                }
            }

            if (RectanglePlayer.Intersects(RectangleZombie) && healthTime >= healthLossTime && zombieAlive)
            {
                healthTime = 0;
                playerHealth -= 25;
                RectangleHealthLevel.Width = RectanglePlayer.Width * playerHealth / 100;
                RectangleHealthLevel.X = 10;
            }

            if (healthTime >= healthGainTime && playerAlive)
            {
                healthTime = 0;
                playerHealth += 25;
                RectangleHealthLevel.Width = RectanglePlayer.Width * playerHealth / 100;
                if (playerHealth > 100)
                {
                    playerHealth = 100;
                    RectangleHealthLevel.Width = RectangleHealthBar.Width - 5;
                }
            }

            if (playerHealth <= 0)
            {
                playerAlive = false;
            }
        }
    }
}

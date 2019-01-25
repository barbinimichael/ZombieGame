using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingPlatform
{
    class Zombie
    {
        //Zombie textures
        Texture2D zombieLeft, zombieRight, zombieLeftImpact, zombieRightImpact, zombieDead;
        public Rectangle RectangleZombie;

        //Zombie Direction
        bool zombieLeftDirection = true;

        //Zombie Health
        bool zombieShot = false;
        public bool zombieAlive = true;
        public int zombieHealth = 200;

        public Zombie()
        {

        }

        public void Load(ContentManager contentManager, int frameHeight, Rectangle RectangleGround, int location)
        {
            zombieLeft = contentManager.Load<Texture2D>("Zombie");
            zombieRight = contentManager.Load<Texture2D>("Zombie1");
            zombieLeftImpact = contentManager.Load<Texture2D>("ZombieImpact");
            zombieRightImpact = contentManager.Load<Texture2D>("Zombie1Impact");
            zombieDead = contentManager.Load<Texture2D>("ZombieDead");

            int zombieHeight = frameHeight / 3;

            RectangleZombie = new Rectangle(location, RectangleGround.Y - zombieHeight + 10, 150, zombieHeight);
        }

        public void Update(Rectangle RectanglePlayer, Rectangle RectangleBulletStreak, bool backgroundMoveRight, bool backgroundMoveLeft,
                           bool LeftLimit, bool RightLimit, int currentDamage, int frameHeight, int groundHeight, Rectangle RectangleLadder, 
                           int frameWidth)
        {
            zombieMovement(RectanglePlayer, backgroundMoveRight, backgroundMoveLeft, LeftLimit, RightLimit, RectangleLadder, frameWidth);

            zombieHurt(RectangleBulletStreak, currentDamage, frameHeight, groundHeight);
        }

        public void zombieMovement(Rectangle RectanglePlayer, bool backgroundMoveRight, bool backgroundMoveLeft, bool LeftLimit,
            bool RightLimit, Rectangle RectangleLadder, int frameWidth)
        {

            Console.WriteLine(RectangleZombie.Y);
            if (zombieAlive)
            {
                if ((RectangleZombie.Intersects(RectangleLadder) && RectanglePlayer.Y < RectangleZombie.Y
                         && RectangleZombie.Y > RectangleLadder.Y)
                         || (RectangleZombie.Intersects(RectangleLadder) && RectanglePlayer.Y > RectangleZombie.Y
                         && RectangleZombie.Y < RectangleLadder.Y))
                {
                    RectangleZombie.Y = RectanglePlayer.Y;
                }
                if ((RectanglePlayer.Y < RectangleZombie.Y || RectanglePlayer.Y > RectangleZombie.Y)
                         && RectangleZombie.X < RectangleLadder.X)
                {
                    RectangleZombie.X++;
                    zombieLeftDirection = false;
                }
                else if ((RectanglePlayer.Y < RectangleZombie.Y || RectanglePlayer.Y > RectangleZombie.Y)
                    && RectangleZombie.X > RectangleLadder.X)
                {
                    RectangleZombie.X--;
                    zombieLeftDirection = true;
                }
                if (RectanglePlayer.Y == RectangleZombie.Y)
                {
                    if (RectanglePlayer.X < RectangleZombie.X)
                    {
                        RectangleZombie.X--;
                        zombieLeftDirection = true;
                    }
                    else if (RectanglePlayer.X > RectangleZombie.X)
                    {
                        RectangleZombie.X++;
                        zombieLeftDirection = false;
                    }
                }
                if (backgroundMoveLeft && LeftLimit == false)
                {
                    RectangleZombie.X += 4;

                    LeftLimit = false;

                }
                if (backgroundMoveRight && RightLimit == false)
                {
                    RectangleZombie.X -= 4;
                    RightLimit = false;
                }
                if (RectangleZombie.X + RectangleZombie.Width <= frameWidth)
                {
                    RightLimit = true;
                }
                else if (RectangleZombie.X >= 0)
                {
                    LeftLimit = true;
                }
            }

        }

        public void zombieHurt(Rectangle RectangleBulletStreak, int currentDamage, int frameHeight, int groundHeight)
        {
            if (RectangleBulletStreak.Intersects(RectangleZombie))
            {
                zombieShot = true;
                zombieHealth -= currentDamage;
            }
            else
            {
                zombieShot = false;
            }

            if (zombieHealth <= 0)
            {
                RectangleZombie.Y = frameHeight - groundHeight;
                zombieAlive = false;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (zombieShot)
            {
                if (zombieLeftDirection && zombieAlive)
                {
                    spriteBatch.Draw(zombieLeftImpact, RectangleZombie, Color.White);
                }
                else
                {
                    spriteBatch.Draw(zombieRightImpact, RectangleZombie, Color.White);
                }
            }
            else if (zombieShot == false && zombieAlive)
            {
                if (zombieLeftDirection)
                {
                    spriteBatch.Draw(zombieLeft, RectangleZombie, Color.White);
                }
                else
                {
                    spriteBatch.Draw(zombieRight, RectangleZombie, Color.White);
                }
            }
            else if (zombieAlive == false)
            {
                spriteBatch.Draw(zombieDead, RectangleZombie, Color.White);
            }
        }
    }
}

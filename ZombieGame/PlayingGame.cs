using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingPlatform
{
    class PlayingGame
    {
        //Player
        Player player = new Player();

        //Background
        Background background = new Background();

        //Weapons
        Gun gun = new Gun();

        //Zombie
        List<Zombie> zombie = new List<Zombie>();
        int spawnTime = 0;
        int spawnWait = 3000;

        Random randLoc = new Random();
        Random randWhichLoc = new Random();
        int location, whichLoc;

        //Point system
        Points points = new Points();

        public Song horrorMusic;
        int songChoice = 0;

        public PlayingGame()
        {
        }

        public void Load(int frameWidth, int frameHeight, ContentManager contentManager)
        {
            //Background music
            horrorMusic = contentManager.Load<Song>("HorrorMusic");
            //Background
            background.load(frameHeight, frameWidth, contentManager, player.RectanglePlayer);
            //Player
            player.CreatingPlayerRectangle(background.RectangleGround, frameWidth, frameHeight);
            player.Load(contentManager);
            //Weapon
            gun.Load(contentManager, player.RectanglePlayer, player.playerHeight, frameHeight, background.groundHeight);

            //Point system
            points.Load(contentManager, player.RectangleHealthBar);
        }

        public void Update(int frameWidth, int frameHeight, GameTime gameTime, ContentManager contentManager)
        {

            spawnTime += gameTime.ElapsedGameTime.Milliseconds;
            if (songChoice == 0)
            {
                MediaPlayer.Play(horrorMusic);
                songChoice = 1;
            }
            if (player.playerAlive)
            {
                //Player movement
                player.Move(frameWidth, frameHeight, background.RectangleBackground, background.RectangleLadder, gameTime, background.groundHeight);
                //Player fireing weapon
                player.fireWeapon(gun.currentFireRate, gun.currentAmmo);
                //Points
                points.decreasePoints(gun.doubleBarrelPurchase, gun.mFourPurchase, gun.mFourPurchaseAmmo, gun.doubleBarrelPurchaseAmmo);

                if (spawnTime >= spawnWait && zombie.Count() <= 1000)
                {
                    spawnTime = 0;
                    Zombie zomb = new Zombie();
                    zombie.Add(zomb);

                    whichLoc = randWhichLoc.Next(0, 3);

                    if (whichLoc == 1) location = randLoc.Next(-250, 150);
                    else location = randLoc.Next(frameWidth, frameWidth + 200);
                    zomb.Load(contentManager, frameHeight, background.RectangleGround, location);
                }
                //Weapon
                gun.Update(player.fireing, player.Left, player.RectanglePlayer, gameTime);
                foreach (Zombie zomb in zombie)
                {
                    //Player health 
                    player.playerHealthUpdate(background.lavaNumber, background.RectangleLava, zomb.RectangleZombie, zomb.zombieAlive);

                    //Points system
                    points.Update(gun.RectangleBulletStreak, player.fireing, zomb.RectangleZombie,
                                  zomb.zombieAlive, player.RectanglePlayer, zomb.zombieHealth, gun.currentDamage);
                    //Zombie
                    zomb.Update(player.RectanglePlayer, gun.RectangleBulletStreak, player.backgroundMoveRight, player.backgroundMoveLeft,
                              background.LeftLimit, background.RightLimit, gun.currentDamage, frameHeight, background.groundHeight, background.RectangleLadder, 
                              frameWidth);

                }
            }
            //Background
            background.update(player.backgroundMoveRight, player.backgroundMoveLeft, frameWidth);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont timesNewRoman, int frameWidth, int frameHeight, ContentManager contentManager)
        {
            //Background
            background.draw(spriteBatch);
            //Weapon

            if (player.playerAlive)
            {
                //Weapon
                gun.Draw(spriteBatch, player.RectanglePlayer, player.fireing);
                gun.purchaseWeapon(background.Rectangledoublebarrelchalk, background.RectanglemFourchalk,
                                       spriteBatch, timesNewRoman, frameWidth, frameHeight,
                                       player.RectanglePlayer, points.numberPoints);
            }
            //Player Health
            player.DrawHealthBar(spriteBatch, frameHeight, background.RectangleGround, background.lavaNumber, background.RectangleLava);

            foreach (Zombie zomb in zombie)
            {
                zomb.Draw(spriteBatch);
            }

            //Points system
            points.Draw(spriteBatch);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingPlatform
{
    class Gun
    {
        //Which gun currently using 
        Texture2D currentWeapon;
        //Orientation of the player when weapon being held
        Texture2D currentWeaponLeft, currentWeaponRight;
        //Orientation of the player when weapon being fired
        Texture2D currentWeaponLeftFireing, currentWeaponRightFireing;

        //Which gun in inventory
        bool inventoryOne = true;
        bool inventoryTwo = false;
        //If second inventory exists
        bool inventoryTwoAvailable = false;

        //Available weapons in game
        public bool fortyFive = true;
        public bool mFour, doublebarrel;
        Texture2D fortyFiveLeft, fortyFiveRight,
                  mFourLeft, mFourRight,
                  doublebarrelLeft, doublebarrelRight;

        //Available weapons when firing 
        Texture2D fortyFiveLeftFiring, fortyFiveRightFiring,
                  mFourLeftFiring, mFourRightFiring,
                  doublebarrelLeftFiring, doublebarrelRightFiring;
        //Sound effects of weapons fireing 
        public SoundEffect currentSound;
        SoundEffect fortyFiveSound;
        SoundEffect mFourSound;
        SoundEffect doublebarrelSound;
        //Rate of fire of weapons
        public int currentFireRate;
        int fortyFiveFireRate = 500 ;
        int mFourFireRate = 100;
        int doublebarrelFireRate = 100;
        //Weapon Damage
        public int currentDamage;
        int fortyFiveDamage = 50;
        int mFourDamage = 30;
        int doubleBarrelDamage = 250;
        //Purchasing Weapon
        public bool mFourPurchase = false;
        public bool mFourPurchaseAmmo = false;
        public bool doubleBarrelPurchase = false;
        public bool doubleBarrelPurchaseAmmo = false;
        //Ammo count
        bool findoutAmmo = true;
        bool findoutAmmoReserve = true;
        public int currentAmmo;
        public int currentAmmoReserve;
        int fortyFiveAmmo = 8;
        int fortyFiveAmmoReserve = 32;
        int mFourAmmo = 30;
        int mFourAmmoReserve = 120;
        int doublebarrelAmmo = 2;
        int doublebarrelAmmoReserve = 8;
        SpriteFont timesNewRoman;
        //Weapon reload
        int currentReload;
        int fortyFiveReload = 1000;
        int mFourReload = 2500;
        int doublebarrelReload = 3000;
        int reloadTime = 0;
        //Bullet streak
        Texture2D bulletStreak;
        public Rectangle RectangleBulletStreak;
        int currentRange, currentSpread;
        int fortyFiveRange = 500;
        int fortyFiveSpread = 15;
        int mFourRange = 600;
        int mFourSpread = 10;
        int doubleBarrelRange = 150;
        int doubleBarrelSpread = 100;

        public Gun()
        {

        }

        public void Load(ContentManager contentManager, Rectangle RectanglePlayer, int playerHeight, int frameHeight, int groundHeight)
        {
            //Forty-five caliber pistol textures
            fortyFiveLeft = contentManager.Load<Texture2D>("Mike451");
            fortyFiveRight = contentManager.Load<Texture2D>("Mike45");
            fortyFiveLeftFiring = contentManager.Load<Texture2D>("MikeFire451");
            fortyFiveRightFiring = contentManager.Load<Texture2D>("MikeFire45");
            //Sound effect
            fortyFiveSound = contentManager.Load<SoundEffect>("fortyFive");

            //M4A1 Assault Rifle textures
            mFourLeft = contentManager.Load<Texture2D>("Mike1");
            mFourRight = contentManager.Load<Texture2D>("Mike");
            mFourLeftFiring = contentManager.Load<Texture2D>("Mike1Fire");
            mFourRightFiring = contentManager.Load<Texture2D>("MikeFire");
            //Sound effect
            mFourSound = contentManager.Load<SoundEffect>("M4A1");

            //Doublebarrel shotgun 
            doublebarrelLeft = contentManager.Load<Texture2D>("MikeDoubleBarrel1");
            doublebarrelRight = contentManager.Load<Texture2D>("MikeDoubleBarrel");
            doublebarrelLeftFiring = contentManager.Load<Texture2D>("MikeDoubleBarrelFire1");
            doublebarrelRightFiring = contentManager.Load<Texture2D>("MikeDoubleBarrelFire");
            //Sound effect
            doublebarrelSound = contentManager.Load<SoundEffect>("DoubleBarrel");

            //Font 
            timesNewRoman = contentManager.Load<SpriteFont>("TimesNewRoman");

            //Starting graphic
            currentWeapon = fortyFiveRight;
            //Starting sound
            currentFireRate = fortyFiveFireRate;
            currentSound = fortyFiveSound;
            //Starting range of weapon 
            currentRange = fortyFiveRange;
            currentSpread = fortyFiveSpread;
            //Starting ammo
            currentAmmo = fortyFiveAmmo;
            currentAmmoReserve = fortyFiveAmmoReserve;
            //Staring reload
            currentReload = fortyFiveReload;

            //BulletStreak
            bulletStreak = contentManager.Load<Texture2D>("BulletStreak");
            RectangleBulletStreak = new Rectangle(RectanglePlayer.X + RectanglePlayer.Width,
                                                  RectanglePlayer.Y + RectanglePlayer.Height / 2,
                                                  0,
                                                  currentSpread);
        }

        public void Update(bool fireing, bool Left, Rectangle RectanglePlayer, GameTime gameTime)
        {
            whichWeapon();
            bulletStreakEffect(Left, fireing, RectanglePlayer);
            WeaponAmmoLive(fireing, gameTime);

            if (fireing == false)
            {
                if (Left)
                {
                    currentWeapon = currentWeaponLeft;
                }
                else if (Left == false)
                {
                    currentWeapon = currentWeaponRight;
                }
            }
            else if (fireing)
            {
                if (Left)
                {
                    currentWeapon = currentWeaponLeftFireing;
                }
                else if (Left == false)
                {
                    currentWeapon = currentWeaponRightFireing;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle RectanglePlayer, bool fireing)
        {
            spriteBatch.Draw(currentWeapon, RectanglePlayer, Color.White);
            spriteBatch.Draw(bulletStreak, RectangleBulletStreak, Color.White);

            spriteBatch.DrawString(timesNewRoman, currentAmmo.ToString() + ":" + currentAmmoReserve.ToString(), new Vector2(10, 100), Color.Red);

            if (fireing) currentSound.Play();
        }

        public void whichWeapon()
        {
            if (fortyFive)
            {
                currentWeaponLeft = fortyFiveLeft;
                currentWeaponRight = fortyFiveRight;
                currentWeaponLeftFireing = fortyFiveLeftFiring;
                currentWeaponRightFireing = fortyFiveRightFiring;
                currentFireRate = fortyFiveFireRate;
                currentSpread = fortyFiveSpread;
                currentRange = fortyFiveRange;
                currentSound = fortyFiveSound;
                currentDamage = fortyFiveDamage;
                currentReload = fortyFiveReload;
            }
            else if (mFour)
            {
                currentWeaponLeft = mFourLeft;
                currentWeaponRight = mFourRight;
                currentWeaponLeftFireing = mFourLeftFiring;
                currentWeaponRightFireing = mFourRightFiring;
                currentFireRate = mFourFireRate;
                currentSpread = mFourSpread;
                currentRange = mFourRange;
                currentSound = mFourSound;
                currentDamage = mFourDamage;
                currentReload = mFourReload;
            }
            else if (doublebarrel)
            {
                currentWeaponLeft = doublebarrelLeft;
                currentWeaponRight = doublebarrelRight;
                currentWeaponLeftFireing = doublebarrelLeftFiring;
                currentWeaponRightFireing = doublebarrelRightFiring;
                currentFireRate = doublebarrelFireRate;
                currentSpread = doubleBarrelSpread;
                currentRange = doubleBarrelRange;
                currentSound = doublebarrelSound;
                currentDamage = doubleBarrelDamage;
                currentReload = doublebarrelReload;
            }
        }

        public void bulletStreakEffect(bool Left, bool firing, Rectangle RectanglePlayer)
        {
            RectangleBulletStreak.Y = RectanglePlayer.Y + RectanglePlayer.Height / 2;
            if (firing)
            {
                RectangleBulletStreak.Width = currentRange;
                if (Left)
                {
                    RectangleBulletStreak.X = RectanglePlayer.X - currentRange;
                }
                else if (Left == false)
                {
                    RectangleBulletStreak.X = RectanglePlayer.X + RectanglePlayer.Width;
                }
            }
            else if (firing == false)
            {
                currentDamage = 0;
                RectangleBulletStreak.Width = 0;
                if (Left)
                {
                    RectangleBulletStreak.X = RectanglePlayer.X;
                }
                else if (Left == false)
                {
                    RectangleBulletStreak.X = RectanglePlayer.X + RectanglePlayer.Width;

                }
            }
        }

        public void purchaseWeapon(Rectangle Rectangledoublebarrelchalk, Rectangle RectanglemFourchalk,
                                   SpriteBatch spriteBatch, SpriteFont timesNewRoman, int frameWidth, int frameHeight,
                                   Rectangle RectanglePlayer, int numberPoints)
        {
            string doublebarrelpurchase = "Press f to purchase doublebarrel for 500";
            string doublebarrelpurchaseammo = "Press f to purchase ammo for 250";
            string mFourpurchase = "Press f to purchase M4A1 for 750";
            string mFourpurchaseammo = "Press f to purchase ammo for 350";

            if (RectanglePlayer.Intersects(Rectangledoublebarrelchalk))
            {
                if (doublebarrel)
                {
                    spriteBatch.DrawString(timesNewRoman, doublebarrelpurchaseammo, new Vector2(frameWidth / 4, frameHeight / 2), Color.Red);
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && numberPoints >= 250)
                    {
                        doubleBarrelPurchaseAmmo = true;
                        findoutAmmo = true;
                        findoutAmmoReserve = true;
                    }
                    else
                    {
                        doubleBarrelPurchaseAmmo = false;
                    }
                }
                else if (doublebarrel == false)
                {
                    spriteBatch.DrawString(timesNewRoman, doublebarrelpurchase, new Vector2(frameWidth / 4, frameHeight / 2), Color.Red);
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && numberPoints >= 500)
                    {
                        doublebarrel = true;
                        fortyFive = false;
                        mFour = false;
                        doubleBarrelPurchase = true;
                        findoutAmmo = true;
                        findoutAmmoReserve = true;
                        currentReload = doublebarrelReload;
                    }
                }
            }

            if (RectanglePlayer.Intersects(RectanglemFourchalk))
            {
                if (mFour)
                {
                    spriteBatch.DrawString(timesNewRoman, mFourpurchaseammo, new Vector2(frameWidth / 4, frameHeight / 2), Color.Red);
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && numberPoints >= 350)
                    {
                        mFourPurchaseAmmo = true;
                        findoutAmmo = true;
                        findoutAmmoReserve = true;
                    }
                    else
                    {
                        mFourPurchaseAmmo = false;
                    }
                }
                else if (mFour == false)
                {
                    spriteBatch.DrawString(timesNewRoman, mFourpurchase, new Vector2(frameWidth / 4, frameHeight / 2), Color.Red);
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && numberPoints >= 750)
                    {
                        mFour = true;
                        fortyFive = false;
                        doublebarrel = false;
                        mFourPurchase = true;
                        findoutAmmo = true;
                        findoutAmmoReserve = true;
                        currentReload = mFourReload;
                    }
                }
            }
        }

        public void WeaponAmmoLive(bool fireing, GameTime gameTime)
        {
            if (fireing)
            {
                currentAmmo--;
            }

            if (currentAmmo <= 0 && currentAmmoReserve != 0)
            {
                fireing = false;

                reloadTime += gameTime.ElapsedGameTime.Milliseconds;

                if (reloadTime >= currentReload)
                {
                    reloadTime = 0;
                    findoutAmmo = true;
                }
            }

            if (currentAmmo <= 0 && currentAmmoReserve <= 0)
            {
                fireing = false;
            }

            if (findoutAmmo)
            {
                if (fortyFive)
                {
                    currentAmmo = fortyFiveAmmo;
                }
                else if (doublebarrel)
                {
                    currentAmmo = doublebarrelAmmo;
                }
                else if (mFour)
                {
                    currentAmmo = mFourAmmo;
                }
                findoutAmmo = false;
                currentAmmoReserve -= currentAmmo;
            }

            if (findoutAmmoReserve)
            {
                if (fortyFive)
                {
                    currentAmmoReserve = fortyFiveAmmoReserve;
                }
                else if (doublebarrel)
                {
                    currentAmmoReserve = doublebarrelAmmoReserve;
                }
                else if (mFour)
                {
                    currentAmmoReserve = mFourAmmoReserve;
                }
                findoutAmmoReserve = false;
            }
        }
    }
}

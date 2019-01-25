using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingPlatform
{
    class Points
    {

        //Points Display
        Rectangle RectanglePointDisplay;
        Texture2D pointDisplay;
        SpriteFont TimesNewRoman;

        //Point value
        public int numberPoints = 0;
        public bool givePoints = true;
        bool ShouldIncreasePointsdb = true;
        bool ShouldIncreasePointsmF = true;

        public Points()
        {

        }

        public void Load(ContentManager contentManager, Rectangle RectangleHealthBar)
        {
            pointDisplay = contentManager.Load<Texture2D>("Button");
            RectanglePointDisplay = new Rectangle(RectangleHealthBar.X, 
                                                  RectangleHealthBar.Y + RectangleHealthBar.Height,
                                                  RectangleHealthBar.Width / 2,
                                                  RectangleHealthBar.Height * 3 / 2);

            TimesNewRoman = contentManager.Load<SpriteFont>("TimesNewRoman");
        }

        public void Update(Rectangle RectangleBulletStreak, bool fireing, Rectangle RectangleZombie, 
                           bool zombieAlive, Rectangle RectanglePlayer, int zombieHealth, 
                           int currentDamage)
        {
            zombiePoint(RectangleBulletStreak, fireing, RectangleZombie, zombieAlive, RectanglePlayer, zombieHealth, 
                                 currentDamage);
        }

        public void zombiePoint(Rectangle RectangleBulletStreak, bool fireing, Rectangle RectangleZombie, bool zombieAlive, Rectangle RectanglePlayer, int zombieHealth, 
                                int currentDamage)
        {
            
            if (RectangleBulletStreak.Intersects(RectangleZombie) && !RectangleZombie.Intersects(RectanglePlayer))
            {
                numberPoints += 10;

                if (zombieHealth <= currentDamage)
                {
                    numberPoints += 100;
                }
            }
        }

        public void decreasePoints(bool doubleBarrelPurchase, bool mFourPurchase, bool doubleBarrelPurchaseAmmo, bool mFourPurchaseAmmo)
        {
            if (doubleBarrelPurchase && ShouldIncreasePointsdb)
            {
                ShouldIncreasePointsdb = false;
                numberPoints -= 500;
            }
            else if (mFourPurchase && ShouldIncreasePointsmF)
            {
                ShouldIncreasePointsmF = false;
                numberPoints -= 750;
            }
            else if (doubleBarrelPurchaseAmmo)
            {
                numberPoints -= 150;
                doubleBarrelPurchaseAmmo = false;
            }
            else if (mFourPurchaseAmmo)
            {
                numberPoints -= 250;
                mFourPurchaseAmmo = false;
            }
            else
            {
                numberPoints += 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pointDisplay, RectanglePointDisplay, Color.White);
            spriteBatch.DrawString(TimesNewRoman, numberPoints.ToString(), new Vector2(RectanglePointDisplay.X+ 5,
                                                                                    RectanglePointDisplay.Y),
                                                                                    Color.Red);
        }
    }
}

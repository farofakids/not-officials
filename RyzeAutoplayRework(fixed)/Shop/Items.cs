using Autoplay;
using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;
using System;

namespace Shop
{
    class Items : Helper
    {
        public static List<CItem> RyzeItems = new List<CItem>()
        {
            new CItem(0, 0),
            new CItem(350, (int)ItemId.Sapphire_Crystal),         //1
            new CItem(400, (int)ItemId.Tear_of_the_Goddess),      //2
            new CItem(1100, (int)ItemId.Sorcerers_Shoes),         //3
            new CItem(1250, (int)ItemId.Needlessly_Large_Rod),    //4
            new CItem(1100, (int)ItemId.Archangels_Staff),        //5
            new CItem(2800, (int)ItemId.Frozen_Heart),            //6
            new CItem(1200, (int)ItemId.Catalyst_of_Aeons),  //7
            new CItem(1800, (int)ItemId.Rod_of_Ages),             //8
            new CItem(2650, (int)ItemId.Void_Staff),              //9
            new CItem(850, (int)ItemId.Blasting_Wand),            //10
            new CItem(2150, (int)ItemId.Rabadons_Deathcap)        //11
        };
        public static void BuyRyzeItems()
        {
            #region SapphireCrystal
            if (!HasItem(RyzeItems[1]) && !HasItem(RyzeItems[2]) && !HasItem(RyzeItems[5]) && !HasItem(3040) && !HasItem(3048))
            {
                if (myHero.Gold >= RyzeItems[1].Gold)
                    Buy(RyzeItems[1]);
            }
            #endregion
            #region Tear
            if (!HasItem(RyzeItems[2]) && !HasItem(RyzeItems[5]) && HasItem(RyzeItems[1]) && !HasItem(3040) && !HasItem(3048))
            {
                if (myHero.Gold >= RyzeItems[2].Gold)
                    Buy(RyzeItems[2]);
            }
            #endregion
            #region Catalyst
            if(!HasItem(RyzeItems[7]) && !HasItem(RyzeItems[8]))
            {
                if (myHero.Gold >= RyzeItems[7].Gold)
                    Buy(RyzeItems[7]);
            }
            #endregion
            #region ROA
            if (!HasItem(RyzeItems[8]) && HasItem(RyzeItems[7]))
            {
                if (myHero.Gold >= RyzeItems[8].Gold)
                    Buy(RyzeItems[8]);
            }
            #endregion
            #region Boots
            if (!HasItem(RyzeItems[3]))
            {
                if (myHero.Gold >= RyzeItems[3].Gold)
                    Buy(RyzeItems[3]);
            }
            #endregion
            #region LargeRod
            if (!HasItem(RyzeItems[4]) && !HasItem(RyzeItems[5]) && !HasItem(3040) && !HasItem(3048))
            {
                if (myHero.Gold >= RyzeItems[4].Gold)
                    Buy(RyzeItems[4]);
            }
            #endregion
            #region ArchStaff
            if (!HasItem(RyzeItems[5]) && HasItem(RyzeItems[4]) && HasItem(RyzeItems[2]) && !HasItem(3040) && !HasItem(3048))
            {
                if (myHero.Gold >= RyzeItems[5].Gold)
                    Buy(RyzeItems[5]);
            }
            #endregion
            #region FrozenHearth
            if (!HasItem(RyzeItems[6]))
            {
                if (myHero.Gold >= RyzeItems[6].Gold)
                    Buy(RyzeItems[6]);
            }
            #endregion
            #region VoidStaff
            if (!HasItem(RyzeItems[9]))
            {
                if (myHero.Gold >= RyzeItems[9].Gold)
                    Buy(RyzeItems[9]);
            }
            #endregion
            #region BlastWand
            if (!HasItem(RyzeItems[10]) && !HasItem(RyzeItems[11]))
            {
                if (myHero.Gold >= RyzeItems[10].Gold)
                    Buy(RyzeItems[10]);
            }
            #endregion
            #region Rabadon
            if (!HasItem(RyzeItems[11]) && HasItem(RyzeItems[10]))
            {
                if (myHero.Gold >= RyzeItems[11].Gold)
                    Buy(RyzeItems[11]);
            }
            #endregion
        }

        private static void Buy(CItem item)
        {
            Item itm = new Item(item.ID);
            itm.Buy();
        }

        private static bool HasItem(CItem item)
        {
            return Item.HasItem(item.ID, myHero);
        }
        private static bool HasItem(int id)
        {
            return Item.HasItem(id, myHero);
        }

        public class CItem
        {
            public float Gold;
            public int ID;
            public CItem(float g, int id)
            {
                Gold = g;
                ID = id;
            }
        }//CItem
    }
}
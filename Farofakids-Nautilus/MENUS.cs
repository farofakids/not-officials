using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace Farofakids_Nautilus
{
    internal class MENUS
    {
        public static Menu FarofakidsNautilusMenu, ComboMenu, HarassMenu, DrawingMenu, MiscMenu;

        public static void Initialize()
        {
            FarofakidsNautilusMenu = MainMenu.AddMenu("Farofakids Nautilus", "Farofakids-Nautilus");
            FarofakidsNautilusMenu.AddGroupLabel("Farofakids Nautilus");
            FarofakidsNautilusMenu.Add("URFMODE", new CheckBox("URF MODE: EVER W AND E"));

            // Combo Menu
            ComboMenu = FarofakidsNautilusMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.Add("UseQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("UseWCombo", new CheckBox("Use W"));
            ComboMenu.Add("UseECombo", new CheckBox("Use E"));
            ComboMenu.Add("UseRCombo", new CheckBox("Use R"));
            if (EntityManager.Heroes.Enemies.Count > 0)
            {
                ComboMenu.AddSeparator();
                ComboMenu.AddGroupLabel("Q Settings: Use ON:");
                var addedChamps = new List<string>();
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => !addedChamps.Contains(enemy.ChampionName)))
                {
                    addedChamps.Add(enemy.ChampionName);
                    ComboMenu.Add("UseQ_On" + enemy.ChampionName, new CheckBox(string.Format("{0} ({1})", enemy.ChampionName, enemy.Name)));
                }
            }
            if (EntityManager.Heroes.Enemies.Count > 0)
            {
                ComboMenu.AddSeparator();
                ComboMenu.AddGroupLabel("R Settings: Use ON:");
                var addedChamps = new List<string>();
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => !addedChamps.Contains(enemy.ChampionName)))
                {
                    addedChamps.Add(enemy.ChampionName);
                    ComboMenu.Add("UseR_On" + enemy.ChampionName, new CheckBox(string.Format("{0} ({1})", enemy.ChampionName, enemy.Name)));
                }
            }

            // Harass Menu
            HarassMenu = FarofakidsNautilusMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.Add("UseQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("UseWHarass", new CheckBox("Use W"));
            HarassMenu.Add("UseEHarass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("HarassMana", new Slider("Mana Limiter at Mana %", 40));

            // Drawing Menu
            DrawingMenu = FarofakidsNautilusMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("QRange", new CheckBox("Q range", false));
            DrawingMenu.Add("WRange", new CheckBox("W range", false));
            DrawingMenu.Add("ERange", new CheckBox("E range", false));
            DrawingMenu.Add("RRange", new CheckBox("R range", false));

            // Setting Menu
            MiscMenu = FarofakidsNautilusMenu.AddSubMenu("Settings", "Settings");
            MiscMenu.AddGroupLabel("Settings");
            MiscMenu.AddLabel("Interrupter");
            MiscMenu.Add("InterruptSpells", new CheckBox("Interrupt spells"));

        }

        public static bool URFMODE { get { return FarofakidsNautilusMenu["URFMODE"].Cast<CheckBox>().CurrentValue; } }

        //combo
        public static bool UseQCombo { get { return ComboMenu["UseQCombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool UseWCombo { get { return ComboMenu["UseWCombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool UseECombo { get { return ComboMenu["UseECombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool UseRCombo { get { return ComboMenu["UseRCombo"].Cast<CheckBox>().CurrentValue; } }

        //harras
        public static bool UseQHarass { get { return HarassMenu["UseQHarass"].Cast<CheckBox>().CurrentValue; } }
        public static bool UseWHarass { get { return HarassMenu["UseWHarass"].Cast<CheckBox>().CurrentValue; } }
        public static bool UseEHarass { get { return HarassMenu["UseEHarass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["HarassMana"].Cast<Slider>().CurrentValue; } }

        //draw
        public static bool QRange { get { return DrawingMenu["QRange"].Cast<CheckBox>().CurrentValue; } }
        public static bool WRange { get { return DrawingMenu["WRange"].Cast<CheckBox>().CurrentValue; } }
        public static bool ERange { get { return DrawingMenu["ERange"].Cast<CheckBox>().CurrentValue; } }
        public static bool RRange { get { return DrawingMenu["RRange"].Cast<CheckBox>().CurrentValue; } }
        
        //misc
        public static bool InterruptSpells { get { return MiscMenu["InterruptSpells"].Cast<CheckBox>().CurrentValue; } }

    }
}

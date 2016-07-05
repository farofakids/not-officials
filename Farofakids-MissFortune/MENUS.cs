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

namespace Farofakids_MissFortune
{
    internal class MENUS
    {
        private static Menu FarofakidsMissFortuneMenu, ComboMenu,  DrawingMenu;

        public static void Initialize()
        {
            FarofakidsMissFortuneMenu = MainMenu.AddMenu("Farofakids MissFortune", "Farofakids-MissFortune");
            FarofakidsMissFortuneMenu.AddGroupLabel("Farofakids MissFortune");

            ComboMenu = FarofakidsMissFortuneMenu.AddSubMenu("Spells Config", "Spells Config");
            ComboMenu.AddLabel("Q config");
            ComboMenu.Add("autoQ", new CheckBox("auto Q"));
            ComboMenu.Add("harasQ", new CheckBox("Use Q on minion"));
            ComboMenu.Add("killQ", new CheckBox("Use Q only if can kill minion", false));
            ComboMenu.Add("qMinionWidth", new Slider("Collision width calculation", 70, 200, 0));

            ComboMenu.AddLabel("W config");
            ComboMenu.Add("harasW", new CheckBox("Harass W"));
            ComboMenu.Add("autoW", new CheckBox("autoW"));

            ComboMenu.AddLabel("E config");
            ComboMenu.Add("autoE", new CheckBox("auto E"));
            ComboMenu.Add("AGC", new CheckBox("AntiGapcloserE"));

            ComboMenu.AddLabel("R config");
            ComboMenu.Add("autoR", new CheckBox("auto R"));
            ComboMenu.Add("Rturrent", new CheckBox("Don't R under turret"));
            ComboMenu.Add("useR", new KeyBind("Semi-manual cast R key", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
            ComboMenu.Add("disableBlock", new KeyBind("disableBlock, Disable R key", false, KeyBind.BindTypes.HoldActive, "R".ToCharArray()[0]));
            ComboMenu.Add("newTarget", new CheckBox("Try change focus after attack ", false));

            // Drawing Menu
            DrawingMenu = FarofakidsMissFortuneMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("QRange", new CheckBox("Q range", false));
            DrawingMenu.Add("WRange", new CheckBox("W range", false));
            DrawingMenu.Add("ERange", new CheckBox("E range", false));
            DrawingMenu.Add("RRange", new CheckBox("R range", false));

            Chat.Print("Farofakids-MissFortune: Loaded", System.Drawing.Color.Red);

        }

        public static bool harasQ { get { return ComboMenu["harasQ"].Cast<CheckBox>().CurrentValue; } }
        public static bool harasW { get { return ComboMenu["harasW"].Cast<CheckBox>().CurrentValue; } }
        public static bool autoW { get { return ComboMenu["autoW"].Cast<CheckBox>().CurrentValue; } }
        public static bool disableBlock { get { return ComboMenu["disableBlock"].Cast<KeyBind>().CurrentValue; } }
        public static bool useR { get { return ComboMenu["useR"].Cast<KeyBind>().CurrentValue; } }
        public static bool AGC { get { return ComboMenu["AGC"].Cast<CheckBox>().CurrentValue; } }
        public static bool newTarget { get { return ComboMenu["newTarget"].Cast<CheckBox>().CurrentValue; } }
        public static bool autoQ { get { return ComboMenu["autoQ"].Cast<CheckBox>().CurrentValue; } }
        public static bool autoE { get { return ComboMenu["autoE"].Cast<CheckBox>().CurrentValue; } }
        public static int qMinionWidth { get { return ComboMenu["qMinionWidth"].Cast<Slider>().CurrentValue; } }
        public static bool killQ { get { return ComboMenu["killQ"].Cast<CheckBox>().CurrentValue; } }
        public static bool autoR { get { return ComboMenu["autoR"].Cast<CheckBox>().CurrentValue; } }
        public static bool Rturrent { get { return ComboMenu["Rturrent"].Cast<CheckBox>().CurrentValue; } }
        
        //draw
        public static bool QRange { get { return DrawingMenu["QRange"].Cast<CheckBox>().CurrentValue; } }
        public static bool WRange { get { return DrawingMenu["WRange"].Cast<CheckBox>().CurrentValue; } }
        public static bool ERange { get { return DrawingMenu["ERange"].Cast<CheckBox>().CurrentValue; } }
        public static bool RRange { get { return DrawingMenu["RRange"].Cast<CheckBox>().CurrentValue; } }
    }
}

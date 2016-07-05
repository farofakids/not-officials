using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;

// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace DamageIndicator
{
    public static class Config
    {
        private const string MenuName = "Damage Draw";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Damage Draw");
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu DamageIndicatorMenu;

            static Modes()
            {

                DamageIndicatorMenu = Menu.AddSubMenu("::DamageIndicator::");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Draw
            {
                private static readonly CheckBox _drawHealth;
                private static readonly CheckBox _drawPercent;
                private static readonly CheckBox _drawStatiscs;
                //Color Config
                private static readonly ColorConfig _healthColor;

                //CheckBoxes

                public static bool DrawHealth
                {
                    get { return _drawHealth.CurrentValue; }
                }

                public static bool DrawPercent
                {
                    get { return _drawPercent.CurrentValue; }
                }

                public static bool DrawStatistics
                {
                    get { return _drawStatiscs.CurrentValue; }
                }


                //Colors
                public static Color HealthColor
                {
                    get { return _healthColor.GetSystemColor(); }
                }

                static Draw()
                {
                    DamageIndicatorMenu.AddGroupLabel("Spell drawings Settings :");
                    _drawHealth = DamageIndicatorMenu.Add("damageIndicatorDraw", new CheckBox("Draw damage indicator ?"));
                    _drawPercent = DamageIndicatorMenu.Add("percentageIndicatorDraw", new CheckBox("Draw damage percentage ?"));
                    _drawStatiscs = DamageIndicatorMenu.Add("statiscsIndicatorDraw", new CheckBox("Draw damage statistics ?"));

                    _healthColor = new ColorConfig(DamageIndicatorMenu, "healthColorConfig", Color.Yellow,
                        "Color Damage Indicator:");
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
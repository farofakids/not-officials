using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Reflection;
namespace KappaLeBlanc
{
    class LBMenu : Helper
    {
        public static Menu Menu, ComboM, LCM, KSM, AntiGapcloserM, HSM,DrawM, FLM, Misc;
        static readonly string[] Modes = new string[] { "Move to nearest ally tower", "Move to mouse", "Move to enemy" };
        public static void StartMenu()
        {
            Menu = MainMenu.AddMenu("Kappa Leblanc", "menu");
            Menu.AddGroupLabel("Kappa Leblanc Reworked");
            Menu.AddLabel("By Capitão Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("Current Version: " + Assembly.GetExecutingAssembly().GetName().Version);

            DrawingsMenu();
            ComboMenu();
            LaneClearMenu();
            KillstealMenu();
            AntiGapMenu();
            Flee();
            MiscMenu();
        }
        private static void DrawingsMenu()
        {
            DrawM = Menu.AddSubMenu("Drawings", "draw");
            DrawM.Add("Q", new CheckBox("Draw Q range"));
            DrawM.Add("W", new CheckBox("Draw W range", false));
            DrawM.Add("WPos", new CheckBox("Draw W Position", false));
            DrawM.Add("E", new CheckBox("Draw E range", false));
            DrawM.AddSeparator();
            DrawM.Add("line", new CheckBox("Draw line to killable target"));
            DrawM.Add("dist", new Slider("Max line distance", 1000, 500, 3000));   

        }
        private static void ComboMenu()
        {
            ComboM = Menu.AddSubMenu("Combo", "combo");
            ComboM.Add("Q", new CheckBox("Use Q"));
            ComboM.Add("W", new CheckBox("Use W"));
            ComboM.Add("extW", new CheckBox("Extended W (W to gapclose)", false));
            ComboM.Add("E", new CheckBox("Use E"));
            ComboM.Add("R", new CheckBox("Use R"));
            ComboM.Add("RQ", new CheckBox("Use R (Q)"));
            ComboM.Add("RW", new CheckBox("Use R (W)", false));
            ComboM.Add("RE", new CheckBox("Use R (E)"));

        }
        private static void LaneClearMenu()
        {
            LCM = Menu.AddSubMenu("Laneclear", "laneclear");
            LCM.Add("Q", new CheckBox("Use Q", false));
            LCM.Add("QMana", new Slider("Min % mana to Q", 20, 0, 100));
            LCM.Add("W", new CheckBox("Use W"));         
            LCM.Add("WMana", new Slider("Min % mana to W", 20, 0, 100));
            LCM.Add("W2", new CheckBox("Auto W2"));
            LCM.Add("WMin", new Slider("Min minions to W", 4, 1, 10));
            LCM.AddSeparator();
            LCM.Add("R", new CheckBox("Use R (W)", false));
            LCM.Add("RMin", new Slider("Min minions to R (W)", 6, 1, 10));

            //Who uses E in laneclear -.-

        }
        private static void KillstealMenu()
        {
            KSM = Menu.AddSubMenu("Killsteal", "ks");
            KSM.Add("Q", new CheckBox("Use Q KS"));
            KSM.Add("W", new CheckBox("Use W KS"));
            KSM.Add("extW", new CheckBox("Use extended W (or R) to KS (W + Q or E)"));
            KSM.Add("wr", new CheckBox("Use W+R + Q/E to KS"));
            KSM.Add("E", new CheckBox("Use E KS"));
            KSM.Add("R", new CheckBox("Use R KS"));

        }
        private static void AntiGapMenu()
        {
            AntiGapcloserM = Menu.AddSubMenu("Anti Gapcloser", "antigap");
            AntiGapcloserM.Add("E", new CheckBox("E anti-gapclose"));
            AntiGapcloserM.Add("RE", new CheckBox("R (E) anti-gapclose", false));

            HarassMenu();
        }
        private static void HarassMenu()
        {
            HSM = Menu.AddSubMenu("Harass");

            HSM.Add("Q", new CheckBox("Use Q"));
            HSM.Add("QMana", new Slider("Min mana % to Q", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("W", new CheckBox("Use W"));
            HSM.Add("extW", new CheckBox("Extended W (W to gapclose)", false));
            HSM.Add("AutoW", new CheckBox("Auto W2 after harass"));
            HSM.Add("WMana", new Slider("Min mana % to W", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("E", new CheckBox("Use E", false));
            HSM.Add("EMana", new Slider("Min mana % to E", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("Auto", new KeyBind("Auto harass", false, KeyBind.BindTypes.PressToggle, 'N'));

        }
        private static void Flee()
        {
            FLM = Menu.AddSubMenu("Flee");

            FLM.Add("E", new CheckBox("Use E"));
            FLM.Add("W", new CheckBox("Use W to cursor pos"));
            FLM.Add("R", new CheckBox("Use R to cursor pos", false));
        }
        private static void MiscMenu()
        {
            Misc = Menu.AddSubMenu("Misc");

            Misc.Add("AutoW", new Slider("Auto W2 when your health is lower than", 20, 0, 100));
            Misc.Add("Clone", new CheckBox("Control clone"));
            var a = Misc.Add("CloneMode", new Slider("", 1, 0, 2));
            a.DisplayName = Modes[a.CurrentValue];
            a.OnValueChange += delegate
                (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs Args)
            {
                sender.DisplayName = Modes[Args.NewValue];
            };
        }
    }
}
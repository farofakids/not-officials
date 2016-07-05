namespace TwistedBuddy
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Program
    {
        public static Spell.Skillshot Q;

        public static Spell.Active W, E, R;

        public const string ChampionName = "TwistedFate";

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.BaseSkinName != ChampionName)
            {
                return;
            }

            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40)
            {
                AllowedCollisionCount = int.MaxValue
            }; 
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);

            // Menu
            Essentials.MainMenu = MainMenu.AddMenu("Twisted Fate", "TwistedFate");

            // Card Selector Menu
            Essentials.CardSelectorMenu = Essentials.MainMenu.AddSubMenu("Card Selector Menu", "csMenu");
            Essentials.CardSelectorMenu.AddGroupLabel("Card Selector Settings");
            Essentials.CardSelectorMenu.Add("useY", new KeyBind("Use Yellow Card", false, KeyBind.BindTypes.HoldActive, "W".ToCharArray()[0]));
            Essentials.CardSelectorMenu.Add("useB", new KeyBind("Use Blue Card", false, KeyBind.BindTypes.HoldActive, "E".ToCharArray()[0]));
            Essentials.CardSelectorMenu.Add("useR", new KeyBind("Use Red Card", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));

            // Combo
            Essentials.ComboMenu = Essentials.MainMenu.AddSubMenu("Combo Menu", "comboMenu");
            Essentials.ComboMenu.AddGroupLabel("Combo Settings");
            Essentials.ComboMenu.Add("useQ", new CheckBox("Use Q in Combo"));
            Essentials.ComboMenu.Add("useCard", new CheckBox("Use W in Combo"));
            Essentials.ComboMenu.Add("useQStun", new CheckBox("Use Q only if Stunned", false));
            Essentials.ComboMenu.Add("qPred", new Slider("Q HitChance %", 75));
            Essentials.ComboMenu.Add("wSlider", new Slider("Range from enemy before picking card (Not including the additional range)", 300, 0, 10000));
            Essentials.ComboMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 25));
            Essentials.ComboMenu.AddGroupLabel("Card Selector Configuration");
            Essentials.ComboMenu.Add("chooser", new ComboBox("Card Select Mode", new[] { "Smart", "Blue", "Red", "Yellow" }));
            Essentials.ComboMenu.Add("enemyW", new Slider("How many enemies before selecting Red Card (SMART)", 4, 1, 5));
            Essentials.ComboMenu.Add("manaW", new Slider("How much mana before selecting Blue Card (SMART)", 25));

            // Harass Menu
            Essentials.HarassMenu = Essentials.MainMenu.AddSubMenu("Harass Menu", "harassMenu");
            Essentials.HarassMenu.AddGroupLabel("Harass Settings");
            Essentials.HarassMenu.Add("useQ", new CheckBox("Use Q in Harass"));
            Essentials.HarassMenu.Add("useCard", new CheckBox("Use W in Harass"));
            Essentials.HarassMenu.Add("qPred", new Slider("Q HitChance %", 75));
            Essentials.HarassMenu.Add("wSlider", new Slider("Range from enemy before picking card (Not including the additional range)", 300, 0, 10000));
            Essentials.HarassMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 25));
            Essentials.HarassMenu.AddGroupLabel("Card Selector Configuration");
            Essentials.HarassMenu.Add("chooser", new ComboBox("Card Select Mode", new[] { "Smart", "Blue", "Red", "Yellow" }));
            Essentials.HarassMenu.Add("enemyW", new Slider("How many enemies before selecting Red Card (SMART)", 3, 1, 5));
            Essentials.HarassMenu.Add("manaW", new Slider("How much mana before selecting Blue Card (SMART)", 25));

            // Lane Clear Menu
            Essentials.LaneClearMenu = Essentials.MainMenu.AddSubMenu("Lane Clear", "laneclearMenu");
            Essentials.LaneClearMenu.AddGroupLabel("LaneClear Settings");
            Essentials.LaneClearMenu.Add("useQ", new CheckBox("Use Q in LaneClear", false));
            Essentials.LaneClearMenu.Add("useCard", new CheckBox("Use W in LaneClear"));
            Essentials.LaneClearMenu.Add("qPred", new Slider("Use Q if Hit x Minions", 3, 1, 5));
            Essentials.LaneClearMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 50));
            Essentials.LaneClearMenu.AddGroupLabel("Card Selector Configuration");
            Essentials.LaneClearMenu.Add("chooser", new ComboBox("Card Select Mode", new[] { "Smart", "Blue", "Red", "Yellow" }));
            Essentials.LaneClearMenu.Add("enemyW", new Slider("How many enemies before selecting Red Card (SMART)", 2, 1, 5));
            Essentials.LaneClearMenu.Add("manaW", new Slider("How much mana before selecting Blue Card (SMART)", 25));

            // Jungle Clear Menu
            Essentials.JungleClearMenu = Essentials.MainMenu.AddSubMenu("Jungle Clear Menu", "jgMenu");
            Essentials.JungleClearMenu.AddGroupLabel("JungleClear Settings");
            Essentials.JungleClearMenu.Add("useQ", new CheckBox("Use Q in JungleClear", false));
            Essentials.JungleClearMenu.Add("useCard", new CheckBox("Use W in JungleClear"));
            Essentials.JungleClearMenu.Add("qPred", new Slider("Q HitChance %", 75));
            Essentials.JungleClearMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 50));
            Essentials.JungleClearMenu.AddGroupLabel("Card Selector Configuration");
            Essentials.JungleClearMenu.Add("chooser", new ComboBox("Card Select Mode", new[] {"Smart", "Blue", "Red", "Yellow"}));
            Essentials.JungleClearMenu.Add("enemyW", new Slider("How many enemies before selecting Red Card (SMART)", 2, 1, 5));
            Essentials.JungleClearMenu.Add("manaW", new Slider("How much mana before selecting Blue Card (SMART)", 25));

            // Kill Steal Menu
            Essentials.KillStealMenu = Essentials.MainMenu.AddSubMenu("Kill Steal Menu", "ksMenu");
            Essentials.KillStealMenu.AddGroupLabel("KillSteal Settings");
            Essentials.KillStealMenu.Add("useQ", new CheckBox("Use Q to KS"));
            Essentials.KillStealMenu.Add("qPred", new Slider("Q HitChance %", 75));
            Essentials.KillStealMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 15));
            Essentials.KillStealMenu.AddSeparator();

            // Drawing Menu
            Essentials.DrawingMenu = Essentials.MainMenu.AddSubMenu("Drawing Menu", "drawMenu");
            Essentials.DrawingMenu.AddGroupLabel("Drawing Settings");
            Essentials.DrawingMenu.Add("drawQ", new CheckBox("Draw Q Range"));
            Essentials.DrawingMenu.Add("drawR", new CheckBox("Draw R Range"));
            Essentials.DrawingMenu.AddSeparator();

            // Misc Menu
            Essentials.MiscMenu = Essentials.MainMenu.AddSubMenu("Misc Menu", "miscMenu");
            Essentials.MiscMenu.AddGroupLabel("Misc Settings");
            Essentials.MiscMenu.Add("autoQ", new CheckBox("Automatically Q's a Stunned Target"));
            Essentials.MiscMenu.Add("qPred", new Slider("Q HitChance %", 75));
            Essentials.MiscMenu.Add("autoY", new CheckBox("Automatically select Yellow Card when R"));
            Essentials.MiscMenu.Add("delay", new Slider("Delay Card Choosing", 175, 175, 345));

            Chat.Print("TwistedBuddy 2.2.0.3 - By KarmaPanda", System.Drawing.Color.Green);

            // Events
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var target = args.Target as AIHeroClient;

            if (target == null || args.SData.Name != "goldcardpreattack" || !Q.IsReady() || !Q.IsInRange(target))
            {
                return;
            }

            if (Essentials.MiscMenu["autoQ"].Cast<CheckBox>().CurrentValue)
            {
                var pred = Q.GetPrediction(target);

                if (pred != null && pred.HitChancePercent >= Essentials.MiscMenu["qPred"].Cast<Slider>().CurrentValue)
                {
                    Q.Cast(pred.CastPosition);
                }
                else
                {
                    Essentials.UseStunQ = true;
                    Essentials.StunnedTarget = target;
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                Essentials.ComboMenu["useQStun"].Cast<CheckBox>().CurrentValue)
            {
                var pred = Q.GetPrediction(target);

                if (pred != null && pred.HitChancePercent >= Essentials.ComboMenu["qPred"].Cast<Slider>().CurrentValue)
                {
                    Q.Cast(pred.CastPosition);
                }
                else
                {
                    Essentials.UseStunQ = true;
                    Essentials.StunnedTarget = target;
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.ToLower() == "gate" && Essentials.MiscMenu["autoY"].Cast<CheckBox>().CurrentValue)
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Essentials.DrawingMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance != null)
                {
                    Circle.Draw(Q.IsReady() ? Color.Green : Color.Red, Q.Range, Player.Instance.Position);
                }
            }

            if (!Essentials.DrawingMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (Player.Instance != null)
            {
                Circle.Draw(R.IsReady() ? Color.Green : Color.Red, R.Range, Player.Instance.Position);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Player.Instance.IsRecalling() && !Player.Instance.IsInShopRange())
            {
                var useY = Essentials.CardSelectorMenu["useY"].Cast<KeyBind>().CurrentValue;
                var useB = Essentials.CardSelectorMenu["useB"].Cast<KeyBind>().CurrentValue;
                var useR = Essentials.CardSelectorMenu["useR"].Cast<KeyBind>().CurrentValue;

                if (useY)
                {
                    CardSelector.StartSelecting(Cards.Yellow);
                }

                if (useB)
                {
                    CardSelector.StartSelecting(Cards.Blue);
                }

                if (useR)
                {
                    CardSelector.StartSelecting(Cards.Red);
                }

                StateManager.AutoQ();
            }

            if (Essentials.KillStealMenu["useQ"].Cast<CheckBox>().CurrentValue)
            {
                StateManager.KillSteal();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateManager.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateManager.LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateManager.JungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateManager.Harass();
            }
        }
    }
}

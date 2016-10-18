using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Veigarino
{
    class ΤοΠιλλ
    {
        public static AIHeroClient myhero { get { return ObjectManager.Player; } }
        private static Menu menu, combo, harass, laneclear, misc, draw, pred, jungleclear;
        public static Item potion { get; private set; }
        public static Spell.Skillshot Q, W, E;
        public static Spell.Targeted R;

        private static void Main(string[] args) { Loading.OnLoadingComplete += OnLoad; }
        public static void OnLoad(EventArgs arg)
        {
            if (Player.Instance.ChampionName != "Veigar") { return; }
            Chat.Print("<font color='#0040FF'>T7</font><font color='#A901DB'> Veigar</font> : Loaded!(v1.7)");
            Chat.Print("<font color='#04B404'>By </font><font color='#FF0000'>T</font><font color='#FA5858'>o</font><font color='#FF0000'>y</font><font color='#FA5858'>o</font><font color='#FF0000'>t</font><font color='#FA5858'>a</font><font color='#0040FF'>7</font><font color='#FF0000'> <3 </font>");
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            DatMenu();
            Game.OnTick += OnTick;
            Player.LevelSpell(SpellSlot.Q);
            potion = new Item((int)ItemId.Health_Potion);
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 2000, 70);
            Q.AllowedCollisionCount = 1;
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 1350, 0, 112);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Circular, 500, 0, 375);
            R = new Spell.Targeted(SpellSlot.R, 650);

        }
        private static void OnTick(EventArgs args)
        {
            if (myhero.IsDead) return;

            var flags = Orbwalker.ActiveModesFlags;

            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) || check(harass, "autoH") && myhero.ManaPercent > slider(harass, "minMH"))
            {
                Harass();
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) || check(laneclear, "AutoL") && slider(laneclear, "LcM") <= myhero.ManaPercent)
            {
                Laneclear();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear) || check(laneclear, "AutoL") && slider(laneclear, "LcM") <= myhero.ManaPercent)
            {
                Laneclear();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear) && myhero.ManaPercent > slider(jungleclear, "JMIN"))
            { 
                Jungleclear(); 
            }

            Misc();
            LastHitQ();
        }

        private static int comb(Menu submenu, string sig)
        {
            return submenu[sig].Cast<ComboBox>().CurrentValue;
        }
        private static bool check(Menu submenu, string sig)
        {
            return submenu[sig].Cast<CheckBox>().CurrentValue;
        }
        private static int slider(Menu submenu, string sig)
        {
            return submenu[sig].Cast<Slider>().CurrentValue;
        }

        private static float QDamage(Obj_AI_Base target)
        {
            return myhero.CalculateDamageOnUnit(target, DamageType.Magical,
             new[] { 70, 110, 150, 190, 230 }[Q.Level] + (Player.Instance.TotalMagicalDamage * 0.6f));
        }
        private static float WDamage(Obj_AI_Base target)
        {
            return myhero.CalculateDamageOnUnit(target, DamageType.Magical,
             new[] { 100, 150, 200, 250, 300 }[W.Level] + (Player.Instance.TotalMagicalDamage * 1f));
        }
        private static float Rdamage(Obj_AI_Base hero)
        {
            return myhero.CalculateDamageOnUnit(hero, DamageType.Magical,
             new[] { 0, 175, 250, 325 }[R.Level] + (Player.Instance.TotalMagicalDamage * 0.75f));
        }
        private static float RdamageMAX(Obj_AI_Base hero)
        {
            return myhero.CalculateDamageOnUnit(hero, DamageType.Magical,
             new[] { 0, 350, 500, 650 }[R.Level] + (Player.Instance.TotalMagicalDamage * 1.5f));
        }
        private static float ComboDMG(AIHeroClient target)
        {
            if (target != null)
            {
                float cdmg = 0;

                if (Q.IsReady() && combo["useQ"].Cast<CheckBox>().CurrentValue) { cdmg += QDamage(target); }
                if (W.IsReady() && combo["useW"].Cast<CheckBox>().CurrentValue) { cdmg += WDamage(target); }
                if (target.HealthPercent > 33 && R.IsReady() && combo["useR"].Cast<CheckBox>().CurrentValue) { cdmg += Rdamage(target); }
                else { cdmg += RdamageMAX(target); };

                return cdmg;
            }
            return 0;
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical, Player.Instance.Position);

            if (target != null)
            {
                var Qpred = Q.GetPrediction(target);
                var Wpred = W.GetPrediction(target);

                if (check(combo, "useQ") && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie
                    && !target.IsInvulnerable)
                {
                    if (!Qpred.Collision || Qpred.CollisionObjects.Count() < 2)
                    {
                        Q.Cast(Qpred.CastPosition);
                    }
                }

                if (check(combo, "useW") && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie
                    && !target.IsInvulnerable)
                {
                    switch (comb(harass, "HWMODE"))
                    {
                        case 0:
                            if (Wpred.HitChancePercent >= slider(pred, "Whit") || Wpred.HitChance == HitChance.Immobile ||
                               (target.HasBuffOfType(BuffType.Slow) && Wpred.HitChance == HitChance.High))
                            {
                                W.Cast(Wpred.CastPosition);
                            }
                            break;
                        case 1:
                            W.Cast(target.Position);
                            break;
                        case 2:
                            if (target.HasBuffOfType(BuffType.Stun))
                            {
                                W.Cast(target.Position);
                            }
                            else if (Wpred.HitChance == HitChance.Immobile)
                            {
                                W.Cast(Wpred.CastPosition);
                            }
                            break;
                    }

                }
            }
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical, Player.Instance.Position);
                if (target != null)
            {

                var Qpred = Q.GetPrediction(target);
                var Wpred = W.GetPrediction(target);
                var Epred = Prediction.Position.PredictUnitPosition(target, 500);
                var Epred2 = E.GetPrediction(target);

                if (check(combo, "useQ") && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie
                    && !target.IsInvulnerable)
                {
                    if (!Qpred.Collision || Qpred.CollisionObjects.Count() < 2)
                    {
                        Q.Cast(Qpred.CastPosition);
                    }
                }
                
                if (check(combo, "useE") && E.IsReady() && target.IsValidTarget(E.Range - 30) && !target.IsZombie
                    && !target.IsInvulnerable)
                {
                    if (check(combo, "Es") && target.HasBuffOfType(BuffType.Stun)) return;
                    switch (comb(combo, "CEMODE"))
                    {
                        case 0:
                            if (Epred2.HitChancePercent >= slider(pred, "Ehit")) E.Cast(Epred2.CastPosition);
                            break;
                        case 1:
                            if (Epred.Distance(myhero.Position) < E.Range)
                            {
                                switch (target.IsFleeing)
                                {
                                    case true:
                                        E.Cast(Epred.To3D().Shorten(myhero.Position, 182));
                                        break;
                                    case false:
                                        E.Cast(Epred.Extend(myhero.Position, 182).To3D());
                                        break;
                                }
                            }
                            break;
                        case 2:
                            if (myhero.CountEnemiesInRange(E.Range + (E.Radius / 2)) >= slider(combo, "CEAOE"))
                            {
                                foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.IsValid && !x.IsAlly &&
                                                                                         x.Distance(myhero.Position) <= E.Range))
                                {
                                    if (enemy.CountEnemiesInRange(175) >= slider(combo, "CEAOE")) E.Cast(enemy.Position);
                                }
                            }
                            break;
                    }
                }

                if (check(combo, "useW") && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie
                    && !target.IsInvulnerable)
                {
                    switch (comb(combo, "CWMODE"))
                    {
                        case 0:
                            if (Wpred.HitChancePercent >= slider(pred, "Whit") || Wpred.HitChance == HitChance.Immobile ||
                               (target.HasBuffOfType(BuffType.Slow) && Wpred.HitChance == HitChance.High))
                            {
                                W.Cast(Wpred.CastPosition);
                            }
                            break;
                        case 1:
                            W.Cast(target.Position);
                            break;
                        case 2:
                            if (target.HasBuffOfType(BuffType.Stun))
                            {
                                W.Cast(target.Position);
                            }
                            else if (Wpred.HitChance == HitChance.Immobile)
                            {
                                W.Cast(Wpred.CastPosition);
                            }
                            break;
                    }
                }

                if (target.HealthPercent > 33 && check(combo, "useR") && R.IsReady() &&
                    R.IsInRange(target.Position) && ComboDMG(target) > target.Health &&
                    Rdamage(target) > target.Health && !target.HasBuffOfType(BuffType.SpellImmunity) && !target.IsInvulnerable && !target.HasUndyingBuff())
                {
                    if ((float)(ComboDMG(target) - Rdamage(target)) > target.Health) return;
                    R.Cast(target);
                }
                else 
                 {
                    if (check(combo, "useR") && R.IsReady() &&
                        R.IsInRange(target.Position) && ComboDMG(target) > target.Health &&
                     RdamageMAX(target) > target.Health && !target.HasBuffOfType(BuffType.SpellImmunity) && !target.IsInvulnerable && !target.HasUndyingBuff())
                    {
                        if ((float)(ComboDMG(target) - RdamageMAX(target)) > target.Health) return;
                        R.Cast(target);
                    }
                }
            }
        }
        private static void Laneclear()
        {
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, myhero.Position, W.Range);

            if (check(laneclear, "LQ") && Q.IsReady() && !laneclear["Qlk"].Cast<KeyBind>().CurrentValue)
            {
                var Qpred = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minion, Q.Width, (int)Q.Range);

                if (Qpred.HitNumber >= 1) Q.Cast(Qpred.CastPosition);
            }

            if (check(laneclear, "LW") && minion != null && W.IsReady())
            {
                var Wpred = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minion, W.Width, (int)W.Range);

                if (Wpred.HitNumber >= slider(laneclear, "Wmm")) W.Cast(Wpred.CastPosition);
            }
        }
        private static void Jungleclear()
        {
            var Monsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(myhero.Position, 1800f);

            if (Monsters != null)
            {
                
                if (check(jungleclear, "JQ") && Q.IsLearned && Q.IsReady())
                {
                    switch (comb(jungleclear, "JQMODE"))
                    {
                        case 0:
                            foreach (var monster in Monsters.Where(x => !x.IsDead && x.IsValidTarget(Q.Range) && x.Health > 30))
                            {
                                Q.Cast(monster.Position);
                            }
                            break;
                        case 1:
                            foreach (var monster in Monsters.Where(x => !x.IsDead && x.IsValidTarget(Q.Range) && x.Health > 30 &&
                                                                        !x.Name.ToLower().Contains("mini")))
                            {
                                var pred = Q.GetPrediction(monster);
                                if (pred.CollisionObjects.Count() < 2) Q.Cast(pred.CastPosition);
                            }
                            break;
                    }
                }

                if (check(jungleclear, "JW") && W.IsLearned && W.IsReady())
                {
                    var mobs = Monsters.Where(x => !x.IsDead && x.IsValidTarget(W.Range) && x.Health > 30);


                    if (mobs != null && mobs.Count() >= slider(jungleclear, "JWMIN"))
                    {
                        var pred = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(mobs, W.Width, (int)W.Range);

                        if (pred.HitNumber >= slider(jungleclear, "JWMIN")) W.Cast(pred.CastPosition);                         
                    }
                }

                if (check(jungleclear, "JE") && E.IsLearned && E.IsReady())
                {
                    var mobs = Monsters.Where(x => !x.IsDead && x.IsValidTarget(E.Range) && x.Health > 10);


                    if (mobs != null && mobs.Count() >= slider(jungleclear, "JEMIN"))
                    {
                        var pred = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(mobs, E.Width, (int)E.Range);

                        if (pred.HitNumber >= slider(jungleclear, "JEMIN")) E.Cast(pred.CastPosition);
                    }
                }
            }
        }
        private static void Misc()
        {

            if (check(misc, "KSJ") && W.IsLearned && W.IsReady())
            {
                foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => !x.Name.ToLower().Contains("mini") && !x.IsDead &&
                                                                                                   x.Health > 200 && x.IsValidTarget(W.Range) &&
                                                                                                   (x.Name.ToLower().Contains("dragon") ||
                                                                                                    x.Name.ToLower().Contains("baron") ||
                                                                                                    x.Name.ToLower().Contains("herald"))))
                {
                    var pred = W.GetPrediction(monster);

                    if (Prediction.Health.GetPrediction(monster, W.CastDelay + 100) > monster.Health)
                    {
                        switch (monster.Name.ToLower().Contains("herald"))
                        {
                            case true:
                                if (pred.HitChancePercent >= 85) W.Cast(pred.CastPosition);
                                break;
                            case false:
                                W.Cast(monster.Position);
                                break;
                        }
                    }
                }
            }

            var target = TargetSelector.GetTarget(1000, DamageType.Magical, Player.Instance.Position);

            if (target != null)
            {
                var Qpred = Q.GetPrediction(target);
                var Wpred = W.GetPrediction(target);

                if (check(misc, "ksQ") && QDamage(target) > target.Health && !target.HasUndyingBuff() &&
                    target.IsValidTarget(Q.Range - 10) && Q.IsReady() && !target.IsInvulnerable)
                {
                    if (target.HasBuffOfType(BuffType.Stun) ||
                        Qpred.HitChancePercent >= slider(pred, "Qhit"))
                    {
                        Q.Cast(target.Position);
                    }
                }
                if (check(misc, "ksW") && WDamage(target) > target.Health && !target.HasUndyingBuff() &&
                    target.IsValidTarget(W.Range) && W.IsReady() && !target.IsInvulnerable)
                {

                    if (Wpred.HitChancePercent >= slider(pred, "Whit") || Wpred.HitChance == HitChance.Immobile ||
                        (target.HasBuffOfType(BuffType.Slow) && Wpred.HitChance == HitChance.High))
                    {
                        W.Cast(Wpred.CastPosition);
                    }
                }

                if (target.HealthPercent > 33 && check(misc, "ksR") && Rdamage(target) > target.Health &&
                    target.IsValidTarget(R.Range) && R.IsReady() &&
                    !target.IsInvulnerable && !target.HasUndyingBuff() && !target.HasBuffOfType(BuffType.SpellImmunity))
                {
                    switch (target.HasBuffOfType(BuffType.SpellShield))
                    {
                        case true:
                            if ((target.MagicShield + target.Health) < Rdamage(target)) R.Cast(target);
                            else break;
                            break;
                        case false:
                            R.Cast(target);
                            break;
                    }
                }
                else
                {
                    if (check(misc, "ksR") && RdamageMAX(target) > target.Health &&
                         target.IsValidTarget(R.Range) && R.IsReady() &&
                         !target.IsInvulnerable && !target.HasUndyingBuff() && !target.HasBuffOfType(BuffType.SpellImmunity))
                    {
                        switch (target.HasBuffOfType(BuffType.SpellShield))
                        {
                            case true:
                                if ((target.MagicShield + target.Health) < RdamageMAX(target)) R.Cast(target);
                                else break;
                                break;
                            case false:
                                R.Cast(target);
                                break;
                        }
                    }
                }
            }
        }
        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender != null && sender.IsEnemy && E.IsInRange(sender.Position) && E.IsReady() &&
                sender.IsValidTarget() && misc["gapmode"].Cast<ComboBox>().CurrentValue != 0)
            {
                var gpred = E.GetPrediction(sender);
                switch (misc["gapmode"].Cast<ComboBox>().CurrentValue)
                {
                    case 1:
                        if (!sender.IsFleeing && sender.IsFacing(myhero))
                            E.Cast(myhero.Position);
                        break;
                    case 2:
                        if (gpred != null && gpred.HitChancePercent >= pred["Ehit"].Cast<Slider>().CurrentValue)
                        {
                            E.Cast(gpred.CastPosition);
                        }
                        break;
                }
            }
        }
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender != null && sender.IsEnemy && E.IsInRange(sender.Position) && E.IsReady() &&
                sender.IsValidTarget() && misc["gapmode"].Cast<ComboBox>().CurrentValue != 0 && e.DangerLevel == DangerLevel.High)
            {
                var gpred = E.GetPrediction(sender);
                switch (misc["gapmode"].Cast<ComboBox>().CurrentValue)
                {
                    case 1:
                        if (!sender.IsFleeing && sender.IsFacing(myhero))
                            E.Cast(myhero.Position);
                        break;
                    case 2:
                        if (gpred != null && gpred.HitChancePercent >= pred["Ehit"].Cast<Slider>().CurrentValue)
                        {
                            E.Cast(gpred.CastPosition);
                        }
                        break;
                }
            }
        }
        private static void DatMenu()
        {

            menu = MainMenu.AddMenu("T7 Veigar", "veigarxd");
            combo = menu.AddSubMenu("Combo", "combo");
            harass = menu.AddSubMenu("Harass", "harass");        
            laneclear = menu.AddSubMenu("Laneclear", "lclear");
            jungleclear = menu.AddSubMenu("Jungleclear", "jclear");
            draw = menu.AddSubMenu("Drawings", "draw");
            misc = menu.AddSubMenu("Misc", "misc");
            pred = menu.AddSubMenu("Prediction", "pred");

            menu.AddGroupLabel("Welcome to T7 Veigar And Thank You For Using!");
            menu.AddLabel("Version 1.7 15/7/2016");
            menu.AddLabel("Author: Toyota7");
            menu.AddSeparator();
            menu.AddLabel("Please Report Any Bugs And If You Have Any Requests Feel Free To PM Me <3");

            combo.AddGroupLabel("Spells");
            combo.Add("useQ", new CheckBox("Use Q in Combo", true));
            combo.Add("useW", new CheckBox("Use W in Combo", true));
            combo.Add("useE", new CheckBox("Use E in Combo", true));
            combo.Add("useR", new CheckBox("Use R in Combo", true));
            combo.AddSeparator();
            combo.AddLabel("W Mode:");
            combo.Add("CWMODE", new ComboBox("Select Mode", 0, "With Prediciton", "Without Prediction", "Only On Stunned Enemies"));
            combo.AddSeparator();
            combo.AddLabel("E Options:");
            combo.Add("CEMODE", new ComboBox("E Mode: ", 0, "Target On The Center", "Target On The Edge(stun)", "AOE"));
            combo.Add("CEAOE", new Slider("Min Champs For AOE Function", 2, 1, 5));
            combo.Add("Es", new CheckBox("Dont Use E On Immobile Enemies", true));

            harass.AddGroupLabel("Spells");
            harass.Add("hQ", new CheckBox("Use Q", true));
            harass.Add("hW", new CheckBox("Use W", false));
            harass.AddSeparator();
            harass.AddLabel("W Mode:");
            harass.Add("HWMODE", new ComboBox("Select Mode", 2, "With Prediciton", "Without Prediction(Not Recommended)", "Only On Stunned Enemies"));
            harass.AddSeparator();
            harass.AddLabel("Min Mana To Harass");
            harass.Add("minMH", new Slider("Stop Harass At % Mana", 40, 0, 100));
            harass.AddSeparator();
            harass.AddLabel("Auto Harass");
            harass.Add("autoH", new CheckBox(" Use Auto harass", false));

            laneclear.AddGroupLabel("Spells");
            laneclear.Add("LQ", new CheckBox("Use Q", true));
            laneclear.Add("LW", new CheckBox("Use W", false));
            laneclear.AddSeparator();
            laneclear.AddLabel("Q Stacking");
            laneclear.Add("Qlk", new CheckBox("Auto Stacking Q"));
            laneclear.Add("autoQmana", new Slider("Last Hit Mana %", 35));
            laneclear.AddSeparator();
            laneclear.AddLabel("Min W Minions");
            laneclear.Add("Wmm", new Slider("Min minions to use W", 2, 1, 6));
            laneclear.AddSeparator();
            laneclear.AddLabel("Stop Laneclear At % Mana");
            laneclear.Add("LcM", new Slider("%", 50, 0, 100));
            laneclear.AddSeparator();
            laneclear.AddLabel("Auto Laneclear");
            laneclear.Add("AutoL", new CheckBox("Auto Laneclear", false));

            jungleclear.AddGroupLabel("Spells");
            jungleclear.Add("JQ", new CheckBox("Use Q", false));
            jungleclear.Add("JQMODE", new ComboBox("Q Mode", 1, "All Monsters", "Big Monsters"));
            jungleclear.AddSeparator();
            jungleclear.Add("JW", new CheckBox("Use W", true));
            jungleclear.Add("JWMIN", new Slider("Min Monsters To Hit With W", 2, 1, 4));
            jungleclear.AddSeparator();
            jungleclear.Add("JE", new CheckBox("Use E To Trap Monsters", false));
            jungleclear.Add("JEMIN", new Slider("Min Monsters To Trap With E", 3, 1, 4));
            jungleclear.AddSeparator();
            jungleclear.Add("JMIN", new Slider("Min Mana % To Jungleclear", 10, 0, 100));

            misc.AddGroupLabel("Killsteal");
            misc.Add("ksQ", new CheckBox("Killsteal with Q", false));
            misc.Add("ksW", new CheckBox("Killsteal with W(With Prediction)", false));
            misc.Add("ksR", new CheckBox("Killsteal with R", false));
            misc.Add("autoign", new CheckBox("Auto Ignite If Killable", true));
            misc.AddSeparator();
            misc.Add("KSJ", new CheckBox("Steal Dragon/Baron/Rift Herald With W", false));
            misc.AddSeparator();
            misc.AddGroupLabel("Gapcloser");
            misc.Add("gapmode", new ComboBox("Use E On Gapcloser                                               Mode:", 2, "Off", "Self", "Enemy(Pred)"));
            misc.AddSeparator();

            pred.AddGroupLabel("Q HitChance");
            pred.Add("Qhit", new Slider("% Hitchance", 85, 0, 100));
            pred.AddSeparator();
            pred.AddGroupLabel("W HitChance");
            pred.Add("Whit", new Slider("% Hitchance", 85, 0, 100));
            pred.AddSeparator();
            pred.AddGroupLabel("E HitChance");
            pred.Add("Ehit", new Slider("% Hitchance", 85, 0, 100));

        }


        private static void LastHitQ()
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                return;
            }
            if (!Q.IsReady())
            {
                return;
            }
            if (Player.Instance.ManaPercent < laneclear["autoQmana"].Cast<Slider>().CurrentValue)
            {
                return;
            }
            if (laneclear["Qlk"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var minion in
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range)
                    .Where(x => Player.Instance.GetSpellDamage(x, SpellSlot.Q) > x.Health))
                {
                        Q.Cast(minion);
                }
            }
        }
    }

}

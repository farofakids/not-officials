using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace Farofakids_MissFortune
{
    internal class MODES
    {
        private static float RCastTime = 0;
        private static bool IsWindingUp = false;

        public static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (SPELLS.E.IsReady() && MENUS.AGC && Player.Instance.Mana >
             Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
             Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana)
            {
                var Target = gapcloser.Sender;
                if (Target.IsValidTarget(SPELLS.E.Range))
                {
                    SPELLS.E.Cast(gapcloser.End);
                }
                return;
            }
            return;
        }

        internal static void Obj_AI_Base_OnSpellCas(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                IsWindingUp = false;
            }
        }

        internal static void Game_OnUpdate(EventArgs args)
        {
            if (MENUS.disableBlock)
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
                return;
            }
            else if (Player.Instance.Spellbook.IsChanneling || Game.Time - RCastTime < 0.3)
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                return;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;

                if (SPELLS.R.IsReady() && MENUS.useR)
                {
                    var t = TargetSelector.GetTarget(SPELLS.R.Range, DamageType.Physical);
                    if (t.IsValidTarget(SPELLS.R.Range))
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                        return;
                    }
                }
            }

            if (MENUS.newTarget)
            {
                var orbT = TargetSelector.GetTarget(600, DamageType.Physical);

                AIHeroClient t2 = null;

                if (orbT != null && orbT is AIHeroClient)
                    t2 = (AIHeroClient)orbT;

                if (t2.IsValidTarget())
                {
                    var ta = ObjectManager.Get<AIHeroClient>().Where(enemy =>
                        enemy.IsValidTarget() && Player.Instance.IsInAutoAttackRange(enemy)
                            && (enemy.Health < Player.Instance.GetAutoAttackDamage(enemy) * 2)).FirstOrDefault();

                    if (ta != null)
                    Orbwalker.ForcedTarget = ta;

                }
            }

            if (!IsWindingUp && SPELLS.Q.IsReady() && MENUS.autoQ)
                LogicQ();

            if (!IsWindingUp && SPELLS.E.IsReady() && MENUS.autoE)
                LogicE();
            if (!IsWindingUp && SPELLS.R.IsReady() && MENUS.autoR)
                LogicR();
        }

        private static void LogicR()
        {
            if (Player.Instance.IsUnderTurret() && MENUS.Rturrent)
                return;

            var t = TargetSelector.GetTarget(SPELLS.R.Range, DamageType.Physical);

            if (t.IsValidTarget(SPELLS.R.Range))
            {
                var rDmg = Player.Instance.GetSpellDamage(t, SpellSlot.R) * new double[] { 0.5, 0.75, 1 }[SPELLS.R.Level];

                if (Player.Instance.CountEnemiesInRange(700) == 0 && t.CountAlliesInRange(400) == 0)
                {
                    var tDis = Player.Instance.Distance(t.ServerPosition);
                    if (rDmg * 7 > t.Health && tDis < 800)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    else if (rDmg * 6 > t.Health && tDis < 900)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    else if (rDmg * 5 > t.Health && tDis < 1000)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    else if (rDmg * 4 > t.Health && tDis < 1100)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    else if (rDmg * 3 > t.Health && tDis < 1200)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    else if (rDmg > t.Health && tDis < 1300)
                    {
                        SPELLS.R.Cast(t);
                        RCastTime = Game.Time;
                    }
                    return;
                }
                if (rDmg * 8 > t.Health && rDmg * 2 < t.Health && Player.Instance.CountEnemiesInRange(300) == 0 /*&& !OktwCommon.CanMove(t)*/)
                {
                    SPELLS.R.Cast(t);
                    RCastTime = Game.Time;
                    return;
                }
            }
        }

        private static void LogicE()
        {
            var t = TargetSelector.GetTarget(SPELLS.E.Range, DamageType.Magical);
            if (t.IsValidTarget())
            {
                var eDmg = Player.Instance.GetSpellDamage(t, SpellSlot.E);
                if (eDmg > t.Health)
                    SPELLS.E.Cast(t);
                else if (eDmg + Player.Instance.GetSpellDamage(t, SpellSlot.Q) > t.Health && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana)
                    SPELLS.E.Cast(t);
                else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana)
                {
                    if (Player.Instance.IsInAutoAttackRange(t) || Player.Instance.CountEnemiesInRange(300) > 0 || t.CountEnemiesInRange(250) > 1)
                        SPELLS.E.Cast(t);
                    else
                    {
                        foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(SPELLS.E.Range) /*&& Orbwalker.CanMove(enemy)*/))
                            SPELLS.E.Cast(enemy);
                    }
                }
            }
        }

        private static void LogicQ()
        {
            var t = TargetSelector.GetTarget(SPELLS.Q.Range, DamageType.Physical);
            var t1 = TargetSelector.GetTarget(SPELLS.Q1.Range, DamageType.Physical);
            if (t.IsValidTarget(SPELLS.Q.Range) && Player.Instance.Distance(t.ServerPosition) > 500)
            {
                var qDmg = Player.Instance.GetSpellDamage(t, SpellSlot.Q);
                if (qDmg + Player.Instance.GetAutoAttackDamage(t) > t.Health)
                    SPELLS.Q.Cast(t);
                else if (qDmg + Player.Instance.GetAutoAttackDamage(t) * 3 > t.Health)
                    SPELLS.Q.Cast(t);
                else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                    SPELLS.Q.Cast(t);
                else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                    SPELLS.Q.Cast(t);
            }
            else if (t1.IsValidTarget(SPELLS.Q1.Range) && MENUS.harasQ && Player.Instance.Distance(t1.ServerPosition) > SPELLS.Q.Range + 50)
            {
                SPELLS.Q1.Width = MENUS.qMinionWidth;

                var poutput = SPELLS.Q1.GetPrediction(t1);
                var col = poutput.CollisionObjects;
                if (col.Count() == 0)
                    return;

                var minionQ = col.Last();
                if (minionQ.IsValidTarget(SPELLS.Q.Range))
                {
                    if (MENUS.killQ && Player.Instance.GetSpellDamage(minionQ, SpellSlot.Q) < minionQ.Health)
                        return;
                    var minionToT = minionQ.Distance(t1.Position);
                    var minionToP = minionQ.Distance(poutput.CastPosition);
                    if (minionToP < 400 && minionToT < 420 && minionToT > 150 && minionToP > 200)
                    {
                        if (Player.Instance.GetSpellDamage(t1, SpellSlot.Q) + Player.Instance.GetAutoAttackDamage(t1) > t1.Health)
                            SPELLS.Q.Cast(col.Last());
                        else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                            SPELLS.Q.Cast(col.Last());
                        else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                            SPELLS.Q.Cast(col.Last());
                    }
                }
            }
        }

        internal static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (target.IsMe)
                return;
            if (!(target is AIHeroClient))
                return;
            var t = target as AIHeroClient;

            if (SPELLS.Q.IsReady() && t.IsValidTarget(SPELLS.Q.Range))
            {
                if (Player.Instance.GetSpellDamage(t, SpellSlot.Q) + Player.Instance.GetAutoAttackDamage(t) * 3 > t.Health)
                    SPELLS.Q.Cast(t);
                else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana > 
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                    SPELLS.Q.Cast(t); 
                else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana)
                    SPELLS.Q.Cast(t);
            }
            if (SPELLS.W.IsReady())
            {
                if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana &&
                    MENUS.autoW)
                    SPELLS.W.Cast();
                else if (Player.Instance.Mana >
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana +
                    Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana
                    && MENUS.harasW)
                    SPELLS.W.Cast();
            }
        }

        internal static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "MissFortuneBulletTime")
            {
                RCastTime = Game.Time;
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
            if (sender.IsMe)
            {
                IsWindingUp = true;
            }
        }

    }

}


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

namespace Farofakids_Nautilus
{
    internal class MODES
    {
        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MENUS.InterruptSpells) return;
            SPELLS.Q.Cast(sender);
            if (sender != null)
            {
                var target = TargetSelector.GetTarget(SPELLS.Q.Range, DamageType.Magical);
                if (target != null)
                    SPELLS.Q.Cast(target);
            }
            if (sender.IsEnemy && args.DangerLevel == DangerLevel.High &&  SPELLS.W.IsReady() && SPELLS.W.IsInRange(sender))
            {
                SPELLS.W.Cast();
            }

        }

        public static void Combo()
        {
            var tsR = TargetSelector.GetTarget(SPELLS.R.Range, DamageType.Magical);

                foreach (
                    var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            enemy => ((TargetSelector.SeletedEnabled && TargetSelector.SelectedTarget == enemy) || MENUS.ComboMenu["UseQ_On" + enemy.ChampionName].Cast<CheckBox>().CurrentValue) &&
                                     enemy.IsValidTarget(SPELLS.Q.Range + 150) &&
                                     !enemy.HasBuffOfType(BuffType.SpellShield)))
                    {
                    if (SPELLS.Q.IsReady() && MENUS.UseQCombo)
                    { 
                        var prediction = SPELLS.Q.GetPrediction(enemy);
                    //if (prediction.HitChance >= MENUS.hitchances[0])
                    if (prediction.HitChance >= HitChance.Medium)
                    {
                        // Cast if hitchance is high enough
                        if (prediction.HitChance >= HitChance.High)
                        {
                            SPELLS.Q.Cast(prediction.CastPosition);
                        }
                    }
                    }

                    if (SPELLS.W.IsReady() && MENUS.UseWCombo && enemy.IsValidTarget(450))
                        SPELLS.W.Cast();

                    if (SPELLS.E.IsReady() && MENUS.UseECombo && enemy.IsValidTarget(SPELLS.E.Range))
                        SPELLS.E.Cast();

                    if (SPELLS.R.IsReady() && MENUS.UseRCombo && enemy.IsValidTarget(SPELLS.R.Range) && tsR.Health >= SPELLS.GetComboDamage(tsR))
                    {
                        var useR = (MENUS.ComboMenu["UseR_On" + enemy.ChampionName].Cast<CheckBox>().CurrentValue);
                        if (useR)
                        {
                            SPELLS.R.Cast(tsR);
                        }
                    }
                }
            }

        public static void Harras()
        {
            if (Player.Instance.ManaPercent < MENUS.HarassMana)
                return;

            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies.Where(
                        enemy => ((TargetSelector.SeletedEnabled && TargetSelector.SelectedTarget == enemy) || MENUS.ComboMenu["UseQ_On" + enemy.ChampionName].Cast<CheckBox>().CurrentValue) &&
                                 enemy.IsValidTarget(SPELLS.Q.Range + 150) &&
                                 !enemy.HasBuffOfType(BuffType.SpellShield)))
            {
                if (SPELLS.Q.IsReady() && MENUS.UseQCombo)
                {
                    var prediction = SPELLS.Q.GetPrediction(enemy);
                    //if (prediction.HitChance >= MENUS.hitchances[0])
                    if (prediction.HitChance >= HitChance.Medium)
                    {
                        // Cast if hitchance is high enough
                        if (prediction.HitChance >= HitChance.High)
                        {
                            SPELLS.Q.Cast(prediction.CastPosition);
                        }
                    }
                }

                if (SPELLS.W.IsReady() && MENUS.UseWCombo && enemy.IsValidTarget(450))
                    SPELLS.W.Cast();

                if (SPELLS.E.IsReady() && MENUS.UseECombo && enemy.IsValidTarget(SPELLS.E.Range))
                    SPELLS.E.Cast();
            }
        }

    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using SharpDX;

namespace AkaYasuo.Modes
{
    partial class Harass
    {
        public static void Harass2()
        {
            if (MenuManager.HarassMenu["QLastHit"].Cast<CheckBox>().CurrentValue && !Variables.HaveQ3
                && !Variables.isDashing)
            {
                var obj =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Variables._Player.ServerPosition, Program.Q.Range)
                        .FirstOrDefault(i => DamageManager._GetQDmg(i) >= i.Health);
                if (obj != null)
                {
                    Program.Q.Cast(obj.ServerPosition);
                }
            }

            var target = TargetSelector.GetTarget(
    !Variables.HaveQ3 ? Variables.QRange : Variables.Q2Range,
    DamageType.Physical);
            if (target != null)
            {
                if (!Variables.HaveQ3)
                {
                    Program.Q.Cast(target);
                }
                else if (Variables.HaveQ3)
                {
                    var hit = -1;
                    var predPos = new Vector3();
                    foreach (var hero in EntityManager.Heroes.Enemies.Where(i => i.IsValidTarget(Variables.Q2Range)))
                    {
                        var pred = Prediction.Position.PredictLinearMissile(hero, Variables.Q2Range, Program.Q2.Width, Program.Q2.CastDelay, Program.Q2.Speed, int.MaxValue, Variables._Player.ServerPosition, true);
                        var pred2 = pred.GetCollisionObjects<AIHeroClient>();
                        if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High && pred2.Length > hit)
                        {
                            hit = pred2.Length;
                            predPos = pred.CastPosition;
                        }
                    }
                    if (predPos.IsValid())
                    {
                        Core.DelayAction(() => Program.Q2.Cast(predPos), 250);
                    }
                    else
                    {
                        Core.DelayAction(() => Program.Q2.Cast(target.Position), 250);
                    }
                }
            }
        }
    }
}


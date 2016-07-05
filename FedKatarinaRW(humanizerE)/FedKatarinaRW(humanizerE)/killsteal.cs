using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;



namespace SupaKS
{
    internal class KillSteal
    {
        public static bool UseQ
        {
            get { return FedKatarinaV2.Program.KillStealMenu["kQ"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool UseW
        {
            get { return FedKatarinaV2.Program.KillStealMenu["kW"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool UseE
        {
            get { return FedKatarinaV2.Program.KillStealMenu["kE"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool UseR
        {
            get { return FedKatarinaV2.Program.KillStealMenu["kR"].Cast<CheckBox>().CurrentValue; }
        }

        public static void Execute()
        {
            if (Player.Instance.IsUnderTurret()) return;

            var killableEnemies =
                EntityManager.Heroes.Enemies.Where(
                    t =>
                        t.IsValidTarget() && !t.HasUndyingBuff() &&
                        FedKatarinaV2.Damage.CalculateDamage(t, UseQ, UseW, UseE, UseR, false) >= t.Health);
            var target = TargetSelector.GetTarget(killableEnemies, DamageType.Magical);

            if (target == null) return;

            if (UseQ &&
                target.Health <= FedKatarinaV2.Damage.CalculateDamage(target, true, false, false, false, false))
            {
                CastQ(target);
            }

            else if (UseW &&
                     target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, false, true, false, false, false))
            {
                CastW(target);
            }

            else if (UseE &&
                     target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, false, false, true, false, false))
            {
                CastE(target);
            }

            else if (target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, false, false, false, true, false))
            {
                CastR(target);
            }

            else if (UseQ && UseW &&
                     target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, true, true, false, false, false))
            {
                if (!FedKatarinaV2.Program.Q.IsReady() || !FedKatarinaV2.Program.W.IsReady()) return;

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead &&
                        FedKatarinaV2.Damage.CalculateDamage(target, false, true, false, false, false) >=
                        target.Health)
                    {
                        CastW(target);
                    }
                }, FedKatarinaV2.Program.Q.CastDelay);
            }

            else if (UseQ &&
                     target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, true, false, false, true, false))
            {
                if (!FedKatarinaV2.Program.Q.IsReady())

                    CastQ(target);
                Core.DelayAction(() =>
                {
                }, FedKatarinaV2.Program.Q.CastDelay);
            }

            else if (UseQ && UseW && UseE &&
                     target.Health <=
                     FedKatarinaV2.Damage.CalculateDamage(target, true, true, true, false, false))
            {
                if (!FedKatarinaV2.Program.Q.IsReady() || !FedKatarinaV2.Program.W.IsReady() || !FedKatarinaV2.Program.E.IsReady())
                {
                    return;
                }

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead)
                    {
                        CastW(target);
                        Core.DelayAction(() =>
                        {
                            if (!target.IsDead)
                            {
                                CastE(target);
                            }
                        }, FedKatarinaV2.Program.W.CastDelay);
                    }
                }, FedKatarinaV2.Program.Q.CastDelay);
            }
        }

        private static void CastQ(AIHeroClient target)
        {
            if (!FedKatarinaV2.Program.Q.IsReady()) return;

            if (FedKatarinaV2.Program.Q.IsInRange(target))
            {
                FedKatarinaV2.Program.Q.Cast(target);
            }
        }


        private static void CastW(AIHeroClient target)
        {
            if (!FedKatarinaV2.Program.W.IsReady()) return;

            if (FedKatarinaV2.Program.W.IsInRange(target))
            {
                FedKatarinaV2.Program.W.Cast();
            }
        }

        private static void CastE(AIHeroClient target)
        {
            if (!FedKatarinaV2.Program.E.IsReady()) return;

            if (FedKatarinaV2.Program.E.IsInRange(target))
            {
                //FedKatarinaV2.Program.E.Cast(target);
                Core.DelayAction(() => FedKatarinaV2.Program.E.Cast(target), new Random().Next(FedKatarinaV2.WardJumper.WardjumpMenu["checkTimeE"].Cast<Slider>().CurrentValue));
            }
        }


        private static void CastR(AIHeroClient target)
        {
            if (!FedKatarinaV2.Program.R.IsReady()) return;

            if (FedKatarinaV2.Program.E.IsInRange(target))
            {
                FedKatarinaV2.Program.R.Cast();
            }

        }
    }
}
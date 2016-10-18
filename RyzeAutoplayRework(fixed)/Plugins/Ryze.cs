using EloBuddy;
using EloBuddy.SDK;
using Autoplay;
using EloBuddy.SDK.Enumerations;
using System.Linq;

namespace Plugins
{
    public class Ryze : Helper
    {
        private static Spell.Skillshot Q;
        private static Spell.Targeted W;
        private static Spell.Targeted E;
        private static Spell.Active R;

        public static void Init()
        {
            Chat.Print(Player.Instance.ChampionName + "Loaded");
            new RyzeMenu().Init();
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);
        }
        public static void RyzeRecall()
        {
            var stahppls = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (!stahppls.IsValidTarget(W.Range) || stahppls == null) return;

            if (W.IsReady() && W.IsInRange(stahppls))
            {
                W.Cast(stahppls);
            }
        }
        public static void Farm()
        {

            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Name.ToLower().Contains("minion") && x.IsValidTarget(Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion == null || !minion.IsValid) return;
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            if (Orbwalker.CanAutoAttack && myHero.IsInAutoAttackRange(minion) && GetClosestTurret(AARange()) == null)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
            }
            if (!Pasive)
            {
                if (Q.IsReady() && myHero.ManaPercent >= 15)
                {
                    Q.Cast(minion);
                }
                if (!Q.IsReady() && E.IsReady() && myHero.Level > 10 ? myHero.ManaPercent >= 50 : myHero.ManaPercent >= 75)
                {
                    E.Cast(minion);
                }
                if (myHero.Level > 10)
                {
                    if (!E.IsReady() && W.IsReady() && myHero.ManaPercent >= 50)
                    {
                        W.Cast(minion);
                    }
                    if (R.IsReady() && myHero.ManaPercent >= 50)
                    {
                        if (Pasive || myHero.GetBuffCount("ryzepassivestack") == 4 && !Q.IsReady() | !W.IsReady() | !E.IsReady())
                        {
                            R.Cast();
                        }
                    }
                }
            }
            if (Pasive)
            {
                if (Q.IsReady() && myHero.ManaPercent >= 15)
                {
                    Q.Cast(minion);
                }
                if (!Q.IsReady() && E.IsReady() && myHero.ManaPercent >= 40)
                {
                    E.Cast(minion);
                }
                if (!E.IsReady() && W.IsReady() && myHero.ManaPercent >= 40)
                {
                    W.Cast(minion);
                }
                if (R.IsReady() && myHero.ManaPercent >= 60)
                {
                    R.Cast();
                }
            }
        }
        public static void Combo()
        {
            //Lazy ;-;

            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            if (target != null && target.IsValidTarget(900)                             
                                              //Credits to ParaVayne (funboxxx)
                                              && !target.HasBuff("sionpassivezombie")               //sion Passive
                                              && !target.HasBuff("KarthusDeathDefiedBuff")          //karthus passive
                                              && !target.HasBuff("kogmawicathiansurprise")          //kog'maw passive
                                              && !target.HasBuff("zyrapqueenofthorns")              //zyra passive
                                              && !target.HasBuff("ChronoShift")                     //zilean R
                                              && !target.HasBuff("yorickrazombie"))                 //yorick R
            {
                var QDmg = myHero.GetSpellDamage(target, SpellSlot.Q);
                var WDmg = myHero.GetSpellDamage(target, SpellSlot.W);
                var EDmg = myHero.GetSpellDamage(target, SpellSlot.E);
                if (QDmg + WDmg + EDmg > target.Health && (target.CountEnemiesInRange(500) <= 3 || Stacks >= 3) && !target.IsDead)
                {
                    ComboPLS = true;
                }
                else { ComboPLS = false; }
            }
            else { ComboPLS = false; }

            if (target == null) return;         
            var QPred = Q.GetPrediction(target);
            bool StacksBuff = myHero.HasBuff("ryzepassivestack");
            
            if (!Q.IsReady() && !W.IsReady() && !E.IsReady())
            {
                if (Orbwalker.CanAutoAttack && myHero.IsInAutoAttackRange(target))
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                }
            }
            if (target.IsValidTarget(Q.Range))
            {
                if (Stacks <= 2 || !StacksBuff)
                {
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
                if (Stacks == 3)
                {
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
                                R.Cast();
                        }
                    }
                }
                if (Stacks == 4)
                {
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
                if (Pasive)
                {
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                            && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
            }
            else
            {
                if (W.IsReady()
                    && target.IsValidTarget(W.Range))
                    W.Cast(target);

                if (Q.IsReady()
                    && target.IsValidTarget(Q.Range))
                    Q.Cast(QPred.UnitPosition);

                if (E.IsReady()
                    && target.IsValidTarget(E.Range))
                    E.Cast(target);
            }
            if (!R.IsReady() || Stacks != 4) return;

            if (Q.IsReady() || W.IsReady() || E.IsReady()) return;

            R.Cast();
        }

    }
}

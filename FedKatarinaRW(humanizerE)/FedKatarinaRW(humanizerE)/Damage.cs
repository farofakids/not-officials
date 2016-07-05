using EloBuddy;
using EloBuddy.SDK;

namespace FedKatarinaV2
{
    internal class Damage
    {

        public static float CalculateDamage(Obj_AI_Base target, bool q, bool w, bool e, bool r, bool ignite)
        {
            var totaldamage = 0f;

            if (target == null) return totaldamage;

            if (q && Program.Q.IsReady())
            {
                totaldamage += QDamage(target);
            }

            if (w && Program.W.IsReady())
            {
                totaldamage = WDamage(target);
            }

            if (e && Program.E.IsReady())
            {
                totaldamage += EDamage(target);
            }

            if (r && Program.R.IsReady())
            {
                totaldamage += RDamage(target);
            }
            /*
            if (ignite && Program.Ignite != null && Program.Ignite.IsReady() && Program.Ignite.IsInRange(target))
            {
                totaldamage += Player.Instance.GetSummonerSpellDamage(target,
                    EloBuddy.SDK.DamageLibrary.SummonerSpells.Ignite);
            }*/

            return totaldamage;
        }


        /// <summary>
        /// Calculates the Damage done with Q
        /// </summary>
        /// <param name="target">The Target</param>
        /// <returns>Returns the Damage done with Q</returns>
        public static float QDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 60, 85, 110, 135, 160 }[Program.Q.Level] + (Player.Instance.TotalMagicalDamage * 0.4f));
        }

        /// <summary>
        /// Calculates the Damage done with W
        /// </summary>
        /// <param name="target">The Target</param>
        /// <returns>Returns the Damage done with W</returns>
        public static float WDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 40, 75, 110, 145, 180 }[Program.W.Level] + (Player.Instance.TotalMagicalDamage * 0.2f));
        }

        /// <summary>
        /// Calculates the Damage done with E
        /// </summary>
        /// <param name="target">The Target</param>
        /// <returns>Returns the Damage done with E</returns>
        public static float EDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 40, 70, 100, 130, 160 }[Program.E.Level] + (Player.Instance.TotalMagicalDamage * 0.2f));
        }

        /// <summary>
        /// Calculates the Damage done with R
        /// </summary>
        /// <returns>Returns the Damage done with R</returns>
        public static float RDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 350, 550, 750 }[Program.R.Level] + (Player.Instance.TotalMagicalDamage * 0.2f));
        }
    }
}
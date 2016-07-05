using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace Farofakids_Nautilus
{
    internal class SPELLS
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W, E;
        public static Spell.Targeted R;

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 2000, 90)
            {
                AllowedCollisionCount = 0
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 550);
            R = new Spell.Targeted(SpellSlot.R, 755);
        }

        public static float GetComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;

            if (Q.IsReady())
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.Q);

            if (W.IsReady())
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.W);

            if (E.IsReady())
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.E);

            if (R.IsReady())
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R);

            return (float)damage;
        }

    }
}

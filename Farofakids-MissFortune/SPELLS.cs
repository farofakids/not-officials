using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace Farofakids_MissFortune
{
    internal class SPELLS
    {
        public static Spell.Targeted Q;
        public static Spell.Skillshot Q1, E, R;
        public static Spell.Active W;


        public static void Initialize()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 655);
            Q1 = new Spell.Skillshot(SpellSlot.Q, 1300, SkillShotType.Linear, 250, 1500, 70);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 500, int.MaxValue, 200);
            R = new Spell.Skillshot(SpellSlot.R, 1350, SkillShotType.Circular, 250, 2000, 100);
        }

    }
}

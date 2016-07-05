using EloBuddy;
using EloBuddy.SDK;

namespace DamageIndicator
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).IsReady)
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);
            }

            // W
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).IsReady)
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.W);
            }

            // E
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).IsReady)
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.E);
            }

            // R
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady)
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.R);
            }

            return damage;
        }
    }
}
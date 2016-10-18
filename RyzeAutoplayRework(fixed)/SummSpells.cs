using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using System;

namespace Autoplay
{
    class SummSpells : Helper
    {
        public static Spell.Active Heal = null;
        public static Spell.Active Barrier = null;
        public void Init()
        {
            var heal = myHero.Spellbook.Spells.Where(x => x.Name.Contains("heal"));
            SpellDataInst Heal1 = heal.Any() ? heal.First() : null;
            if (Heal1 != null)
            {
                Heal = new Spell.Active(Heal1.Slot);
            }

            var barrier = myHero.Spellbook.Spells.Where(x => x.Name.Contains("barrier"));
            SpellDataInst Barrier1 = barrier.Any() ? barrier.First() : null;
            if (Barrier1 != null)
            {
                Barrier = new Spell.Active(Barrier1.Slot);
            }
            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (myHero.CountEnemiesInRange(900) > 0)
            {
                if (myHero.HealthPercent <= 50)
                {
                    if (Heal != null && Heal.IsReady())
                    {
                        Heal.Cast();
                    }
                }
                else if (myHero.HealthPercent <= 35)
                {
                    if (Barrier != null && Barrier.IsReady())
                    {
                        Barrier.Cast();
                    }
                }
            }
        }
    }
}
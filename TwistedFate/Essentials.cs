namespace TwistedBuddy
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Essentials
    {
        public static bool UseStunQ = false;

        public static AIHeroClient StunnedTarget { get; set; }

        public static Menu MainMenu, CardSelectorMenu, ComboMenu, LaneClearMenu, JungleClearMenu, HarassMenu, KillStealMenu, DrawingMenu, MiscMenu;
        
        public static Cards HeroCardSelection(AIHeroClient t, Menu menu)
        {
            if (t == null || menu == null) return Cards.None;

            var card = Cards.None; 
            var alliesaroundTarget = t.CountEnemiesInRange(200);
            var enemyW = menu["enemyW"].Cast<Slider>().CurrentValue;
            var manaW = menu["manaW"].Cast<Slider>().CurrentValue;

            if (Player.Instance.ManaPercent <= manaW)
            {
                card = Cards.Blue;
                return card;
            }

            if (Player.Instance.ManaPercent > manaW && alliesaroundTarget >= enemyW)
            {
                card = Cards.Red;
                return card;
            }

            if (Player.Instance.ManaPercent > manaW && alliesaroundTarget < enemyW)
            {
                card = Cards.Yellow;
                return card;
            }

            return card;
        }
    }
}

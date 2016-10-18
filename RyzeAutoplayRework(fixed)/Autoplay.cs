using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using Plugins;
using System.Linq;
using Color = System.Drawing.Color;

namespace Autoplay
{
    class Program : Helper
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }
        private static void Game_OnGameLoad(EventArgs args)
        {
            if (myHero.Team == GameObjectTeam.Order)
            {
                Spawn = new Vector3(422, 433, 183);
            }
            else
            {
                Spawn = new Vector3(14314, 14395, 172);
            }
            new PluginLoader().LoadPlugin();
            new SummSpells().Init();
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Player.OnDamage += Player_OnDamage;
            Console.WriteLine("Ryze injected");
            random = GetRandompos(-150, 150);
        }

        private static void Player_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (sender != null)
            {
                if (sender.IsEnemy)
                {
                    if (sender.Type == GameObjectType.obj_AI_Turret)
                    {
                        LeaveTowerPls = true;
                        tower = (Obj_AI_Turret)sender;
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (RecallNoob)
            {
                if (myHero.Distance(recall) > 50 + myHero.BoundingRadius)
                {
                    Drawing.DrawText(300, 235, Color.White, "Walking to recall point");
                }
                else
                {
                    Drawing.DrawText(300, 235, Color.White, "Recalling");
                } 
            }
            if (ComboPLS)
            {
                Drawing.DrawText(300, 220, Color.White, "Combo ON");
            }
            if (Waitingminions)
            {
                Drawing.DrawText(300, 205, Color.White, "Waiting minions...");
            }
            if (ChangedToAllies)
            {
                Drawing.DrawText(300, 190, Color.White, "Following allies");
            }
            if (LeaveTowerPls)
            {
                Drawing.DrawCircle(tower.Position, 1000, Color.Red);
                Drawing.DrawText(157, 800, Color.Red, "Leaving tower");
            }

            Drawing.DrawCircle(Pos + random, 50, Color.Yellow);

        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (tower != null)
            {
                if (!tower.IsDead)
                {
                    if (myHero.Distance(tower) >= 1100)
                    {
                        LeaveTowerPls = false;
                    }
                }
                else
                {
                    LeaveTowerPls = false;
                    tower = null;
                }
                if (LeaveTowerPls)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo,tower.ServerPosition.Extend(myHero.ServerPosition, 1200).To3D());
                }
            }
            
            if (myHero.IsInShopRange())
            {
                RecallNoob = false;
                
                if (myHero.HealthPercent < 80)
                {
                    WaitingHealth = true;
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                }
                else if (myHero.MaxMana > 100 && myHero.ManaPercent < 80)
                {
                    WaitingHealth = true;
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                }
                else
                {
                    WaitingHealth = false;
                    Orbwalker.DisableMovement = false;
                    Orbwalker.DisableAttacking = false;
                }
            }
            if (myHero.IsInShopRange() || myHero.IsDead)
            {
                switch (myHero.ChampionName.ToLower())
                {
                    case "ryze":
                        Core.DelayAction(() => Shop.Items.BuyRyzeItems(), 1000);
                        break;
                }
            }
            
            if (myHero.IsDead) return;

            var turret = GetClosestTurret(AARange());
            if (turret != null)
            {
                if (Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, turret);
                }
            }
            if (!LeaveTowerPls)
            {
                Orbwalker.OrbwalkTo(Pos + random);
            }
            if (!myHero.IsRecalling())
            {
                if (!RecallNoob && !LeaveTowerPls)
                {
                    Orbwalk.OrbwalkManager();
                    if (!ComboPLS) { Farm(); }
                    Combo();
                }
                RecallManager();            
            }
            if (RecallNoob)
            {
                switch(myHero.ChampionName)
                {
                    case "Ryze":
                        Ryze.RyzeRecall();
                        break;
                }
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
            }
        }
        
        
        public static void Recall()
        {
            if (myHero.IsInShopRange() || myHero.IsDead) return;
            if (!Checked)
            {
                RecallNoob = true;
                recall = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null && k.BaseSkinName.Contains("Turret")).OrderBy(k => k.Distance(myHero)).First().ServerPosition.Extend(Spawn, 300).To3D();
                Checked = true;
            }
            if (myHero.Distance(recall) > 50 + myHero.BoundingRadius)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, recall);
            }
            else
            {
                if (!myHero.IsRecalling())
                {
                    if (B.IsReady())
                    {
                        B.Cast();
                        Checked = false;
                    }
                }               
            }
        }

        private static void Farm()
        {
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Ryze.Farm();
                    break;
            }
        }

        private static void Combo()
        {
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Ryze.Combo();
                    break;
            }
        }
        private static void RecallManager()
        {
            if (RecallNoob)
            {
                Recall();
            }
            if (myHero.HealthPercent < 35)
            {
                RecallNoob = true;
            }
            else if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent <= 15)
                {
                    RecallNoob = true;
                }
            }
        }
    }
}
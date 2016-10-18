using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autoplay
{
    public class Helper
    {
        public static Spell.Active B = new Spell.Active(SpellSlot.Recall);
        public static bool Waitingminions = false;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static readonly Vector3 Toplane = new Vector3(1117, 10878, 53);
        public static Vector3 Spawn;
        public static Obj_AI_Turret tower = null;
        public static int random;
        public static int RandomCheck = 0;
        public static bool ChangedToAllies = false;
        public static bool WaitingHealth = false;
        public static int Tick = 0;
        public static bool Once = false;
        public static bool Checked = false;
        public static bool RecallNoob = false;
        public static bool CheckedH = false;
        public static bool CheckedC = false;
        public static bool LeaveTowerPls = false;
        public static bool AttackingEnemy = false;
        public static bool ComboPLS = false;
        public static int lastTurret = 0;
        public static Vector3 recall = new Vector3();
        private static string[] SmiteNames = new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" };
        public static Vector3 Pos;
        //CT-SummonerRift
        #region Turrets
        public static List<Obj_AI_Turret> GetAllyTurrets()
        {
            return (EntityManager.Turrets.Allies.ToList());
        }
        public static Obj_AI_Turret GetTopAllyTurret()
        {
            List<Vector3> aTPoints = null;

            if (ObjectManager.Player.Team == GameObjectTeam.Order)
                // Enemy == Blue
                aTPoints = new List<Vector3>
                {
                    new Vector3(973, 10446, 52),
                    new Vector3(1509, 6705, 52),
                    new Vector3(1155, 4270, 95)
                };

            else
            {
                // Enemy == Red
                aTPoints = new List<Vector3>
                {
                    new Vector3(4337, 13817, 52),
                    new Vector3(7950, 13382, 52),
                    new Vector3(10446, 13645, 95)
                };
            }
            List<Obj_AI_Turret> Turrets = GetAllyTurrets();
            List<Obj_AI_Turret> Temp = new List<Obj_AI_Turret>();

            foreach (Obj_AI_Turret t in Turrets)
            {
                foreach (Vector3 p in aTPoints)
                {
                    if (t.Distance(p) < 500)
                    {
                        Temp.Add(t);
                    }
                }
            }//foreach

            return (Temp.Count > 0 ? Temp[0] : SpawnTurret());
        }

        private static Obj_AI_Turret SpawnTurret()
        {
            Obj_AI_Turret spawn = null;
            spawn = EntityManager.Turrets.Allies.Where(x => x.Distance(Spawn) < 500).First();
            return (spawn);
        }

        public static Obj_AI_Turret GetClosestTurret(float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Enemies
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T[0] : null);
        }
        public static Obj_AI_Turret GetClosestTurret(AIHeroClient player, float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Enemies
                .Where(t => t.Distance(player.Position) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T[0] : null);
        }
        public static Obj_AI_Turret GetClosestTurret(Vector3 player, float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Enemies
                .Where(t => t.Distance(player) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T[0] : null);
        }
        public static Obj_AI_Turret ClosestAllyTurret(float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Allies
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T.Last() : null);
        }

        #endregion
        #region Minions
        public static List<Obj_AI_Minion> GetEnemyMinions()
        {
            return (EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => !t.IsDead).ToList());
        }
        public static float AARange()
        {
            return myHero.GetAutoAttackRange() - 10;
        }
        public static List<Obj_AI_Minion> GetAllyMinions()
        {
            return (EntityManager.MinionsAndMonsters.AlliedMinions.Where(t => !t.IsDead).ToList());
        }
        public static Obj_AI_Minion GetClosestMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead && t.Name.ToLower().Contains("minion")).ToList();


            if (T.Count > 0)
            {
                float Dist = Player.Instance.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Player.Instance.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        public static Obj_AI_Minion GetClosestTopMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();


            if (T.Count > 0)
            {
                float Dist = Toplane.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Toplane.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        public static Obj_AI_Minion GetClosestAllyMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.AlliedMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead && t.Name.ToLower().Contains("minion")).ToList();
            if (T.Count > 0)
            {
                float Dist = Player.Instance.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Player.Instance.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        public static Obj_AI_Minion GetFarthestAllyMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.AlliedMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead && t.Name.ToLower().Contains("minion")).ToList();
            if (T.Count > 0)
            {
                float Dist = Player.Instance.Distance(T.Last().Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Player.Instance.Distance(T.Last().Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        #endregion

        public static AIHeroClient GetNearestAlly()
        {
            //Like old times Kappa
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsRecalling() && !x.IsInShopRange() && !x.IsDead && !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner1).Name) &&
            !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner2).Name)).OrderBy(x => x.Distance(myHero));

            return (ally.Count() > 0 ? ally.First() : null);
        }
        public static AIHeroClient GetNearestAlly(float range)
        {
            //Like old times Kappa
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsRecalling() && !x.IsInShopRange() && !x.IsDead && !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner1).Name) &&
            !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner2).Name) &&
            x.Distance(myHero) < range).OrderBy(x => x.Distance(myHero));

            return (ally.Count() > 0 ? ally.First() : null);
        }
        public static int GetRandompos(int min, int max)
        {
            var erandom = new Random().Next(min, max);
            return erandom;
        }
    }
}



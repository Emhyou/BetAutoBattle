using System;
using System.Collections.Generic;
using System.Text;

namespace BetAutoBattle
{
    internal class Battle
    {
        public static List<Monster> BuildTeam(List<MonsterData> pool, int budget, int maxMembers)
        {
            List<Monster> team = new List<Monster>();
            List<string> usedNames = new List<string>();
            int usedCost = 0;
            Random localRand = new Random();
            int failCount = 0;

            while (usedCost < budget && failCount < 30 && team.Count < maxMembers)
            {
                var candidate = pool[localRand.Next(pool.Count)];

                if (usedCost + candidate.Cost > budget || usedNames.Contains(candidate.Name))
                {
                    failCount++;
                    continue;
                }

                team.Add(new Monster(candidate.Name, candidate.Hp, candidate.Attack, candidate.Speed, candidate.Cost, candidate.Hate));
                usedNames.Add(candidate.Name);
                usedCost += candidate.Cost;
                failCount = 0;
            }

            return team;
        }
        public static Monster ChooseTarget(List<Monster> enemyTeam)
        {
            Monster target = null;
            int highestHate = -1;

            foreach (var m in enemyTeam)
            {
                if (m.hp <= 0) continue; // 死んでるのは対象外

                if (m.hate > highestHate)
                {
                    highestHate = m.hate;
                    target = m;
                }
            }

            return target;
        }
        public static Monster DetermineFirst(Monster a, Monster b)
        {
            return a.speed >= b.speed ? a : b;
        }
        public static void Attack(Monster attacker, Monster target, string attackerTeam, string targetTeam)
        {
            target.hp -= attacker.attack;
            Console.WriteLine($"[{attackerTeam}] {attacker.name}が[{targetTeam}] {target.name}に{attacker.attack}のダメージ！残りHPは{target.hp}");
        }
    }
}
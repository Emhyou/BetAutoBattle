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
            int usedCost = 0;
            Random localRand = new Random();
            int failCount = 0;

            while (usedCost < budget && failCount < 10 && team.Count < maxMembers)
            {
                var candidate = pool[localRand.Next(pool.Count)];

                if (usedCost + candidate.Cost > budget)
                {
                    failCount++;
                    continue;
                }

                team.Add(new Monster(candidate.Name, candidate.Hp, candidate.Attack, candidate.Speed, candidate.Cost));
                usedCost += candidate.Cost;
                failCount = 0;
            }

            return team;
        }
        public static Monster DetermineFirst(Monster a, Monster b)
        {
            return a.speed >= b.speed ? a : b;
        }

        public static void Attack(Monster attacker, Monster target)
        {
            target.hp -= attacker.attack;
            Console.WriteLine($"{attacker.name}が{target.name}に{attacker.attack}のダメージ！残りHPは{target.hp}");
        }
    }
}
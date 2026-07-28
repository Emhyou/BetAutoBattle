using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BetAutoBattle
{
    public class MonsterData
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Speed { get; set; }
        public int Cost { get; set; }
        public int Hate { get; set; }
        public int BlockRate { get; set; }
        public int DodgeRate { get; set; }
    }

    [XmlRoot("Monsters")]
    public class MonsterList
    {
        [XmlElement("Monster")]
        public List<MonsterData> Monsters { get; set; }
    }

    internal class Monster
    {
        internal static Random rand = new Random();
        public string name { get; set; }
        private int _hp;
        public int hp
        {
            get { return _hp; }
            set { _hp = value < 0 ? 0 : value; }
        }
        public int attack { get; set; }
        public int speed { get; set; }

        public int cost { get; set; }

        public int hate { get; set; }

        public Monster(string name, int hp, int attack, int speed,int cost, int hate)
        {
            this.name = name;
            this.hp = hp;
            this.attack = attack;
            this.speed = speed;
            this.cost = cost;
            this.hate = hate;
        }
    }

    class Program
    {
        public static void Main()
        {
            string path = "Monsters.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MonsterList));
            using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                MonsterList monsterList = (MonsterList)xmlSerializer.Deserialize(stream);

                List<Monster> teamA = Battle.BuildTeam(monsterList.Monsters, 45, 3);
                List<Monster> teamB = Battle.BuildTeam(monsterList.Monsters, 60, 2);

                Console.WriteLine("チームA:");
                foreach (var m in teamA)
                {
                    Console.WriteLine($"{m.name} (Cost:{m.cost})");
                }

                Console.WriteLine("チームB:");
                foreach (var m in teamB)
                {
                    Console.WriteLine($"{m.name} (Cost:{m.cost})");
                }

                List<Monster> allMonsters = new List<Monster>();
                allMonsters.AddRange(teamA);
                allMonsters.AddRange(teamB);
                allMonsters.Sort((a, b) => b.speed.CompareTo(a.speed));

                bool teamAAlive = true;
                bool teamBAlive = true;

                while (teamAAlive && teamBAlive)
                {
                    foreach (var attacker in allMonsters)
                    {
                        if (attacker.hp <= 0) continue;

                        List<Monster> enemyTeam = teamA.Contains(attacker) ? teamB : teamA;
                        Monster target = Battle.ChooseTarget(enemyTeam);

                        if (target == null) break; // 相手全滅

                        string attackerTeamName = teamA.Contains(attacker) ? "A" : "B";
                        string targetTeamName = teamA.Contains(target) ? "A" : "B";
                        Battle.Attack(attacker, target, attackerTeamName, targetTeamName);
                    }

                    teamAAlive = teamA.Exists(m => m.hp > 0);
                    teamBAlive = teamB.Exists(m => m.hp > 0);
                }

                Console.WriteLine(teamAAlive ? "チームAの勝利！" : "チームBの勝利！");
            }
        }
    }
}
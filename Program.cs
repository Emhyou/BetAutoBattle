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

        public Monster(string name, int hp, int attack, int speed,int cost)
        {
            this.name = name;
            this.hp = hp;
            this.attack = attack;
            this.speed = speed;
            this.cost = cost;
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

                List<Monster> teamA = Battle.BuildTeam(monsterList.Monsters, 30, 4);
                List<Monster> teamB = Battle.BuildTeam(monsterList.Monsters, 40, 3);

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
            }
        }
    }
}
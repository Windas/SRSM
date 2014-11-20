using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
	public class Player
	{
		public string PlName { get; set; }
		public string PlNum { get; set; }
		public int FoulTime { get; set; }
		public int Score { get; set; }
		public int Age { get; set; }
        public string Team { get; set; }

        public Player(string name, string num, int ft, int sc, int age, string team)
        {
            PlName = name;
            PlNum = num;
            FoulTime = ft;
            Score = sc;
            Age = age;
            Team = team;
        }
        public Player() { }
	}
}

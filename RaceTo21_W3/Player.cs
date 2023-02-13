using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{
		public string name;
		public List<Card> cards = new List<Card>();
		public PlayerStatus status = PlayerStatus.active;
		public int score;
		public int overallScore;

		public Player(string n)
		{
			name = n;
        }

		/* Function: Introduce() **********
		 * Loops through number of players and introduces player by name.
		 * Called by ShowPlayers() method in CardTable object.
		 *********************************/
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}
	}
}


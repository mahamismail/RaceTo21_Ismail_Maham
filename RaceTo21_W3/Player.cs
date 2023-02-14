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

		/* Function: ClearHand() **********
         * Removes the cards from the player's hands
         ************************************/
		public void ClearHand()
		{
			cards.Clear();
		}

		/*Function: ResetRound() ***************
		* Reset the round and set all players to active.
		* 
		*/
		public void ResetRound()
		{
			ClearHand();
			status = PlayerStatus.active; // make all status active again.
		}

		public void BeginRound()
		{
			ClearHand();
			status = PlayerStatus.active; // make all status active again.
		}
	}
}


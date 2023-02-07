using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class CardTable
    {
        public CardTable()
        {
            Console.WriteLine("Setting Up Table...");
        }

        /* Shows the name of each player and introduces them by table position.
         * Is called by Game object.
         * Game object provides list of players.
         * Calls Introduce method on each player object.
         */
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i+1); // List is 0-indexed but user-friendly player positions would start with 1...
            }
        }

        /* Gets the user input for number of players.
         * Is called by Game object.
         * Returns number of players to Game object.
         */
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        /* Gets the name of a player
         * Is called by Game object
         * Game object provides player number
         * Returns name of a player to Game object
         */
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }

        public bool OfferACard(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want 3 cards? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }

        /*********************** FEATURE TO DO *******************************
        Only the winning player earns points equal to their score that round. 
            o Players who went “bust” lose points equal to their hand total minus 21. 
            o Players who “stay” earn no points for the round. 
            o Game ends when one player reaches an agreed-upon score (for example, 100 points) or after an 
            agreed-upon number of rounds. 
                • Allow players to customize this number (rounds or score, whichever you choose) at start 
                of game. 
                    • At start of game, let players choose whether game will last a specific number of 
                    rounds or until an agreed-upon score is reached.
        
        SO FAR:
        - Set a public int for overall Score in Player class called gameScore
        - Made gameScore additions and subtractions in the function showFinalScores()
        - Created enum Task ShowBigScore
        - Created function ShowOverAllScore, which when called writes to console and displays player's overall game score.
        - This function is called when Task is set to ShowBigScore
        - Set task to ShowBigScores once all players are introduced.
        - Set task to ShowBigScores once CheckForEnd is true and once someone has won or gone bust (Probably after GameOver?); then move forward.
        
        BUG: 
        - ShowOverAllScore() is not being recognized in the Game class when Task is ShowBigScore.
         */
        public void ShowOverallScore(Player player)
        {
            Console.Write(player.name + "'s Overall Score: " + player.gameScore);
        }

        public void ShowHand(Player player)
        {
            if (player.cards.Count > 0)
            {
                Console.Write(player.name + " has: ");
                foreach (Card card in player.cards)
                {
                    if (player.cards.Count == 1) // if there is only one card 
                    {
                        Console.Write(card.displayName);
                    }
                    else if (player.cards.IndexOf(card) == player.cards.Count - 1) // if the card is the latest card
                    {
                        Console.Write(card.displayName + ". ");
                    }
                    else
                    {
                        Console.Write(card.displayName + ", ");
                    }
                }
                Console.Write("=" + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }


        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                Console.WriteLine(player.name + " wins!");
            }
            else
            {
                Console.WriteLine("Everyone busted!");
            }
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
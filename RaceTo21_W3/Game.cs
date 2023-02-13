using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class Game
    {
        int numberOfPlayers; // number of players in current game
        List<Player> players = new List<Player>(); // list of objects containing player data
        CardTable cardTable; // object in charge of displaying game information
        Deck deck = new Deck(); // deck of cards
        int currentPlayer = 0; // current player on list
        public Task nextTask; // keeps track of game state
        private bool cheating = false; // lets you cheat for testing purposes if true

        public Game(CardTable c)
        {
            cardTable = c;
            deck.Shuffle();
            //deck.ShowAllCards();
            nextTask = Task.GetNumberOfPlayers;
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));

        }

        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         */
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == Task.GetNumberOfPlayers)
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = Task.GetNames;
            }
            else if (nextTask == Task.GetNames)
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                }
                nextTask = Task.IntroducePlayers;
            }
            else if (nextTask == Task.IntroducePlayers)
            {   
                cardTable.ShowPlayers(players);
                nextTask = Task.PlayerTurn;
                //nextTask = Task.ShowBigScores; // DISPLAY OVERALL SCORES
            }
            //else if (nextTask == Task.ShowBigScores)
            //{
            //    //cardTable.ShowOverallScore(); // CALL FUNCTION SHOW OVERALL SCORE ////////////////////// NOT WORKING
            //    nextTask = Task.PlayerTurn;
            // }
            else if (nextTask == Task.PlayerTurn)
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
                if (player.status == PlayerStatus.active)
                {
                    if (cardTable.HowManyCards(player) == 0) // if false make the player stay
                    {
                        player.status = PlayerStatus.stay;
                    }
                    else if (cardTable.numOfCardsPicked <= 3 && cardTable.numOfCardsPicked != 0) //if OfferACard is true call 1, 2, 3 cards then check score
                    {
                        int numOfCards = cardTable.numOfCardsPicked;

                        for (int i = 0; i < numOfCards; i++) // this loops according to the number of cards needed. If 3, deals out and adds 3 cards.
                        {
                            Card card = deck.DealTopCard();
                            player.cards.Add(card);
                        }
                        player.score = ScoreHand(player);
                        if (player.score > 21)
                        {
                            player.status = PlayerStatus.bust;
                        }
                        else if (player.score == 21)
                        {
                            player.status = PlayerStatus.win;
                        }
                    }
                }
                cardTable.ShowHand(player);
                nextTask = Task.CheckForEnd;
            }
            else if (nextTask == Task.CheckForEnd)
            {   
                if (CheckForWinAndBust() || !CheckActivePlayers())
                {
                    Player winner = DoFinalScoring();
                    cardTable.AnnounceWinner(winner);
                    nextTask = Task.GameOver;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer > players.Count - 1)
                    {
                        currentPlayer = 0; // back to the first player...
                    }
                    nextTask = Task.PlayerTurn;
                }
            }
            else // we shouldn't get here...
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = Task.GameOver;
            }
        }

        public int ScoreHand(Player player)
        {
            int score = 0;

            if (cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    string faceValue = card.id.Remove(card.id.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;

        }

        public bool CheckActivePlayers()
        {
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }
            return false; // everyone has stayed or busted, or someone won!
        }

        /*
        This bool CheckForWinAndBust() returns true if win
         or if all are bust except one player. Else returns false.
        */
        public bool CheckForWinAndBust()
        {
            int counter = 0;

            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active) // if player active
                {
                    return false; // means at least one player is still going!
                }
                else if (player.status == PlayerStatus.win) // if player wins
                {
                    return true;
                }
                else if (player.status == PlayerStatus.bust) // if player is bust
                {
                    counter++; // this number of busts so far

                    if (counter == players.Count - 1) // checking if the number of busts are one less than the number total players. Means one person is not bust.
                    {
                        return true;
                    }
                }
            }
            return false; // everyone has stayed or busted, or someone won!
        }

        public Player DoFinalScoring()
        {
            int highScore = 0;
            
            foreach (var player in players)
            {
                cardTable.ShowHand(player);
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    highScore = player.score;
                    return player;
                }
                if (player.status == PlayerStatus.stay || player.status == PlayerStatus.active) // if all bust but one, the remaining player also can win
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                // if busted.
            }
            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore);
            }
            return null; // everyone must have busted because nobody won!
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq; // currently only needed if we use alternate shuffle method

namespace RaceTo21
{
    public class Deck
    {
        List<Card> cards = new List<Card>();

        public Deck()
        {
            Console.WriteLine("*********** Building deck...");
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };
            int cardCounter = 0;

            Dictionary<string, string> imageIDs = new Dictionary<string, string>(); // add a dictionary for keeping image png Ids

            for (int cardVal = 1; cardVal <= 13; cardVal++)
            {
                foreach (string cardSuit in suits)
                {
                    string cardName;
                    string pngName;
                    string cardLongName;

                    switch (cardVal)
                    {
                        case 1:
                            cardName = "A";
                            pngName = "A";
                            cardLongName = "Ace";
                            break;
                        case 11:
                            cardName = "J";
                            pngName = "J";
                            cardLongName = "Jack";
                            break;
                        case 12:
                            cardName = "Q";
                            pngName = "Q";
                            cardLongName = "Queen";
                            break;
                        case 13:
                            cardName = "K";
                            pngName = "K";
                            cardLongName = "King";
                            break;
                        default:
                            cardName = cardVal.ToString();
                            pngName = cardVal.ToString().PadLeft(2, '0'); // to set 0 before a single digit number in png name
                            cardLongName = cardName;
                            break;
                    }
                    cards.Add(new Card(cardName + cardSuit.First<char>(), cardLongName + " of " + cardSuit)); // Adding the Card string to Card List

                    var keyofimage = cards[cardCounter].id;
                    cardCounter++;

                    imageIDs[$"{keyofimage}"] = $"card_{cardSuit.ToLower()}_{pngName}.png"; // adding field and value to Dictionary imageIDs.


                }
            }

            /* View png names
            foreach (string key in imageIDs.Keys)
            {
                Console.Write(key + ":" + imageIDs[key]); // show all png names
                Console.WriteLine();
            }
            */

        }

        public void Shuffle()
        {
            Console.WriteLine("Shuffling Cards...");

            Random rng = new Random();

            // one-line method that uses Linq:
            // cards = cards.OrderBy(a => rng.Next()).ToList();

            // multi-line method that uses Array notation on a list!
            // (this should be easier to understand)
            for (int i=0; i<cards.Count; i++)
            {
                Card tmp = cards[i];
                int swapindex = rng.Next(cards.Count);
                cards[i] = cards[swapindex];
                cards[swapindex] = tmp;

                //cards[i] = cards[swapindex]; // makes a duplicate
                //cards[swapindex] = cards[i]; //doesn't change anything
            }
        }

        /* Maybe we can make a variation on this that's more useful,
         * but at the moment it's just really to confirm that our 
         * shuffling method(s) worked! And normally we want our card 
         * table to do all of the displaying, don't we?!
         */

        public void ShowAllCards()
        {
            for (int i=0; i<cards.Count; i++)
            {
                Console.Write(i+":"+cards[i].displayName); // a list property can look like an Array!
                if (i < cards.Count -1)
                {
                    Console.Write(" ");
                } else
                {
                    Console.WriteLine("");
                }
            }
        }

        public Card DealTopCard()
        {
            Card card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            // Console.WriteLine("I'm giving you " + card);
            return card;
        }
    }
}


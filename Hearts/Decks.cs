using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    /// <summary>
    /// Some helpter methods to create decks of cards
    /// </summary>
    public static class Decks
    {
        /// <summary>
        /// Create the standard 52 card Frensh deck in an arbitrary order
        /// </summary>
        public static Card[] GetStandardDeck()
        {
            int number_of_ranks = Enum.GetNames(typeof(Rank)).Length;
            int number_of_suites = Enum.GetNames(typeof(Suite)).Length;

            int number_of_cards = number_of_suites * number_of_ranks;

            Card[] result = new Card[number_of_cards];

            int index = 0;
            foreach (Suite suite in Enum.GetValues(typeof(Suite)))
            {
                foreach (Rank rank in Enum.GetValues (typeof(Rank)))
                {
                    result[index++] = new Card { Suite = suite, Rank = rank };
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a deck for playing a game of Hearts with the given number of players.
        /// The number of cards returned is guaranteed to be divisible by the number of players.
        /// </summary>
        /// <param name="number_of_players">Number of players</param>
        /// <returns>A subset of the standard 52 cards French deck.</returns>
        /// <exception cref="ArgumentException">Invalid number of players</exception>
        public static Card[] GetStandardDeck(int number_of_players)
        {
            Card[] cards_to_remove;
            switch (number_of_players)
            {
                case 3:
                    cards_to_remove = [
                        new() { Suite = Suite.Clubs, Rank = Rank.Two },
                    ];
                    break;
                case 4:
                    cards_to_remove = [];
                    break;
                case 5:
                    cards_to_remove = [
                        new() { Suite = Suite.Clubs, Rank = Rank.Two },
                        new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                    ];
                    break;
                case 6:
                    cards_to_remove = [
                        new() { Suite = Suite.Clubs, Rank = Rank.Two },
                        new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                        new() { Suite = Suite.Spades, Rank = Rank.Two },
                        new() { Suite = Suite.Clubs, Rank = Rank.Three },
                    ];
                    break;
                default:
                    throw new ArgumentException("number of players needs to be 3 to 6", nameof(number_of_players));
            }

            return GetStandardDeck()
                .Where(c => !cards_to_remove.Contains(c))
                .ToArray();
        }
    }
}

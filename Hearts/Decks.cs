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
        public static List<Card> GetStandardDeck()
        {
            List<Card> result = Enum
               .GetValues(typeof(Suite))
               .Cast<Suite>()
               .Join(Enum.GetValues(typeof(Rank)).Cast<Rank>(),
                    suite => true,
                    rank => true,
                    (suite, rank) => new Card { Suite = suite, Rank = rank })
               .ToList();

            return result;
        }

        /// <summary>
        /// Creates a deck for playing a game of Hearts with the given number of players.
        /// The number of cards returned is guaranteed to be divisible by the number of players.
        /// </summary>
        /// <param name="number_of_players">Number of players</param>
        /// <returns>A subset of the standard 52 cards French deck.</returns>
        /// <exception cref="ArgumentException">Invalid number of players</exception>
        public static List<Card> GetStandardDeck(int number_of_players)
        {
            List<Card> deck = GetStandardDeck();
            switch (number_of_players)
            {
                case 3:
                    deck.Remove(new Card { Suite = Suite.Clubs, Rank = Rank.Two });
                    break;
                case 4:
                    break;
                case 5:
                    deck.Remove(new Card { Suite = Suite.Clubs, Rank = Rank.Two });
                    deck.Remove(new Card { Suite = Suite.Diamonds, Rank = Rank.Two });
                    break;
                case 6:
                    deck.Remove(new Card { Suite = Suite.Clubs, Rank = Rank.Two });
                    deck.Remove(new Card { Suite = Suite.Diamonds, Rank = Rank.Two }); 
                    deck.Remove(new Card { Suite = Suite.Spades, Rank = Rank.Two });
                    deck.Remove(new Card { Suite = Suite.Clubs, Rank = Rank.Three });
                    break;
                default:
                    throw new ArgumentException("number of players needs to be 3 to 6", nameof(number_of_players));
            }

            return deck;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    /// <summary>
    /// Each trick has a leading player (i.e. the player who plays the first card)
    /// and the other players play one card each. In the end, one player wins the trick.
    /// The number of cards in a trick equals the number of players in the game.
    /// I trick which is in progress (i.e. some players played cards, others are still
    /// waiting for their turn) may have fewer cards.
    /// </summary>
    public class Trick
    {
        /// <summary>
        /// Index of the player who plays the first card in the trick.
        /// </summary>
        public int LeadingPlayer { get; set; } = -1;

        /// <summary>
        /// The cards that have been played so far in the trick. The first card
        /// is the card played by the leading player.
        /// </summary>
        public Card[] Cards { get; set; } = Array.Empty<Card>();

        /// <summary>
        /// Get the card that is currently winning the trick.
        /// </summary>
        public Card WinningCard => Cards[WinningCardIndex];

        /// <summary>
        /// Get the index of the card that is currently winning the trick.
        /// </summary>
        public int WinningCardIndex => GetWinningCardIndex(Cards);

        public static int GetWinningCardIndex(Card[] cards)
        {
            if (cards.Length <= 0)
                throw new ArgumentException("Array cannot be empty", nameof(cards));

            // find the card with the highest rank among cards of the leading suite
            Card leading_card = cards[0];
            Suite leading_suite = leading_card.Suite;
            int position = 0;
            int max = (int)leading_card.Rank;
            for (int i = 0; i < cards.Length; ++i)
            {
                Card card = cards[i];
                int value = (int)card.Rank;
                if (card.Suite == leading_suite && value > max)
                {
                    position = i;
                    max = value;
                }
            }

            return position;
        }
    }
}

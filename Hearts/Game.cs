using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts
{
    /// <summary>
    /// The game state, including all played cards and the players hands
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Creates a game for the given number of players and deals cards randomly.
        /// </summary>
        /// <param name="numberOfPlayers">Total number of players</param>
        /// <param name="dealer">Index of the player who deals the cards.</param>
        public Game(int numberOfPlayers, int dealer)
        {
            NumberOfPlayers = numberOfPlayers;
            Scores = Enumerable.Range(1, NumberOfPlayers).Select(_ => 0).ToList();
            Dealer = dealer;

            // The player next to the dealer leads the first trick
            ActivePlayer = GetNextPlayer(Dealer); 
            Tricks = [new Trick { LeadingPlayer = ActivePlayer }];

            // shuffle deck
            Card[] deck = Decks.GetStandardDeck(NumberOfPlayers).ToArray();
            Random.Shuffle(deck);

            // deal the cards
            Hands = deck
               .Select((card, index) => new { card, index })
               .GroupBy(grp => grp.index % 4)
               .Select(grouping => new Hand { Cards = grouping.Select(arg => arg.card).ToList() })
               .ToList();
        }

        /// <summary>
        /// Create game with predetermined hands
        /// </summary>
        /// <param name="hands">The hands of all the players.</param>
        /// <param name="dealer">Index of the player who deals the cards.</param>
        public Game(IEnumerable<Hand> hands, int dealer)
        {
            Hands = hands.ToList();
            
            NumberOfPlayers = Hands.Count;
            Scores = Enumerable.Range(1, Hands.Count).Select(_ => 0).ToList();
            Dealer = dealer;

            // The player next to the dealer leads the first trick
            ActivePlayer = GetNextPlayer(dealer);
            Tricks = [new Trick { LeadingPlayer = ActivePlayer }];
        }

        /// <summary>
        /// Returns the player which will have their turn after the given player.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        /// <returns>The player after the current player.</returns>
        private int GetNextPlayer(int current_player)
        {
            return current_player < 0 ? current_player : (current_player + 1) % NumberOfPlayers;
        }

        /// <summary>
        /// Number of players who are playing the game
        /// </summary>
        public int NumberOfPlayers { get; private set; }

        /// <summary>
        /// The index of the player who deals the cards
        /// </summary>
        public int Dealer { get; private set; }

        /// <summary>
        /// The player who is currently deciding their turn
        /// </summary>
        public int ActivePlayer { get; private set; }

        /// <summary>
        /// True iff the game has ended
        /// </summary>
        public bool IsFinished { get; private set; } = false;

        /// <summary>
        /// Hands of all the players
        /// </summary>
        public List<Hand> Hands { get; private set; }

        /// <summary>
        /// Hand of the active player.
        /// </summary>
        public Hand ActivePlayerHand => Hands[ActivePlayer];

        /// <summary>
        /// Current number of penalty points for each player.
        /// The numbers are positive, but the goal is to have the fewest points.
        /// </summary>
        public List<int> Scores { get; private set; }

        /// <summary>
        /// All the tricks that have been already played.
        /// </summary>
        public List<Trick> Tricks { get; private set; }

        /// <summary>
        /// The trick that is currently being played.
        /// </summary>
        public Trick CurrentTrick => Tricks.Last();

        private Random Random { get; } = new();

        public void PlayCard(int player, Card card)
        {
            if (player != ActivePlayer)
                throw new ArgumentException("Not this players turn", nameof(player));
            if (!Hands[player].Cards.Contains(card))
                throw new ArgumentException("Player doesn't have this card", nameof(card));
            if (IsFinished)
                throw new InvalidOperationException("Game already finished");

            // throw if suite is not followed
            if (CurrentTrick.Cards.Count > 0)
            {
                Suite leading_suite = CurrentTrick.Cards[0].Suite;
                if (card.Suite != leading_suite && Hands[player].Cards.Any(c => c.Suite == leading_suite))
                    throw new InvalidOperationException("Must follow leading suite");
            }



            // remove card from player hand
            ActivePlayerHand.Cards = ActivePlayerHand.Cards.Where(c => c != card).ToList();
            // append card to trick
            CurrentTrick.Cards = CurrentTrick.Cards.Append(card).ToList();
            // next players turn
            ActivePlayer = GetNextPlayer(ActivePlayer);

            // check for new trick
            if (CurrentTrick.Cards.Count == NumberOfPlayers)
            {
                // who won the trick
                int winner = (CurrentTrick.WinningCardIndex + ActivePlayer) % NumberOfPlayers;
                // whoever gets the trick, gets all the points
                Scores[winner] = CurrentTrick.Cards.Select(ScoreCard).Sum();
                // winner of trick goes next
                ActivePlayer = winner;
                // new trick
                Tricks.Add(new Trick());
                CurrentTrick.LeadingPlayer = ActivePlayer;
            }

            // check game end
            if (ActivePlayerHand.Cards.Count == 0)
            {
                IsFinished = true;
            }
        }

        /// <summary>
        /// Returns all the cards that can be legally played by the active player.
        /// </summary>
        public IEnumerable<Card> GetPlayableCards()
        {
            // When the game is finished, no legal actions exist.
            if (IsFinished)
                return [];

            // If this is the first card in the trick, all cards are legal.
            if (CurrentTrick.Cards.Count == 0)
                return ActivePlayerHand.Cards;

            // Must follow suite if possible
            Suite leading_suite = CurrentTrick.Cards[0].Suite;
            var cards = ActivePlayerHand.Cards.Where(c => c.Suite == leading_suite).ToList();
            return cards.Any() ? cards : ActivePlayerHand.Cards;
        }

        /// <summary>
        /// Returns the number of penalty points given to the player who wins the given card.
        /// </summary>
        public static int ScoreCard(Card card)
        {
            return card.Suite == Suite.Hearts ? 1 : 0;
        }
    }
}

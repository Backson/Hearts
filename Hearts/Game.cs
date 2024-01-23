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
            Hands = new Hand[numberOfPlayers];
            Scores = new int[numberOfPlayers];
            Dealer = dealer;

            // The player next to the dealer leads the first trick
            int leading_player = GetNextPlayer(dealer);
            ActivePlayer = leading_player;
            Tricks = [new Trick() { LeadingPlayer = leading_player }];

            // shuffle deck
            Card[] deck = Decks.GetStandardDeck(numberOfPlayers);
            Random.Shuffle(deck);

            // deal the cards
            List<Card>[] dealt_cards = new List<Card>[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                dealt_cards[i] = new List<Card>();
            }
            int counter = 0;
            foreach (Card card in deck)
            {
                int which_player = counter++ % numberOfPlayers;
                dealt_cards[which_player].Add(card);
            }
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Hands[i] = new Hand() { Cards = dealt_cards[i].ToArray() };
            }
        }

        /// <summary>
        /// Create game with predetermined hands
        /// </summary>
        /// <param name="hands">The hands of all the players.</param>
        /// <param name="dealer">Index of the player who deals the cards.</param>
        public Game(IEnumerable<Hand> hands, int dealer)
        {
            int numberOfPlayers = hands.Count();
            NumberOfPlayers = numberOfPlayers;
            Hands = new Hand[numberOfPlayers];
            Scores = new int[numberOfPlayers];
            Dealer = dealer;

            // The player next to the dealer leads the first trick
            int leading_player = GetNextPlayer(dealer);
            ActivePlayer = leading_player;
            Tricks = [new Trick() { LeadingPlayer = leading_player }];

            Hands = hands.ToArray();
        }

        /// <summary>
        /// Returns the player which will have their turn after the given player.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        /// <returns>The player after the current player.</returns>
        private int GetNextPlayer(int current_player)
        {
            if (current_player < 0)
                return current_player;
            else
                return (current_player + 1) % NumberOfPlayers;
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
        public Hand[] Hands { get; private set; }

        /// <summary>
        /// Hand of the active player.
        /// </summary>
        public Hand ActivePlayerHand { get => Hands[ActivePlayer]; }

        /// <summary>
        /// Current number of penalty points for each player.
        /// The numbers are positive, but the goal is to have the fewest points.
        /// </summary>
        public int[] Scores { get; private set; }

        /// <summary>
        /// All the tricks that have been already played.
        /// </summary>
        public Trick[] Tricks { get; private set; } = [];

        /// <summary>
        /// The trick that is currently being played.
        /// </summary>
        public Trick CurrentTrick { get => Tricks[CurrentTrickIndex]; }

        /// <summary>
        /// Index of the trick that is currently being played.
        /// </summary>
        public int CurrentTrickIndex { get; private set; }

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
            if (CurrentTrick.Cards.Length > 0)
            {
                Suite leading_suite = CurrentTrick.Cards[0].Suite;
                if (card.Suite != leading_suite && Hands[player].Cards.Where(c => c.Suite == leading_suite).Count() > 0)
                    throw new InvalidOperationException("Must follow leading suite");
            }



            // remove card from player hand
            ActivePlayerHand.Cards = ActivePlayerHand.Cards.Where(c => c != card).ToArray();
            // append card to trick
            CurrentTrick.Cards = CurrentTrick.Cards.Append(card).ToArray();
            // next players turn
            ActivePlayer = GetNextPlayer(ActivePlayer);

            // check for new trick
            if (CurrentTrick.Cards.Length == NumberOfPlayers)
            {
                // who won the trick
                int winner = (CurrentTrick.WinningCardIndex + ActivePlayer) % NumberOfPlayers;
                // whoever gets the trick, gets all the points
                foreach (Card c in CurrentTrick.Cards)
                {
                    Scores[winner] += ScoreCard(c);
                }
                // winner of trick goes next
                ActivePlayer = winner;
                // new trick
                CurrentTrickIndex++;
                while (CurrentTrickIndex >= Tricks.Length)
                    Tricks = Tricks.Append(new()).ToArray();
                CurrentTrick.LeadingPlayer = ActivePlayer;
            }

            // check game end
            if (ActivePlayerHand.Cards.Length == 0)
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
            if (CurrentTrick.Cards.Length == 0)
                return ActivePlayerHand.Cards;

            // Must follow suite if possible
            Suite leading_suite = CurrentTrick.Cards[0].Suite;
            var cards = ActivePlayerHand.Cards.Where(c => c.Suite == leading_suite);
            if (cards.Any())
                return cards;
            else
                return ActivePlayerHand.Cards;
        }

        /// <summary>
        /// Returns the number of penalty points given to the player who wins the given card.
        /// </summary>
        public static int ScoreCard(Card card)
        {
            if (card.Suite == Suite.Hearts)
                return 1;
            else
                return 0;
        }
    }
}

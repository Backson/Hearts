using Hearts;
using System.Diagnostics;

namespace HeartsTests
{
    [TestClass]
    public class TrickTests
    {
        [TestMethod]
        public void TestSingleCardWins()
        {
            Card[] cards = [new() { Suite = Suite.Spades, Rank = Rank.Ace }];
            Trick trick = new() { Cards = cards };

            Assert.AreEqual(0, trick.WinningCardIndex);
            Assert.AreEqual(cards[0], trick.WinningCard);
        }

        [TestMethod]
        public void TestNoFollowLoses()
        {
            Card[] cards = [
                new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                new() { Suite = Suite.Hearts, Rank = Rank.Two },
                new() { Suite = Suite.Spades, Rank = Rank.Two },
                new() { Suite = Suite.Clubs, Rank = Rank.Two },
            ];
            Trick trick = new() { Cards = cards };

            Assert.AreEqual(0, trick.WinningCardIndex);
            Assert.AreEqual(cards[0], trick.WinningCard);
        }

        [TestMethod]
        public void TestNoHigherLoses()
        {
            Card[] cards = [
                new() { Suite = Suite.Diamonds, Rank = Rank.Ace },
                new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                new() { Suite = Suite.Diamonds, Rank = Rank.Three },
                new() { Suite = Suite.Diamonds, Rank = Rank.Four },
            ];
            Trick trick = new() { Cards = cards };

            Assert.AreEqual(0, trick.WinningCardIndex);
            Assert.AreEqual(cards[0], trick.WinningCard);
        }

        [TestMethod]
        public void TestHigherWins()
        {
            Card[] cards = [
                new() { Suite = Suite.Diamonds, Rank = Rank.Ten },
                new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                new() { Suite = Suite.Diamonds, Rank = Rank.Jack },
                new() { Suite = Suite.Diamonds, Rank = Rank.Three },
            ];
            Trick trick = new() { Cards = cards };

            Assert.AreEqual(2, trick.WinningCardIndex);
            Assert.AreEqual(cards[2], trick.WinningCard);
        }

        // The algorithm shouldn't care about the start player number
        [TestMethod]
        public void TestActivePlayer()
        {
            Card[] cards = [
                new() { Suite = Suite.Diamonds, Rank = Rank.Ten },
                new() { Suite = Suite.Diamonds, Rank = Rank.Two },
                new() { Suite = Suite.Diamonds, Rank = Rank.Jack },
                new() { Suite = Suite.Diamonds, Rank = Rank.Three },
            ];
            Trick trick = new() { LeadingPlayer = 2, Cards = cards };

            Assert.AreEqual(2, trick.WinningCardIndex);
            Assert.AreEqual(cards[2], trick.WinningCard);
        }
    }
}
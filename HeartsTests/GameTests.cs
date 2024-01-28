using Hearts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void TestPlayerAfterDealerStarts0()
        {
            Game game = new(4, 0);
            Assert.AreEqual(1, game.ActivePlayer);
        }

        [TestMethod]
        public void TestPlayerAfterDealerStarts1()
        {
            Game game = new(4, 1);
            Assert.AreEqual(2, game.ActivePlayer);
        }

        [TestMethod]
        public void TestPlayerAfterDealerStarts2()
        {
            Game game = new(4, 2);
            Assert.AreEqual(3, game.ActivePlayer);
        }

        [TestMethod]
        public void TestPlayerAfterDealerStarts3()
        {
            Game game = new(4, 3);
            Assert.AreEqual(0, game.ActivePlayer);
        }

        [TestMethod]
        public void TestFourPlayerInitialHandSize()
        {
            Game game = new(4, 0);

            Assert.AreEqual(13, game.Hands[0].Cards.Count);
            Assert.AreEqual(13, game.Hands[1].Cards.Count);
            Assert.AreEqual(13, game.Hands[2].Cards.Count);
            Assert.AreEqual(13, game.Hands[3].Cards.Count);
        }

        [TestMethod]
        public void TestPlay()
        {
            Hand[] hands = [
                new() { Cards = [ new Card(Suite.Hearts, Rank.Four), new Card(Suite.Spades, Rank.Ace)] },
                new() { Cards = [ new Card(Suite.Diamonds, Rank.Two), new Card(Suite.Spades, Rank.Two)] },
                new() { Cards = [ new Card(Suite.Hearts, Rank.Ace), new Card(Suite.Spades, Rank.Three)] },
                new() { Cards = [ new Card(Suite.Hearts, Rank.King), new Card(Suite.Hearts, Rank.Four)] },
            ];
            Game game = new(hands, 3);

            Assert.AreEqual(0, game.ActivePlayer);

            game.PlayCard(0, new Card(Suite.Hearts, Rank.Four));
            game.PlayCard(1, new Card(Suite.Diamonds, Rank.Two));
            game.PlayCard(2, new Card(Suite.Hearts, Rank.Ace));
            game.PlayCard(3, new Card(Suite.Hearts, Rank.King));

            game.PlayCard(2, new Card(Suite.Spades, Rank.Three));
            game.PlayCard(3, new Card(Suite.Hearts, Rank.Four));
            game.PlayCard(0, new Card(Suite.Spades, Rank.Ace));
            game.PlayCard(1, new Card(Suite.Spades, Rank.Two));

            Assert.AreEqual(true, game.IsFinished);
            Assert.AreEqual(new Card(Suite.Hearts, Rank.Ace), game.Tricks[0].WinningCard);
            Assert.AreEqual(new Card(Suite.Spades, Rank.Ace), game.Tricks[1].WinningCard);
            Assert.AreEqual(1, game.Scores[0]);
            Assert.AreEqual(0, game.Scores[1]);
            Assert.AreEqual(3, game.Scores[2]);
            Assert.AreEqual(0, game.Scores[3]);
        }


    }
}

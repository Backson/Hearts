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

            Assert.AreEqual(13, game.Hands[0].Cards.Length);
            Assert.AreEqual(13, game.Hands[1].Cards.Length);
            Assert.AreEqual(13, game.Hands[2].Cards.Length);
            Assert.AreEqual(13, game.Hands[3].Cards.Length);
        }

        [TestMethod]
        public void TestPlay()
        {
            Hand[] hands = [
                new() { Cards = [ Card.Make(Suite.Hearts, Rank.Four), Card.Make(Suite.Spades, Rank.Ace)] },
                new() { Cards = [ Card.Make(Suite.Diamonds, Rank.Two), Card.Make(Suite.Spades, Rank.Two)] },
                new() { Cards = [ Card.Make(Suite.Hearts, Rank.Ace), Card.Make(Suite.Spades, Rank.Three)] },
                new() { Cards = [ Card.Make(Suite.Hearts, Rank.King), Card.Make(Suite.Hearts, Rank.Four)] },
            ];
            Game game = new(hands, 3);

            Assert.AreEqual(0, game.ActivePlayer);

            game.PlayCard(0, Card.Make(Suite.Hearts, Rank.Four));
            game.PlayCard(1, Card.Make(Suite.Diamonds, Rank.Two));
            game.PlayCard(2, Card.Make(Suite.Hearts, Rank.Ace));
            game.PlayCard(3, Card.Make(Suite.Hearts, Rank.King));

            game.PlayCard(2, Card.Make(Suite.Spades, Rank.Three));
            game.PlayCard(3, Card.Make(Suite.Hearts, Rank.Four));
            game.PlayCard(0, Card.Make(Suite.Spades, Rank.Ace));
            game.PlayCard(1, Card.Make(Suite.Spades, Rank.Two));

            Assert.AreEqual(true, game.IsFinished);
            Assert.AreEqual(Card.Make(Suite.Hearts, Rank.Ace), game.Tricks[0].WinningCard);
            Assert.AreEqual(Card.Make(Suite.Spades, Rank.Ace), game.Tricks[1].WinningCard);
            Assert.AreEqual(1, game.Scores[0]);
            Assert.AreEqual(0, game.Scores[1]);
            Assert.AreEqual(3, game.Scores[2]);
            Assert.AreEqual(0, game.Scores[3]);
        }


    }
}

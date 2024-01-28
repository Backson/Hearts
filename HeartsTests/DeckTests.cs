using Hearts;

namespace HeartsTests
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void TestDeckHasCorrectSize()
        {
            List<Card> deck = Decks.GetStandardDeck(3);
            Assert.AreEqual(52 - 52 % 3, deck.Count);
        }

        [TestMethod]
        public void TestFourPlayersDeckHasCorrectSize()
        {
            List<Card> deck = Decks.GetStandardDeck(4);
            Assert.AreEqual(52 - 52 % 4, deck.Count);
        }

        [TestMethod]
        public void TestFivePlayersDeckHasCorrectSize()
        {
            List<Card> deck = Decks.GetStandardDeck(5);
            Assert.AreEqual(52 - 52 % 5, deck.Count);
        }

        [TestMethod]
        public void TestSixPlayersDeckHasCorrectSize()
        {
            List<Card> deck = Decks.GetStandardDeck(6);
            Assert.AreEqual(52 - 52 % 6, deck.Count);
        }
    }
}

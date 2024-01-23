using Hearts;

namespace HeartsTests
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void TestDeckHasCorrectSize()
        {
            Card[] deck = Decks.GetStandardDeck(3);
            Assert.AreEqual(52 - 52 % 3, deck.Length);
        }

        [TestMethod]
        public void TestFourPlayersDeckHasCorrectSize()
        {
            Card[] deck = Decks.GetStandardDeck(4);
            Assert.AreEqual(52 - 52 % 4, deck.Length);
        }

        [TestMethod]
        public void TestFivePlayersDeckHasCorrectSize()
        {
            Card[] deck = Decks.GetStandardDeck(5);
            Assert.AreEqual(52 - 52 % 5, deck.Length);
        }

        [TestMethod]
        public void TestSixPlayersDeckHasCorrectSize()
        {
            Card[] deck = Decks.GetStandardDeck(6);
            Assert.AreEqual(52 - 52 % 6, deck.Length);
        }
    }
}

using Hearts;

Random random = new Random();
int number_of_players = 4;
int dealer_index = 0;
int player_index = random.Next(number_of_players);

Game game = new Game(number_of_players, dealer_index);

while (!game.IsFinished)
{
    if (game.CurrentTrick.Cards.Length == 0)
    {
        Console.WriteLine($"Trick {game.CurrentTrickIndex + 1}:");
    }

    int active_player = game.ActivePlayer;
    if (active_player == player_index)
    {
        Console.WriteLine("  Choose a card to play from your hand:");
        Card[] cards_in_hand = game.Hands[player_index].Cards;
        cards_in_hand = cards_in_hand.OrderBy(card => card.Suite).ThenBy(card => card.Rank).ToArray();
        int index = 0;
        foreach (Card card in cards_in_hand)
        {
            Console.WriteLine($"    ({(index++) + 1}) {card}");
        }

        GET_CHOICE:

        string? input = Console.ReadLine();
        if (input == null || !int.TryParse(input, out int choice) || choice <= 0 || choice > cards_in_hand.Length)
        {
            Console.WriteLine($"  Invalid input, try again...");
            goto GET_CHOICE;
        }

        Card played_card = cards_in_hand[choice - 1];

        if (!game.GetPlayableCards().Contains(played_card))
        {
            Console.WriteLine($"  Must follow suite, try again...");
            goto GET_CHOICE;
        }

        game.PlayCard(player_index, played_card);
        Console.WriteLine($"  You played the {played_card}!");
    }
    else
    {
        var playable_cards = game.GetPlayableCards().ToArray();
        random.Shuffle(playable_cards);
        Card played_card = playable_cards[0];
        game.PlayCard(active_player, played_card);
        Console.WriteLine($"  Player {active_player + 1} played the {played_card}!");
    }
}

Console.WriteLine($"Game over! Here are the scores:");

for (int i = 0; i < number_of_players; ++i)
{
    if (i == player_index)
        Console.WriteLine($"  Player {i + 1} (You): {game.Scores[i]}");
    else
        Console.WriteLine($"  Player {i + 1}: {game.Scores[i]}");
}

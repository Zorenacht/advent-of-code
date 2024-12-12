namespace AoC._2023;

public sealed class Day07 : Day
{
    [Puzzle(answer: 250232501)]
    public long Part1()
    {
        var parsed = Input
            .Select(x => (x.Split(" ")[0], int.Parse(x.Split(" ")[1])))
            .ToArray();
        var ordered = parsed.OrderBy(x => Value(x.Item1, withJoker: false));
        return ordered
            .Select((val, index) => val.Item2 * (index + 1))
            .Sum();
    }

    [Puzzle(answer: 249138943)]
    public long Part2()
    {
        var parsed = Input
            .Select(x => (x.Split(" ")[0], int.Parse(x.Split(" ")[1])))
            .ToArray();
        var ordered = parsed.OrderBy(x => Value(x.Item1, withJoker: true));
        return ordered
            .Select((val, index) => val.Item2 * (index + 1))
            .Sum();
    }

    private string Cards = "23456789TJQKA";
    private string CardsWithJoker = "J23456789TQKA";

    public int Value(string hand, bool withJoker)
    {
        if (hand.Length != 5) throw new NotSupportedException();

        //count amount of each card
        var cards = withJoker ? CardsWithJoker : Cards;
        var counter = new int[cards.Length];
        foreach (char ch in hand) counter[cards.IndexOf(ch)]++;

        //set value based on pair priority
        int max, secondMax;
        if (withJoker)
        {
            var ordered = counter[1..].OrderByDescending(x => x).ToArray();
            (max, secondMax) = (ordered[0] + counter[0], ordered[1]);
        }
        else
        {
            var ordered = counter.OrderByDescending(x => x).ToArray();
            (max, secondMax) = (ordered[0], ordered[1]);
        }

        int pairStrength = (max, secondMax) switch
        {
            (5, _) => 6,
            (4, _) => 5,
            (3, 2) => 4,
            (3, _) => 3,
            (2, 2) => 2,
            (2, _) => 1,
            _ => 0
        };
        int maxShift = 25;
        int value = pairStrength << maxShift;

        //set value based on card values
        for (int i = 0; i < hand.Length; i++)
        {
            value += cards.IndexOf(hand[i]) << (20 - (i * 4));
        }
        return value;
    }
}
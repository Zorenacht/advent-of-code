namespace AoC_2023;

public sealed class Day07 : Day
{
    //not 249902176
    //not 248569925
    [Puzzle(answer: 250232501)]
    public long Part1()
    {
        var set = new HashSet<string>();
        var dict = new Dictionary<string, string>();
        var parsed = Input.Select(x =>
        {
            return (x.Split(" ")[0], int.Parse(x.Split(" ")[1]));
        }).ToList();
        var sum = 0;
        var values = parsed.Select(x => Value(x.Item1)).ToArray();
        parsed = parsed.OrderBy(x => Value(x.Item1)).ToList();
        for (int i = 0; i < parsed.Count(); i++)
        {
            sum += (i + 1) * parsed[i].Item2;
        }
        return sum;
    }

    public int Value(string hand)
    {
        var counter = new int[Cards.Length];
        foreach (char ch in hand)
        {
            counter[Cards.IndexOf(ch)]++;
        }
        int value = 0;
        int maxShift = 30;
        if (counter.Any(x => x == 5)) value += 1 << (maxShift);
        else if (counter.Any(x => x == 4)) value += 1 << (maxShift - 1);
        else if (counter.Any(x => x == 3) && counter.Any(x => x == 2)) value += 1 << (maxShift - 2);
        else if (counter.Any(x => x == 3)) value += 1 << (maxShift - 3);
        else if (counter.Count(x => x == 2) == 2) value += 1 << (maxShift - 4);
        else if (counter.Any(x => x == 2)) value += 1 << (maxShift - 5);

        for (int i = 0; i < hand.Length; i++)
        {
            value += Cards.IndexOf(hand[i]) << (20 - (i * 4));
        }
        return value;
    }

    public int Comparer(string x, string y)
    {
        return 0;
    }

    private string Cards = "23456789TJQKA";

    private string CardsJ = "J23456789TQKA";


    public long Value2(string hand)
    {
        var counter = new int[CardsJ.Length];
        foreach (char ch in hand)
        {
            counter[CardsJ.IndexOf(ch)]++;
        }
        int value = 0;
        int maxShift = 30;
        int jokerIndex = CardsJ.IndexOf('J');
        int jokers = counter[jokerIndex];
        var woJokers = counter.ToList();
        woJokers.RemoveAt(jokerIndex);
        woJokers = woJokers.ToList();
        if (woJokers.Any(x => x == 5 - jokers)) value += 1 << (maxShift);
        else if (woJokers.Any(x => x == 4 - jokers)) value += 1 << (maxShift - 1);
        else if (woJokers.Any(x => x == 3 - jokers) && woJokers.Any(x => x == 2) ||
            woJokers.Any(x => x == 3) && woJokers.Any(x => x == 2 - jokers)) value += 1 << (maxShift - 2);
        else if (woJokers.Any(x => x == 3 - jokers)) value += 1 << (maxShift - 3);
        else if (jokers == 0 || jokers == 1 || jokers == 2)
        {
            var id = woJokers.IndexOf(2);
            if (woJokers.IndexOf(2) != -1 && woJokers.Where((x, i) => i != id && x + jokers == 2).Count() >= 1)
                value += 1 << (maxShift - 4);
            else if (woJokers.Any(x => x == 2 - jokers)) value += 1 << (maxShift - 5);
        }

        for (int i = 0; i < hand.Length; i++)
        {
            value += CardsJ.IndexOf(hand[i]) << (20 - (i * 4));
        }
        return value;
    }

    [Puzzle(answer: null)]
    public long Part2()
    {
        var set = new HashSet<string>();
        var dict = new Dictionary<string, string>();
        var parsed = Input.Select(x =>
        {
            return (x.Split(" ")[0], int.Parse(x.Split(" ")[1]));
        }).ToList();
        var sum = 0;
        var values = parsed.Select(x => Value2(x.Item1)).ToArray();
        parsed = parsed.OrderBy(x => Value2(x.Item1)).ToList();
        for (int i = 0; i < parsed.Count(); i++)
        {
            sum += (i + 1) * parsed[i].Item2;
        }
        return sum;
    }

    [Test]
    public void HandValue()
    {
        var noPair = "23456";
        var onePair = "23466";
        var onePairJoker = "2375J";
        var twoPair = "22455";
        var twoPairJoker = "2275J";
        var triple = "22251";
        var tripleJoker1 = "22J51";
        var tripleJoker2 = "2JJ51";
        var fullHouse = "22233";

        var four = "JJ992";

        /* Value2("JJTTA").Should().BeGreaterThan(Value2("Q6TTA"));
         Value2("J3TTA").Should().BeGreaterThan(Value2("Q6TTA"));
         Value2("7ATTA").Should().BeGreaterThan(Value2("Q6TTA"));
 */
        Value2(four).Should().BeGreaterThan(Value2(fullHouse));
    }
}
using FluentAssertions;
using Tools;

namespace AoC_2023;

public sealed class Day04 : Day
{
    [Puzzle(answer: 20667)]
    public int Part1()
    {
        var result = 0;
        foreach(var line in Input)
        {
            var split = line.Split(':');
            var start = split[0].Split(new string[] { " ", "  " },StringSplitOptions.RemoveEmptyEntries);
            var card = split[1].Split(" | ")[0];
            var hand = split[1].Split(" | ")[1];
            var no = start[1];
            var cards = card.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var hands = hand.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int count = 0;
            count = hands.Count(x => cards.Contains(x));

            result += (int)Math.Pow(2, count - 1);
        }
        return result;
    }

    public string DigitsOrDot = "0123456789.";
    public string Digits = "0123456789";

    [Puzzle(answer: 5833065)]
    public int Part2()
    {
        var handMatches = new int[Input.Length];
        for(int i=0; i<Input.Length;  i++)
        {
            var line = Input[i];
            var split = line.Split(':');
            var start = split[0].Split(new string[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
            var card = split[1].Split(" | ")[0];
            var hand = split[1].Split(" | ")[1];
            var no = start[1];
            var cards = card.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var hands = hand.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int count = 0;
            count = hands.Count(x => cards.Contains(x));
            handMatches[i] = count;
        }

        int[] amount = Enumerable.Repeat(1, Input.Length).ToArray();
        for (int i = 0; i < Input.Length; i++)
        {
            for (int j = i+1; j <= i + handMatches[i]; j++)
            {
                amount[j] += amount[i];
            }
        }
        return amount.Sum() ;
    }
}
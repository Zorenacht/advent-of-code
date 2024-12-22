using FluentAssertions;
using System;
using Tools;

namespace AoC._2024;

public sealed class Day22 : Day
{
    [Puzzle(answer: 14273043166)]
    public long Part1()
    {
        long result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            long current = line.Ints().First();
            for (int i = 0; i < 2000; ++i)
            {
                (current, _) = SecretNumber(current, 0, [], []);
            }
            result += current;
        }
        return result;
    }

    [Puzzle(answer: 1667)]
    public long Part2()
    {
        var lines = Input;
        var cache = new int[0b11111_11111_11111_11111];
        foreach (var line in lines)
        {
            long current = line.Ints().First();
            int bits = 0;

            bool[] added = new bool[0b11111_11111_11111_11111];
            for (int i = 0; i < 2000; ++i)
            {
                (current, bits) = SecretNumber(current, bits, added, cache);
            }
        }
        return cache.Max(x => x);
    }

    public (long, int) SecretNumber(long current,
        int bits,
        bool[] added,
        int[] bananas)
    {
        var lastDigit = current % 10;
        current = ((current << 06) ^ current) % 16777216;
        current = ((current >> 05) ^ current) % 16777216;
        current = ((current << 11) ^ current) % 16777216;

        // The difference is between -9 and 9, we add 9 to get a positive number between 0 and 18 which fits inside 5 bits
        int diff = (int)((current % 10) - lastDigit + 9);
        bits = ((bits << 5) + diff) & 0b11111_11111_11111_11111;

        if ((bits & 0b11111_00000_00000_00000) > 0 && !added[bits])
        {
            bananas[bits] += (int)(current % 10);
            added[bits] = true;
        };
        return (current, bits);
    }

};
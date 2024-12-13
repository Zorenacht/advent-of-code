using FluentAssertions;
using System.Text.RegularExpressions;

namespace AoC_2024;

public sealed class Day13 : Day
{
    [Puzzle(answer: 35255)]
    public long Part1() => MinCost(extraCost: 0);
    
    [Puzzle(answer: 87582154060429)]
    public long Part2() => MinCost(extraCost: 10000000000000);
    
    private long MinCost(long extraCost)
    {
        long result = 0;
        var groups = Input.GroupBy(string.Empty);
        foreach (var group in groups)
        {
            var buttonRegex = new Regex(@"Button .: X\+(\d+), Y\+(\d+)");
            var prizeRegex = new Regex(@"Prize: X=(\d+), Y=(\d+)");
            var aMatches = buttonRegex.Match(group[0]);
            var bMatches = buttonRegex.Match(group[1]);
            var prizeMatches = prizeRegex.Match(group[2]);
            int[] buttonA = [int.Parse(aMatches.Groups[1].Value), int.Parse(aMatches.Groups[2].Value)];
            int[] buttonB = [int.Parse(bMatches.Groups[1].Value), int.Parse(bMatches.Groups[2].Value)];
            long[] prize = [extraCost + int.Parse(prizeMatches.Groups[1].Value), extraCost + int.Parse(prizeMatches.Groups[2].Value)];
            
            //                          [ buttonA[0] buttonB[0] ] [ aPresses ] = [ prize[0] ]
            // Solve the linear system  [ buttonA[1] buttonB[1] ] [ bPresses ] = [ prize[1] ]
            // by inverting the matrix and checking afterward if the solution consists of integers
            var matrix = new int[][]
            {
                [buttonA[0], buttonB[0]],
                [buttonA[1], buttonB[1]]
            };
            var inverse = new int[][]
            {
                [+matrix[1][1], -matrix[0][1]],
                [-matrix[1][0], +matrix[0][0]]
            };
            var inverseFactor = matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            var aPresses = prize[0] * inverse[0][0] + prize[1] * inverse[0][1];
            var bPresses = prize[0] * inverse[1][0] + prize[1] * inverse[1][1];
            
            if (inverseFactor == 0) throw new NotSupportedException("No unique solution if not invertible.");
            if (aPresses % inverseFactor != 0 || bPresses % inverseFactor != 0) continue;
            aPresses /= inverseFactor;
            bPresses /= inverseFactor;
            
            result += aPresses < 0 || bPresses < 0
                ? 0
                : aPresses * 3 + bPresses;
        }
        return result;
    }
};
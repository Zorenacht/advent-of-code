using FluentAssertions;
using System.Numerics;

namespace AoC_2024;

public sealed class Day09 : Day
{
    [Puzzle(answer: 6279058075753)]
    public long Part1()
    {
        int result = 0;
        var txt = InputAsText;
        var digits = txt.Select(x => x - '0').ToArray();
        //var even = digits.ToList().Where((d, i) => i % 2 == 0).ToArray();
        //var odd = digits.ToList().Where((d, i) => i % 2 == 1).ToArray();
        var arr = new List<int?>();
        for (int i = 0; i < digits.Length; ++i)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < digits[i]; ++j)
                {
                    arr.Add(i / 2);
                }
            }
            else
            {
                for (int j = 0; j < digits[i]; ++j)
                {
                    arr.Add(null);
                }
            }
        }

        int left = 0;
        int right = arr.Count - 1;
        while (left < right)
        {
            if (arr[left] != null) { ++left; continue; }
            if (arr[right] == null) { --right; continue; }
            arr[left] = arr[right];
            arr[right] = null;
        }

        return arr.Where(d => d is not null)
            .Select((d, i) => (long)(d!.Value * i))
            .Sum();
    }

    private record Fs(List<(int Index, int Length)> Files);
    private record Space(int Length);

    [Puzzle(answer: null)]
    public long Part2()
    {
        int result = 0;
        var txt = InputExampleAsText;
        var digits = txt.Select(x => x - '0').ToArray();
        //var even = digits.ToList().Where((d, i) => i % 2 == 0).ToArray();
        //var odd = digits.ToList().Where((d, i) => i % 2 == 1).ToArray();
        var arr = new List<int?>();
        for (int i = 0; i < digits.Length; ++i)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < digits[i]; ++j)
                {
                    arr.Add(i / 2);
                }
            }
            else
            {
                for (int j = 0; j < digits[i]; ++j)
                {
                    arr.Add(null);
                }
            }
        }

        int left = 0;
        int right = arr.Count - 1;
        while (left < right)
        {
            if (arr[left] != null) { ++left; continue; }
            if (arr[right] == null) { --right; continue; }

            int emptyIndex = left;
            int emptyLength = 0;
            while (arr[emptyIndex++] == null) { emptyLength++; }

            int fileToBeMovedLength = 0;
            int fileToBeMovedValue = arr[right]!.Value;
            while (arr[right] == fileToBeMovedValue) { fileToBeMovedLength++; right--; }

            if (fileToBeMovedLength <= emptyLength)
            {
                for (int i = 0; i < fileToBeMovedLength; ++i)
                {
                    arr[left + i] = fileToBeMovedValue;
                    arr[right + 1 + i] = null;
                }
            }
        }

        return arr.Where(d => d is not null)
            .Select((d, i) => (long)(d!.Value * i))
            .Sum();
    }

    //[Puzzle(answer: null)]
    //public long Part2()
    //{
    //    int result = 0;
    //    var txt = InputExampleAsText;
    //    var digits = txt.Select(x => x - '0').ToArray();
    //    var even = digits.ToList().Select((d, i) => (d, i)).Where((d, i) => i % 2 == 0).ToList();
    //    var odd = digits.ToList().Where((d, i) => i % 2 == 1).ToList();

    //    int index = 0;
    //    var files = new List<(int Value, int Length)>();
    //    var empty = new List<(int Value, int Length)>();
    //    for (int i = 0; i < digits.Length; ++i)
    //    {
    //        if(i % 2 == 0) files.Add((i/2, digits[i]));
    //        else empty.Add((0, digits[i]));
    //    }

    //    var total = new List<(int Value, int Length)>();
    //    int fIndex = 0;
    //    for (int i = 0; i < empty.Count && i < files.Count; ++i)
    //    {
    //        total.Add(files[0]);

    //        int length = empty[i].Length;
    //        for (int j = files.Count - 1; j >= 0; --j)
    //        {
    //            if (files[j].Length <= length)
    //            {
    //                length -= files[j].Length;
    //                empty.Insert(i, files[j]);
    //                total.Add(files[j]);
    //                ++i;
    //                files.RemoveAt(j);
    //            }
    //        }
    //        empty[i] = (0, length);
    //        total.Add(empty[i]);

    //        fIndex++;
    //    }

    //    return 0;

    //    //return arr.Where(d => d is not null)
    //    //    .Select((d, i) => (long)(d!.Value * i))
    //    //    .Sum();
    //}
};
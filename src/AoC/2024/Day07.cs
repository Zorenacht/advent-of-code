namespace AoC_2024;

public sealed class Day07 : Day
{
    [Puzzle(answer: 001_708_857_123_053)]
    public long Part1() => Input.Sum(line => new Operator(line).CalibrationValue(false));


    [Puzzle(answer: 189_207_836_795_655)]
    public long Part2() => Input.Sum(line => new Operator(line).CalibrationValue(true));

    private class Operator
    {
        private long _goal;
        private long[] _nums;

        public Operator(string line)
        {
            var splitted = line.Split(": ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            _goal = long.Parse(splitted[0]);
            _nums = splitted[1..]
                .Select(long.Parse)
                .ToArray();
        }

        public long CalibrationValue(bool or) => IsValid(0, 0, or) ? _goal : 0;

        private bool IsValid(int index, long current, bool or)
        {
            if (index == _nums.Length)
                return current == _goal;
            return IsValid(index + 1, current + _nums[index], or)
                || IsValid(index + 1, current * _nums[index], or)
                || or && IsValid(index + 1, long.Parse(current.ToString() + _nums[index].ToString()), or);
        }
    }
};
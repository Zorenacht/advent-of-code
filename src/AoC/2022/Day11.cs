using AoC._2022.Input;
namespace AoC._2022;

public sealed class Day11 : Day
{
    [Test]
    public void Example() => Simulate(Day11Input.ExampleInput, 20, 3).Should().Be(10605);
    [Test]
    public void Part1() => Simulate(Day11Input.Input, 20, 3).Should().Be(110220);
    [Test]
    public void ExampleP2() => Simulate(Day11Input.ExampleInput, 10000, 1).Should().Be(2713310158);
    [Test]
    public void Part2() => Simulate(Day11Input.Input, 10000, 1).Should().Be(19457438264);
    
    private const int Prod = 2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23;
    
    private long Simulate(List<Monkey> monkeys, int turns, int divisor)
    {
        for (int i = 0; i < turns; i++)
        {
            foreach (var monkey in monkeys)
            {
                var monkeyItems = monkey.Items;
                monkey.Items = new List<long>();
                foreach (var item in monkeyItems)
                {
                    var newWorry = monkey.CauseWorry(item, Prod, divisor);
                    var throwsTo = monkey.Throw(newWorry);
                    monkeys[throwsTo].Items.Add(newWorry);
                }
            }
            var topMonkeys2 = monkeys.Select(monkey => monkey.Operations).ToArray();
        }
        var topMonkeys = monkeys.Select(monkey => monkey.Operations).OrderByDescending(n => n).ToArray();
        return (long)topMonkeys[0] * topMonkeys[1];
    }

    public class Monkey
    {
        public List<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public Func<long, int> Throw { get; set; }
        public int Operations { get; set; }
        public int Prod { get; set; }

        public Monkey(List<long> items, Func<long, long> operation, Func<long, int> throwFunc)
        {
            Items = items;
            Operation = operation;
            Throw = throwFunc;
        }

        public long CauseWorry(long item, int prod, int divisor)
        {
            Operations++;
            if (Operation(item) < 0)
            {
                throw new Exception();
            }
            return Operation(item) / divisor % prod;
        }

        public static Func<long, int> CreateTest(int divisor, int onTrue, int onFalse) =>
            val => val % divisor == 0 ? onTrue : onFalse;
    }
}
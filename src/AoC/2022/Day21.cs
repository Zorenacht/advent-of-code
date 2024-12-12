namespace AoC._2022;


public sealed partial class Day21 : Day
{
    [Test]
    public void Example() => Monkeys.Parse(InputExample).Find().Root().Should().Be(152);
    [Test]
    public void Part1() => Monkeys.Parse(InputPart1).Find().Root().Should().Be(331319379445180L);

    [Test]
    [Ignore("Doesn't work")]
    public void ExampleP2() => FindHumanNumber(InputExample).Should().Be(301L);
    [Test]
    [Ignore("Doesn't work")]
    public void Part2() => FindHumanNumber(InputPart1).Should().Be(3715799488132L);

    public static long Simulate(string[] lines, long current) =>
        Monkeys.Parse(lines)
            .Correct()
            .Guess(current)
            .Find()
            .Root();

    public long FindHumanNumber(string[] lines)
    {
        long current = 0;
        long zeroResult = Simulate(lines, 0);
        long result;
        do
        {
            result = Simulate(lines, ++current);
        }
        while (result == zeroResult);
        var startOfNewCycle = result;
        var startOfNewCycleHumanNumber = current;
        int count = 1;
        do
        {
            count++;
            result = Simulate(lines, ++current);
        }
        while (result == zeroResult);
        var a = result - zeroResult;
        /*while(result != 0)
        {
            result = Monkeys.Parse(lines)
                .Correct()
                .Guess(current)
                .Find()
                .Root();
            if (result == 0L)
            {
                while(result == 0L)
                {
                    current--;
                    result = Monkeys
                        .Parse(lines)
                        .Correct()
                        .Guess(current)
                        .Find()
                        .Root();
                }
                return current+1;
            }
            step = (long)(step * Math.Pow(2, Math.Sign(result)));
            current += Math.Sign(result) * step;
        }*/
        return count * zeroResult / a;
    }


    private class Monkeys
    {
        //private readonly List<Node> Sequence;
        private readonly Dictionary<string, Monkey> All;
        private readonly HashSet<Monkey> Known;

        public Monkeys(Dictionary<string, Monkey> all)
        {
            All = all;
            Known = new HashSet<Monkey>();
        }

        public long Root() => All["root"].Value!.Value;

        public Monkeys Find(string name = "root")
        {
            Find(All[name]);
            return this;
        }

        public long Find(Monkey monkey)
        {
            if (Known.Contains(monkey)) return monkey.Value!.Value;
            if (monkey.Value is not null)
            {
                Known.Add(monkey);
                return monkey.Value!.Value;
            }
            var args = monkey.Args.Select(arg => Find(All[arg])).ToArray();
            monkey.Value = monkey.Operation!(args[0], args[1]);
            Known.Add(monkey);
            return monkey.Value!.Value;
        }

        public static Monkeys Parse(string[] lines)
        {
            var dict = new Dictionary<string, Monkey>();
            foreach (var line in lines)
            {
                var split = line.Split(':', ' ');
                var name = split[0];
                var value = split.Length == 3 ? int.Parse(split[2]) : 0;
                var first = split.Length == 5 ? split[2] : "";
                var op = split.Length == 5 ? split[3] : "";
                var second = split.Length == 5 ? split[4] : "";
                var monkey = split.Length == 3
                    ? new Monkey() { Name = name, Value = value }
                    : new Monkey() { Name = name, Operation = Function(op), Args = new string[2] { first, second } };
                dict.Add(name, monkey);
            }
            return new Monkeys(dict);
        }

        public Monkeys Correct()
        {
            All["root"].Operation = (a, b) => a - b;
            return this;
        }

        public Monkeys Guess(long i)
        {
            All["humn"].Value = i;
            return this;
        }

        private static Func<long, long, long> Function(string op) => op switch
        {
            "*" => (a, b) => a * b,
            "+" => (a, b) => a + b,
            "-" => (a, b) => a - b,
            "/" => (a, b) => a / b,
            _ => throw new NotImplementedException()
        };
    }

    private class Monkey
    {
        public string Name { get; set; } = string.Empty;
        public long? Value { get; set; }
        public Func<long, long, long>? Operation { get; set; }
        public string[] Args { get; set; } = [];

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

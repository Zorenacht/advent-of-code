namespace AoC_2022;

public sealed partial class Day11 : Day
{
    [Test]
    public void Example() => Simulate(1).Should().Be(10605);
    [Test]
    public void Part1() => Simulate(20).Should().Be(110220);
    [Test]
    public void ExampleP2() => Simulate2(1).Should().Be(0);
    [Test]
    public void Part2() => Simulate2(10000).Should().Be(19457438264);

    public int Simulate(int turns)
    {
        var monkeys = InputPart1();
        for (int i = 0; i < turns; i++)
        {
            foreach (var monkey in monkeys)
            {
                var monkeyItems = monkey.Items;
                monkey.Items = new List<int>();
                foreach (var item in monkeyItems)
                {
                    var newWorry = monkey.OperateOn(item);
                    var throwsTo = monkey.Test(newWorry);
                    monkeys[throwsTo].Items.Add(newWorry);
                }
            }
        }
        var topMonkeys = monkeys.OrderByDescending(monkey => monkey.Operations).Take(2).ToArray();
        return topMonkeys[0].Operations * topMonkeys[1].Operations;
    }

    public long Simulate2(int turns)
    {
        var monkeys = InputPart2();
        for (int i = 0; i < turns; i++)
        {
            foreach (var monkey in monkeys)
            {
                var monkeyItems = monkey.Items;
                monkey.Items = new List<ModuloInt>();
                foreach (var item in monkeyItems)
                {
                    var newWorry = monkey.OperateOn(item);
                    var throwsTo = monkey.Test(newWorry);
                    monkeys[throwsTo].Items.Add(newWorry);
                }
            }
        }
        var topMonkeys = monkeys.OrderByDescending(monkey => monkey.Operations).Take(2).ToArray();
        return (long)topMonkeys[0].Operations * (long)topMonkeys[1].Operations;
    }

    private List<Monkey> InputPart1()
    {
        return new List<Monkey>() {
            new Monkey
            {
                Items = new List<int>() { 53, 89, 62, 57, 74, 51, 83, 97 },
                Operation = (old) => old * 3,
                Test = Monkey.CreateTest(13,1,5)
            },
            new Monkey
            {
                Items = new List<int>() { 85, 94, 97, 92, 56 },
                Operation = (old) => old + 2,
                Test = Monkey.CreateTest(19,5,2)
            },
            new Monkey
            {
                Items = new List<int>() { 86, 82, 82},
                Operation = (old) => old + 1,
                Test = Monkey.CreateTest(11,3,4)
            },
            new Monkey
            {
                Items = new List<int>() { 94, 68},
                Operation = (old) => old + 5,
                Test = Monkey.CreateTest(17,7,6)
            },
            new Monkey
            {
                Items = new List<int>() { 83, 62, 74, 58, 96, 68, 85 },
                Operation = (old) => old + 4,
                Test = Monkey.CreateTest(3,3,6)
            },
            new Monkey
            {
                Items = new List<int>() { 50, 68, 95, 82 },
                Operation = (old) => old + 8,
                Test = Monkey.CreateTest(7,2,4)
            },
            new Monkey
            {
                Items = new List<int>() { 75},
                Operation = (old) => old * 7,
                Test = Monkey.CreateTest(5,7,0)
            },
            new Monkey
            {
                Items = new List<int>() { 92, 52, 85, 89, 68, 82 },
                Operation = (old) => old * old,
                Test = Monkey.CreateTest(2,0,1)
            },
        };
    }

    private List<MonkeyV2> InputPart2()
    {
        return new List<MonkeyV2>() {
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 53, 89, 62, 57, 74, 51, 83, 97 }),
                Operation = old => old * 3,
                Test = MonkeyV2.CreateTest(13,1,5)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 85, 94, 97, 92, 56 }),
                Operation = old => old + 2,
                Test = MonkeyV2.CreateTest(19,5,2)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 86, 82, 82 }),
                Operation = old => old + 1,
                Test = MonkeyV2.CreateTest(11,3,4)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 94, 68 }),
                Operation = old => old + 5,
                Test = MonkeyV2.CreateTest(17,7,6)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 83, 62, 74, 58, 96, 68, 85 }),
                Operation = old => old + 4,
                Test = MonkeyV2.CreateTest(3,3,6)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 50, 68, 95, 82 }),
                Operation = old => old + 8,
                Test = MonkeyV2.CreateTest(7,2,4)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList(new List<int>() { 75 }),
                Operation = old => old * 7,
                Test = MonkeyV2.CreateTest(5,7,0)
            },
            new MonkeyV2
            {
                Items = MonkeyV2.ModuloIntList( new List<int>() { 92, 52, 85, 89, 68, 82 }),
                Operation = old => old * old,
                Test = MonkeyV2.CreateTest(2,0,1)
            },
        };
    }

    private class Monkey
    {
        public List<int> Items { get; set; }

        private Func<int, int> _operation;
        public Func<int, int> Operation
        {
            get { return _operation; }
            set { _operation = val => value(val) / 3; }
        }
        public Func<int, int> Test { get; set; }
        public int Operations { get; set; }

        public int OperateOn(int item)
        {
            Operations++;
            return Operation(item);
        }


        public static Func<int, int> CreateTest(int divisor, int onTrue, int onFalse) =>
            (val) => val % divisor == 0 ? onTrue : onFalse;
    }

    private class MonkeyV2
    {
        public new List<ModuloInt> Items { get; set; }

        private Func<ModuloInt, ModuloInt> _operation;
        public Func<ModuloInt, ModuloInt> Operation
        {
            get { return _operation; }
            set { _operation = val => value(val); }
        }
        public Func<ModuloInt, int> Test { get; set; }
        public int Operations { get; set; }

        public ModuloInt OperateOn(ModuloInt item)
        {
            Operations++;
            return Operation(item);
        }

        public static List<ModuloInt> ModuloIntList(List<int> list)
        {
            var newList = new List<ModuloInt>();
            foreach (var number in list)
            {
                newList.Add(new ModuloInt(number));
            }
            return newList;
        }

        public static Func<ModuloInt, int> CreateTest(int divisor, int onTrue, int onFalse) =>
            val => val.Modulos[divisor] == 0 ? onTrue : onFalse;

    }

    public class ModuloInt
    {
        private static int[] Primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };

        public Dictionary<int, int> Modulos { get; set; }

        public ModuloInt(int number)
        {
            Modulos = new Dictionary<int, int>();
            foreach (int prime in Primes)
            {
                Modulos.Add(prime, number % prime);
            }
        }

        public ModuloInt(Dictionary<int, int> modulos)
        {
            Modulos = modulos;
        }

        public static ModuloInt operator +(ModuloInt left, int number)
        {
            var modulos = new Dictionary<int, int>();
            foreach (int prime in Primes)
            {
                modulos[prime] = (left.Modulos[prime] + number) % prime;
            }
            return new ModuloInt(modulos);
        }

        public static ModuloInt operator *(ModuloInt left, int number)
        {
            var modulos = new Dictionary<int, int>();
            foreach (int prime in Primes)
            {
                modulos[prime] = (left.Modulos[prime] * number) % prime;
            }
            return new ModuloInt(modulos);
        }

        public static ModuloInt operator *(ModuloInt left, ModuloInt right)
        {
            var modulos = new Dictionary<int, int>();
            foreach (int prime in Primes)
            {
                modulos[prime] = (left.Modulos[prime] * right.Modulos[prime]) % prime;
            }
            return new ModuloInt(modulos);
        }

        public override string ToString()
        {
            return string.Join(",", Modulos);
        }
    }
}
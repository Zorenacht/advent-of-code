namespace Performance.Tests;

public class DatastructureTests
{
    private int Times = 10_000;
    private const int Length = 10_000;

    protected record Int(int Number);

    public class RangeTests : DatastructureTests
    {
        [Test]
        public void CreateArrayRange()
        {
            var list = new Int[Length];
            for (int i = 0; i < Times; i++)
            {
                list = new Int[Length];
                for (int j = 0; j < Length; j++) list[j] = new Int(j);
            }
        }

        [Test]
        public void CreateListRange()
        {
            var list = new List<Int>();
            for (int i = 0; i < Times; i++)
            {
                list = new List<Int>(Length);
                for (int j = 0; j < Length; j++) list.Add(new Int(j));
            }
        }

        [Test]
        public void CreateRangeWithLinq()
        {
            var list = new List<Int>();
            for (int i = 0; i < Times; i++)
            {
                list = Enumerable.Range(0, Length).Select(num => new Int(num)).ToList();
            }
        }
    }

    public class InitializeTests : DatastructureTests
    {

        [Test]
        public void InitializeArrays()
        {
            var list = new int[Length];
            for (int i = 0; i < Times; i++)
            {
                list = new int[Length];
            }
        }

        [Test]
        public void InitializeLists()
        {
            var list = new List<int>();
            for (int i = 0; i < Times; i++)
            {
                list = new List<int>(Length);
            }
        }

        [Test]
        public void InitializeWithLinq()
        {
            var list = new List<int>();
            for (int i = 0; i < Times; i++)
            {
                list = Enumerable.Repeat(0, Length).ToList();
            }
        }
    }
}
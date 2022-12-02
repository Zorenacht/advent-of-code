namespace AoC_2022;

public class Day02 : Day
{
    [Test]
    public void Example()
    {
        int score = 0;
        foreach (string line in InputExample)
        {
            var split = line.Split(' ');
            int opponent = split[0][0] - 'A';
            int you = split[1][0] - 'X';
            score += (you + 1) + 3 * Modulo(you - opponent + 1, 3);
        }
        score.Should().Be(15);
    }

    [Test]
    public void Part1()
    {
        int score = 0;
        foreach (string line in InputPart1)
        {
            var split = line.Split(' ');
            int opponent = split[0][0] - 'A';
            int you = split[1][0] - 'X';
            score += (you + 1) + 3 * Modulo(you - opponent + 1, 3);
        }
        score.Should().Be(9241);
    }

    [Test]
    public void Part2()
    {
        int score = 0;
        foreach (string line in InputPart2)
        {
            var split = line.Split(' ');
            int opponent = split[0][0] - 'A';
            int goal = split[1][0] - 'X'; //0 loss, 1 draw, 2 win
            int offset = goal - 1; //-1 loss, 0 draw, 1 win 
            score += Modulo(opponent + offset, 3) + 1;
            score += 3 * goal;
        }
        score.Should().Be(14610);
    }

    [Test]
    public void Modulo()
    {
        Modulo(-4, 3).Should().Be(2);
        Modulo(-3, 3).Should().Be(0);
        Modulo(-2, 3).Should().Be(1);
        Modulo(-1, 3).Should().Be(2);
        Modulo(0, 3).Should().Be(0);
        Modulo(1, 3).Should().Be(1);
        Modulo(2, 3).Should().Be(2);
        Modulo(3, 3).Should().Be(0);
    }

    //Modulo such that -1 % n = n-1 for positive n
    private int Modulo(int a, int b)
    {
        int remainder = a % b;
        return remainder >= 0 ? remainder : remainder + b;
    }
}
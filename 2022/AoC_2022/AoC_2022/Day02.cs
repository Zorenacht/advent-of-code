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
            char opponent = split[0][0];
            char you = (char)(split[1][0] - ('X' - 'A'));
            score += you - 'A' + 1;
            if (opponent == you)
            {
                score += 3;
            }
            else if (opponent == 'A' && you == 'B' || opponent == 'B' && you == 'C' || opponent == 'C' && you == 'A')
            {
                score += 6;
            }
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
            char opponent = split[0][0];
            char you = (char)(split[1][0] - ('X' - 'A'));
            score += you - 'A' + 1;
            if (opponent == you)
            {
                score += 3;
            }
            else if (opponent == 'A' && you == 'B' || opponent == 'B' && you == 'C' || opponent == 'C' && you == 'A')
            {
                score += 6;
            }
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
            char opponent = split[0][0];
            char you = split[1][0];
            var offset = (you - 'Y') % 3; //+0 for draw, +1 for win, +2 for loss
            score += Modulo(opponent - 'A' + offset,3) + 1;
            score += 3 * (you - 'X');
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

    private int Modulo(int a, int b)
    {
        int remainder = a % b;
        return remainder >= 0 ? remainder : remainder + b;
    }
}
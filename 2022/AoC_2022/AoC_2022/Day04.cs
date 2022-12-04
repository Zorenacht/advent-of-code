namespace AoC_2022;

public class Day04 : Day
{
    [Test]
    public void Example()
    {
        int score = 0;
        foreach (string line in InputExample)
        {
            var split = line.Split('-', ',');
            int x1 = int.Parse(split[0]);
            int y1 = int.Parse(split[1]);
            int x2 = int.Parse(split[2]);
            int y2 = int.Parse(split[3]);
            var ix = Math.Max(x1, x2);
            var iy = Math.Min(y1, y2);
            if( ix == x1 && iy == y1 || ix == x2 && iy == y2)
            {
                score++;
            }
        }
        score.Should().Be(2);
    }

    [Test]
    public void Part1()
    {
        int score = 0;
        foreach (string line in InputPart1)
        {
            var split = line.Split('-', ',');
            int x1 = int.Parse(split[0]);
            int y1 = int.Parse(split[1]);
            int x2 = int.Parse(split[2]);
            int y2 = int.Parse(split[3]);
            var ix = Math.Max(x1, x2);
            var iy = Math.Min(y1, y2);
            if (ix == x1 && iy == y1 || ix == x2 && iy == y2)
            {
                score++;
            }
        }
        score.Should().Be(538);
    }

    [Test]
    public void Part2()
    {
        int score = 0;
        foreach (string line in InputPart2)
        {
            var split = line.Split('-', ',');
            int x1 = int.Parse(split[0]);
            int y1 = int.Parse(split[1]);
            int x2 = int.Parse(split[2]);
            int y2 = int.Parse(split[3]);
            var ix = Math.Max(x1, x2);
            var iy = Math.Min(y1, y2);
            if (iy - ix >= 0)
            {
                score++;
            }
        }
        score.Should().Be(792);
    }
}
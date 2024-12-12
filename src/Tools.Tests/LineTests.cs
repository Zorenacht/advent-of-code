using System.Text;
using Tools.Shapes;

namespace Tools.Tests;

public class Intersection
{
    [Test]
    public void orthogonal_succeeds()
    {
        var line1 = new StraightLine(new Index2D(0, 1), new Index2D(0, -1));
        var line2 = new StraightLine(new Index2D(1, 0), new Index2D(-1, 0));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(Index2D.O);
    }

    [Test]
    public void orthogonal_too_short_fails()
    {
        var line1 = new StraightLine(new Index2D(0, 0), new Index2D(0, 5));
        var line2 = new StraightLine(new Index2D(2, 4), new Index2D(3, 4));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(null);
    }

    [Test]
    public void testing()
    {
        var sol = new Solution().Multiply("123", "456");
        sol.Should().BeEquivalentTo("56088");
    }

    public class Solution
    {
        public string Multiply(string num1, string num2)
        {
            var n1 = new int[num1.Length];
            var n2 = new int[num2.Length];
            for (int i = 0; i < num1.Length; ++i) n1[i] = int.Parse(num1[num1.Length - 1 - i].ToString());
            for (int i = 0; i < num2.Length; ++i) n2[i] = int.Parse(num2[num2.Length - 1 - i].ToString());

            var answer = Multiply(n1, n2);

            bool hasStarted = false;
            var sb = new StringBuilder();
            for (int i = answer.Length - 1; i >= 0; i--)
            {
                if (answer[i] != 0 && !hasStarted) hasStarted = true;
                if (hasStarted) sb.Append(answer[i]);
            }
            return sb.Length > 0 ? sb.ToString() : "0";
        }

        private int[] Multiply(int[] n1, int[] n2)
        {
            var answer = new int[400];
            int carry = 0;
            int i, j;
            for (i = 0; i < n1.Length; ++i)
            {
                for (j = 0; j < n2.Length; ++j)
                {
                    (answer[i + j], carry) = Multiply(n1[i] * n2[j] + carry + answer[i + j]);
                }
                (answer[i + j], carry) = Multiply(carry + answer[i + j]);
            }
            return answer;

            static (int Digit, int Carry) Multiply(int multiplication)
            {
                return (multiplication % 10, multiplication / 10);
            }
        }
    }
}
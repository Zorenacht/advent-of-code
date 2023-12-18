using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
namespace AoC_2023;

public sealed class Day12 : Day
{
    [Puzzle(answer: 21)]
    public long Part1Example()
        => InputExample.Select(line => new HotSprings(line, 1).Arrangements()).Sum();

    [Puzzle(answer: 7251)]
    public long Part1()
        => Input.Select(line => new HotSprings(line, 1).Arrangements()).Sum();


    [Puzzle(answer: 525152)]
    public long Part2Example()
        => InputExample.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    [Puzzle(answer: 2128386729962)]
    public long Part2()
        => Input.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    public class HotSprings
    {
        private readonly string _singleTemplate;
        private readonly int[] _singleSequence;
        private int _repeats;
        private readonly string _template;
        private readonly int[] _sequence;
        private readonly Dictionary<string, Matrix<double>> _cache;

        public HotSprings(string line, int repeat)
        {
            _singleTemplate = line.Split()[0];
            _singleSequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            _repeats = repeat;
            _template = string.Join("?", Enumerable.Repeat(_singleTemplate, repeat));
            _sequence = Enumerable.Repeat(_singleSequence, repeat).SelectMany(x => x).ToArray();
            _cache = new Dictionary<string, Matrix<double>>();

        }

        private Matrix<double> Power(Matrix<double> matrix, int power)
        {
            if (power == 0) return Matrix<double>.Build.SparseIdentity(matrix.RowCount);
            if (power == 1) return matrix;
            if (power % 2 == 0)
            {
                return Power(matrix * matrix, power / 2);
            }
            return matrix * Power(matrix, (power - 1) / 2);
        }

        private Matrix<double> FromTemplate(string template)
        {
            if (template.Length == 1) return _cache[template];
            if(_cache.ContainsKey(template)) return _cache[template];
            return FromTemplate(template[..(template.Length / 2)]) * FromTemplate(template[(template.Length / 2)..]);
        }

        public long Arrangements()
        {
            var states = _sequence.Sum(l => l + 1);
            var indices = new int[_sequence.Length + 1];
            for (int i = 1; i <= _sequence.Length; i++)
            {
                indices[i] = indices[i - 1] + _sequence[i - 1] + 1;
            }
            var dotMatrix = Matrix<double>.Build.Sparse(indices[^1] + 1, indices[^1] + 1);
            dotMatrix[0, 0] = 1;
            for (int i = 1; i < indices.Length; i++)
            {
                int row = indices[i];
                dotMatrix[row - 1, row] = 1;
                dotMatrix[row, row] = 1;
            }
            var hashMatrix = Matrix<double>.Build.Sparse(indices[^1] + 1, indices[^1] + 1);
            for (int i = 0; i < indices.Length - 1; i++)
            {
                int row = indices[i];
                int nextRow = indices[i + 1];
                for (int j = row; j < nextRow - 1; j++)
                {
                    hashMatrix[j, j + 1] = 1;
                }
            }
            var questionMatrix = dotMatrix + hashMatrix;

            _cache["."] = dotMatrix;
            _cache["#"] = hashMatrix;
            _cache["?"] = questionMatrix;

            var singleMatrix = FromTemplate(_singleTemplate);
            var mat = singleMatrix * Power(questionMatrix * singleMatrix, _repeats-1);
            var vector = Vector<double>.Build.Sparse(indices[^1] + 1);
            vector[0] = 1;
            var result = vector * mat;
            return (long)(result[^1] + result[^2]);
        }
    }
}
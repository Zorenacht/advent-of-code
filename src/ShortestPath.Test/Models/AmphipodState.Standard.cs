using System.Text;

namespace ShortestPath.Test.Models;

public sealed partial class AmphipodState : IState<AmphipodState>, IEnumerable<AmphipodPoint>
{
    public bool Equals(AmphipodState? other)
    {
        return
            other is not null &&
            Enumerable.SequenceEqual(Hallway, other.Hallway) &&
            SequenceEquals(Rooms, other.Rooms);
    }

    //taken from stackexchange
    private static bool SequenceEquals<T>(T[,] a, T[,] b) => a.Rank == b.Rank
        && Enumerable.Range(0, a.Rank).All(d => a.GetLength(d) == b.GetLength(d))
        && a.Cast<T>().SequenceEqual(b.Cast<T>());

    public override int GetHashCode()
    {
        var sb = new StringBuilder();
        foreach (var point in this)
        {
            sb.Append(this[point]);
        }
        return HashCode.Combine(sb.ToString());
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Concat(Hallway));
        for (int i = 0; i < Rooms.GetLength(0); i++)
        {
            sb.Append(" #");
            for (int j = 0; j < Rooms.GetLength(1); j++)
            {
                sb.Append(Rooms[i, j] + "#");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
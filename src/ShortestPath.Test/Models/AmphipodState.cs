using NuGet.Frameworks;
using System.Collections;

namespace ShortestPath.Test.Models;

public enum Amphipod
{
    O = 0,
    A = 1,
    B = 2,
    C = 3,
    D = 4,
}

public readonly struct AmphipodPoint
{
    public AmphipodPoint(int hallwayCol)
    {
        Col = hallwayCol;
    }

    public AmphipodPoint(int roomRow, int roomCol)
    {
        Col = roomCol;
        Row = roomRow;
    }

    public bool IsHallwayPoint => Row is null;
    public int Col { get; }
    public int? Row { get; }
}

public sealed partial class AmphipodState : IState<AmphipodState>, IEnumerable<AmphipodPoint>
{
    private Amphipod[] Hallway { get; set; } = new Amphipod[11];
    private Amphipod[,] Rooms { get; set; } = new Amphipod[2, 4];

    private int Weight(Amphipod amphipod) => amphipod switch
    {
        Amphipod.A => (int)Math.Pow(10, 0),
        Amphipod.B => (int)Math.Pow(10, 1),
        Amphipod.C => (int)Math.Pow(10, 2),
        Amphipod.D => (int)Math.Pow(10, 3),
        _ => throw new IndexOutOfRangeException(),
    };


    public IEnumerator<AmphipodPoint> GetEnumerator()
    {
        for (int col = 0; col < Hallway.Length; col++)
        {
            yield return new AmphipodPoint(col);
        }
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int col = 0; col < Rooms.GetLength(1); col++)
            {
                yield return new AmphipodPoint(row, col);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public AmphipodState(Amphipod[] hallway, Amphipod[,] rooms)
    {
        Hallway = hallway;
        Rooms = rooms;
    }

    public AmphipodState Permute(AmphipodPoint first, AmphipodPoint second)
    {
        //temporarily copy 
        var firstAmphipod = this[first];
        var secondAmphipod = this[second];

        //create copy of current array
        var hallway = new Amphipod[11];
        var rooms = new Amphipod[Rooms.GetLength(0), 4];
        Array.Copy(Hallway, hallway, 11);
        Array.Copy(Rooms, rooms, Rooms.GetLength(0) * 4);
        var newState = new AmphipodState(hallway, rooms);

        //swap first and second in array copy
        newState[second] = firstAmphipod;
        newState[first] = secondAmphipod;
        return new AmphipodState(hallway, rooms);
    }

    public IEnumerable<Node<AmphipodState>> NextNodes(int initialDistance)
    {
        var nodes = new List<Node<AmphipodState>>();
        foreach (var point in this)
        {
            var roomNodes = ValidMoves(point).Select(destinationPoint =>
            {
                var newState = Permute(point, destinationPoint);
                return new Node<AmphipodState>(
                    state: newState,
                    distance: initialDistance + Weight(this[point]) * Distance(point, destinationPoint));
            });
            nodes = nodes.Concat(roomNodes).ToList();
        }
        return nodes;
    }

    public int Heuristic()
    {
        int count = 0;
        foreach (var point in this)
        {
            if (this[point] != Amphipod.O)
            {
                int roomDestinationCol = AmphipodRoomCol(this[point]);
                if (point.IsHallwayPoint || point.Col != roomDestinationCol)
                {
                    count += Weight(this[point]) * Distance(point, new AmphipodPoint(AmphipodHallwayCol(this[point])));
                }
            }
        }
        count += Weight(Amphipod.A) * SpaceInRoom(Amphipod.A);
        count += Weight(Amphipod.B) * SpaceInRoom(Amphipod.B);
        count += Weight(Amphipod.C) * SpaceInRoom(Amphipod.C);
        count += Weight(Amphipod.D) * SpaceInRoom(Amphipod.D);
        return count;
    }


    private bool HallwayColHasRoom(int hallwayCol) => HallwayColsWithRoom.Contains(hallwayCol);
    private int[] HallwayColsWithRoom = new int[] { 2, 4, 6, 8 };
    private int RoomToHallwayCol(int roomCol) => roomCol >= 0 && roomCol <= 3
        ? 2 + 2 * roomCol
        : throw new IndexOutOfRangeException();

    private int Distance(AmphipodPoint first, AmphipodPoint second)
    {
        if (!first.IsHallwayPoint && !second.IsHallwayPoint)
        {
            var leaveFirstRoom = first.Row!.Value + 1;
            var leaveSecondRoom = second.Row!.Value + 1;
            var hallwayFirstToSecondRoom = RoomToHallwayCol(first.Col);
            return leaveFirstRoom + leaveSecondRoom + hallwayFirstToSecondRoom;
        }
        else if (!first.IsHallwayPoint && second.IsHallwayPoint)
        {
            return ManhattanDistance(
                first.Row!.Value + 1,
                RoomToHallwayCol(first.Col),
                0,
                second.Col);
        }
        else if (first.IsHallwayPoint && !second.IsHallwayPoint)
        {
            return ManhattanDistance(
                0,
                first.Col,
                second.Row!.Value + 1,
                RoomToHallwayCol(second.Col));
        }
        else if (first.IsHallwayPoint && second.IsHallwayPoint)
        {
            return ManhattanDistance(
                0,
                first.Col,
                0,
                second.Col);
        }
        throw new NotSupportedException();
    }

    private int ManhattanDistance(int x1, int y1, int x2, int y2)
        => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
}
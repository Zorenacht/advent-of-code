using ShortestPath;

namespace AoC._2021.Models;

public sealed partial class AmphipodState : IState<AmphipodState>, IEnumerable<AmphipodPoint>
{
    private bool HasAmphipodIn(AmphipodPoint point)
        => point.IsHallwayPoint
        ? Hallway[point.Col] != Amphipod.O
        : Rooms[point.Row!.Value, point.Col] != Amphipod.O;

    private Amphipod this[AmphipodPoint point]
    {
        get
        {
            return point.IsHallwayPoint
                ? Hallway[point.Col]
                : Rooms[point.Row!.Value, point.Col];
        }
        set
        {
            if (point.IsHallwayPoint)
            {
                Hallway[point.Col] = value;
            }
            else
            {
                Rooms[point.Row!.Value, point.Col] = value;
            }
        }
    }

    public int SpaceInRoom(Amphipod amphipod)
    {
        var col = AmphipodRoomCol(amphipod);
        for (int row = Rooms.GetLength(0) - 1; row >= 0; row--)
        {
            if (Rooms[row, col] != amphipod)
            {
                return (row + 1);
            };
        }
        return 0;
    }

    private IEnumerable<AmphipodPoint> ValidMoves(AmphipodPoint point)
    {
        if (!HasAmphipodIn(point))
        {
            return Enumerable.Empty<AmphipodPoint>();
        }

        if (point.IsHallwayPoint)
        {
            int col = point.Col;
            var amphipod = Hallway[col];
            if (AmphipodCanGoToRoom(amphipod, col, out int roomRow))
            {
                return new List<AmphipodPoint>() { new AmphipodPoint(roomRow, AmphipodRoomCol(amphipod)) };
            };
        }
        else
        {
            int row = point.Row!.Value;
            int col = point.Col;

            if (TryEnterHallway(row, col))
            {
                return RoomToHallwayMoves(RoomToHallwayCol(col));
            }

        }
        return Enumerable.Empty<AmphipodPoint>();
    }

    public int AmphipodRoomCol(Amphipod amphipod) => amphipod switch
    {
        Amphipod.A => 0,
        Amphipod.B => 1,
        Amphipod.C => 2,
        Amphipod.D => 3,
        _ => throw new IndexOutOfRangeException(),
    };

    public int AmphipodHallwayCol(Amphipod amphipod) => amphipod switch
    {
        Amphipod.A => 2,
        Amphipod.B => 4,
        Amphipod.C => 6,
        Amphipod.D => 8,
        _ => throw new IndexOutOfRangeException(),
    };

    private bool AmphipodCanGoToRoom(Amphipod amphipod, int fromCol, out int roomRow)
    {
        roomRow = -1;

        //Move through hallway to front of the room
        var toCol = AmphipodHallwayCol(amphipod);
        for (int col = fromCol + Math.Sign(toCol - fromCol); col != toCol; col += Math.Sign(toCol - col))
        {
            if (Hallway[col] != Amphipod.O)
            {
                return false;
            }
        }

        //Move in the room and check that room only contains the same kind of amphipod
        var roomCol = AmphipodRoomCol(amphipod);
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            var roomSpace = Rooms[row, roomCol];
            if (roomSpace != Amphipod.O && roomSpace != amphipod
                || row == 0 && roomSpace != Amphipod.O)
            {
                return false;
            }
            if (row > 0 && roomSpace == amphipod && roomRow == -1)
            {
                roomRow = row - 1;
            }
        }
        if (roomRow == -1)
        {
            roomRow = Rooms.GetLength(0) - 1;
        }
        return true;
    }

    private IEnumerable<AmphipodPoint> RoomToHallwayMoves(int hallwayCol)
    {
        //Check space directly outside of room
        if (Hallway[hallwayCol] != Amphipod.O) return Enumerable.Empty<AmphipodPoint>();

        var leftNodes = LeftHallway(hallwayCol - 1);
        var rightNodes = RightHallway(hallwayCol + 1);

        return leftNodes.Concat(rightNodes);
    }

    private IEnumerable<AmphipodPoint> LeftHallway(int col)
    {
        while (col >= 0)
        {
            if (Hallway[col] != Amphipod.O) break;
            if (HallwayColHasRoom(col))
            {
                col--;
                continue;
            }

            yield return new AmphipodPoint(col);
            col--;
        }
    }

    private IEnumerable<AmphipodPoint> RightHallway(int col)
    {
        while (col < 11)
        {
            if (Hallway[col] != Amphipod.O) break;
            if (HallwayColHasRoom(col))
            {
                col++;
                continue;
            }

            yield return new AmphipodPoint(col);
            col++;
        }
    }

    private bool TryEnterHallway(int row, int col)
    {
        row--;
        while (row >= 0)
        {
            if (Rooms[row, col] != Amphipod.O)
            {
                break;
            }
            row--;
        }
        return row == -1;
    }

}
using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Rider")]
public readonly struct Index2D(int row, int col) : IEquatable<Index2D>
{
    public Index2D(int row, int col, int direction) : this(row, col) => _direction = direction;
    
    public static readonly Index2D O = new(0, 0);

    public static readonly Index2D N = new(-1, +0, direction: 0);
    public static readonly Index2D NE = new(-1, +1, direction: 1);
    public static readonly Index2D E = new(+0, +1, direction: 2);
    public static readonly Index2D SE = new(+1, +1, direction: 3);
    public static readonly Index2D S = new(+1, +0, direction: 4);
    public static readonly Index2D SW = new(+1, -1, direction: 5);
    public static readonly Index2D W = new(+0, -1, direction: 6);
    public static readonly Index2D NW = new(-1, -1, direction: 7);
    public static Index2D[] Directions => [ N, NE, E, SE, S, SW, W, NW ];
    
    public Index2D TurnLeft() => Directions[(_direction + 6) % 8];
    public Index2D TurnRight() => Directions[(_direction + 2) % 8];
    
    public int Row { get; } = row;
    public int Col { get; } = col;
    private readonly int _direction = -1;

    public Index2D Neighbor(Direction dir)
        => dir switch
        {
            Direction.N => this + N,
            Direction.S => this + S,
            Direction.W => this + W,
            Direction.E => this + E,
            Direction.NW => this + NW,
            Direction.SW => this + SW,
            Direction.SE => this + SE,
            Direction.NE => this + NE,
            _ => throw new NotSupportedException()
        };

    public int Norm => Row * Row + Col * Col;
    public int Manhattan => Math.Abs(Row) + Math.Abs(Col);
    public override string ToString() => $"({Row},{Col})";
    public bool Equals(Index2D other) => other.Row == Row && other.Col == Col;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Index2D other && Equals(other);
    public override int GetHashCode() => (Row + Col) * (Row + Col + 1) / 2 + Col;

    public static bool operator ==(Index2D left, Index2D right) => left.Equals(right);
    public static bool operator !=(Index2D left, Index2D right) => !(left == right);
    public static Index2D operator -(Index2D self) => new(-self.Row, -self.Col);
    public static Index2D operator +(Index2D self) => self;
    public static Index2D operator +(Index2D left, Index2D right) => new(left.Row + right.Row, left.Col + right.Col);
    public static Index2D operator -(Index2D left, Index2D right) => new(left.Row - right.Row, left.Col - right.Col);
    public static Index2D operator +(Direction left, Index2D right) => right.Neighbor(left);
    public static Index2D operator -(Direction left, Index2D right) => -right.Neighbor(left);
    public static Index2D operator +(Index2D left, Direction right) => left.Neighbor(right);
    public static Index2D operator -(Index2D left, Direction right) => left.Neighbor(right.Backwards());
    public static Index2D operator *(int left, Index2D right) => new(left * right.Row, left * right.Col);
    public static Index2D operator *(Index2D left, int right) => new(left.Row * right, left.Col * right);
    public static Index2D operator /(Index2D left, int right) => new(left.Row / right, left.Col / right);
}
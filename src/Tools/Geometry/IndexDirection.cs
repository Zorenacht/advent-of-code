namespace Tools.Geometry;

public record IndexDirection(Index2D Index, Direction Direction);

public record IndexDirectionV2(Index2D Index, Index2D Direction)
{
    public IndexDirectionV2 TurnRight() => this with { Direction = Direction.TurnRight() };
    public IndexDirectionV2 TurnLeft() => this with { Direction = Direction.TurnLeft() };
    public IndexDirectionV2 Forward() => this with { Index = Index + Direction };
    
    public static IndexDirectionV2 OutOfBounds => new(new Index2D(-1, -1), Index2D.N);
};
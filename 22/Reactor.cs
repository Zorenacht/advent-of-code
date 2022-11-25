public record ReactorCuboid(bool On, int x1, int x2, int y1, int y2, int z1, int z2)
{
    public long Count => (long)(x2 - x1 + 1) * (y2 - y1 + 1) * (z2 - z1 + 1);
    public bool IsValid => x1 <= x2 && y1 <= y2 && z1 <= z2;

    public ReactorCuboid? Intersection(ReactorCuboid newCuboid)
    {
        var ix1 = Math.Max(x1, newCuboid.x1);
        var ix2 = Math.Min(x2, newCuboid.x2);
        var iy1 = Math.Max(y1, newCuboid.y1);
        var iy2 = Math.Min(y2, newCuboid.y2);
        var iz1 = Math.Max(z1, newCuboid.z1);
        var iz2 = Math.Min(z2, newCuboid.z2);
        var on = !On; //Only add on cuboid current one is off
        if (ix1 <= ix2 && iy1 <= iy2 && iz1 <= iz2)
        {
            return new ReactorCuboid(on, ix1, ix2, iy1, iy2, iz1, iz2);
        }
        return null;
    }
};

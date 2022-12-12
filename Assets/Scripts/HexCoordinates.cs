using System;

[Serializable]
public struct HexCoordinates
{
    public HexCoordinates(int x, int z)
    {
        X = x;
        Z = z;
    }

    public int X { get; private set; }
    public int Z { get; private set; }

    /// <summary>
    ///     Fixes the X coordinates so they are aligned along a straight axis.
    ///     Done by undoing the horizontal shift. The result is typically know as axial coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>HexCoordinates with x coordinates properly aligned along a straight</returns>
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Z + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X + "\n" + Z;
    }
}
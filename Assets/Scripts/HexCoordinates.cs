using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates
{
    [SerializeField] private int x, z;

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public int X => x;
    public int Z => z;

    public int Y => -X - Z;

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

    /// <summary>
    ///     Converts world space position to Hex coordinates
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        //Divide x by the horizontal width of the hexagon, y is a mirror of x
        var x = position.x / (HexMetrics.InnerRadius * 2f);
        var y = -x;

        //Shift as we move along the z axis.
        //every two rows we shift an entire unit left
        var offset = position.z / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        y -= offset;

        //Round the x and y values to integers 
        var iX = Mathf.RoundToInt(x);
        var iY = Mathf.RoundToInt(y);
        var iZ = Mathf.RoundToInt(-x - y);

        //If there is a rounding error, discard the coord with the largest
        //rounding delta and reconstruct it from the other two.
        if (iX + iY + iZ != 0)
        {
            var dX = Mathf.Abs(x - iX);
            var dY = Mathf.Abs(y - iY);
            var dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
                iX = -iY - iZ;
            else if (dZ > dY) iZ = -iX - iY;
        }

        return new HexCoordinates(iX, iZ);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ", " + Z + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X + "\n" + Y + "\n" + Z;
    }
}
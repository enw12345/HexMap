using UnityEngine;

public static class HexMetrics
{
    public const float OuterRadius = 10f;

    /// <summary>
    ///     The inner radius is equal to the height of one of the triangles that make up the hexagon:
    ///     sqrt(pow(edge, 2) - pow(edge/2, 2)
    /// </summary>
    public const float InnerRadius = OuterRadius * 0.866025404f;

    public const float SolidFactor = 0.75f;
    public const float BlendFactor = 1f - SolidFactor;

    /// <summary>
    ///     vars for slopes
    /// </summary>
    public const float ElevationStep = 5f;

    public const int TerracesPerSlope = 2;
    public const int TerraceSteps = TerracesPerSlope * 2 + 1;
    public const float HorizontalTerraceStepSize = 1f / TerraceSteps;
    public const float VerticalTerraceStepSize = 1f / (TerracesPerSlope + 1);

    public static Texture2D NoiseSource;

    private static readonly Vector3[] Corners =
    {
        new(0f, 0f, OuterRadius),
        new(InnerRadius, 0f, 0.5f * OuterRadius),
        new(InnerRadius, 0f, -0.5f * OuterRadius),
        new(0f, 0f, -OuterRadius),
        new(-InnerRadius, 0f, -0.5f * OuterRadius),
        new(-InnerRadius, 0f, 0.5f * OuterRadius),
        new(0f, 0f, OuterRadius)
    };

    public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
    {
        if (elevation1 == elevation2)
            return HexEdgeType.Flat;
        var delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1) return HexEdgeType.Slope;
        return HexEdgeType.Cliff;
    }

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return Corners[(int) direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return Corners[(int) direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return Corners[(int) direction] * SolidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return Corners[(int) direction + 1] * SolidFactor;
    }

    public static Vector3 GetBridge(HexDirection direction)
    {
        return (Corners[(int) direction] + Corners[(int) direction + 1]) *
               BlendFactor;
    }

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
    {
        var h = step * HorizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;
        var v = (step + 1) / 2 * VerticalTerraceStepSize;
        a.y += (b.y - a.y) * v;
        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step)
    {
        var h = step * HorizontalTerraceStepSize;
        return Color.Lerp(a, b, h);
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return NoiseSource.GetPixelBilinear(
            position.x, position.z);
    }

    private static Vector3 Perturb(Vector3 position)
    {
        var sample = SampleNoise(position);
        position.x += sample.x;
        position.y += sample.y;
        position.z += sample.z;
        return position;
    }
}
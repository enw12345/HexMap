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
}
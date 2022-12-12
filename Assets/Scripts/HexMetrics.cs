using UnityEngine;

public static class HexMetrics
{
    public const float OuterRadius = 10f;

    /// <summary>
    ///     The inner radius is equal to the height of one of the triangles that make up the hexagon:
    ///     sqrt(pow(edge, 2) - pow(edge/2, 2)
    /// </summary>
    public const float InnerRadius = OuterRadius * 0.866025404f;

    public static readonly Vector3[] Corners =
    {
        new(0f, 0f, OuterRadius),
        new(InnerRadius, 0f, 0.5f * OuterRadius),
        new(InnerRadius, 0f, -0.5f * OuterRadius),
        new(0f, 0f, -OuterRadius),
        new(-InnerRadius, 0f, -0.5f * OuterRadius),
        new(-InnerRadius, 0f, 0.5f * OuterRadius),
        new(0f, 0f, OuterRadius)
    };
}
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates Coordinates;
    public Color Color;
    public RectTransform UIRect;

    [SerializeField] private HexCell[] neighbors;
    private int _elevation;

    public int Elevation
    {
        get => _elevation;
        set
        {
            _elevation = value;
            var position = transform.localPosition;
            position.y = value * HexMetrics.ElevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) *
                          HexMetrics.ElevationPerturbStrength;

            transform.localPosition = position;

            var uiPosition = UIRect.localPosition;
            uiPosition.z = -position.y;
            UIRect.localPosition = uiPosition;
        }
    }

    public Vector3 Position => transform.localPosition;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int) direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int) direction] = cell;
        cell.neighbors[(int) direction.Opposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(_elevation,
            neighbors[(int) direction].Elevation);
    }

    /// <summary>
    ///     Returns the edge type in order to triangulate a corner
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(_elevation, otherCell.Elevation);
    }
}
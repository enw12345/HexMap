using TMPro;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private TMP_Text _cellLabelPrefab;

    [SerializeField] private HexCell _hexCellPrefab;

    [SerializeField] private Texture2D _noiseSource;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private int _height = 6;
    [SerializeField] private int _width = 6;


    private Canvas _gridCanvas;
    private HexCell[] _hexCells;
    private HexMesh _hexMesh;

    private void Awake()
    {
        _hexCells = new HexCell[_width * _height];
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();
        HexMetrics.NoiseSource = _noiseSource;

        for (int z = 0, i = 0; z < _height; z++)
        for (var x = 0; x < _width; x++)
            CreateCell(x, z, i++);
    }

    private void Start()
    {
        _hexMesh.Triangulate(_hexCells);
        HexMetrics.NoiseSource = _noiseSource;
    }

    private void OnEnable()
    {
        HexMetrics.NoiseSource = _noiseSource;
    }

    // public HexCell GetCell(Vector3 position, Color color)
    // {
    //     position = transform.InverseTransformPoint(position);
    //     var coordinates = HexCoordinates.FromPosition(position);
    //     // Debug.Log($"Touched at position {coordinates.ToString()}");
    //
    //     var index = coordinates.X + coordinates.Z * _width + coordinates.Z / 2;
    //     var cell = _hexCells[index];
    //     cell.Color = color;
    //     _hexMesh.Triangulate(_hexCells);
    // }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        // Debug.Log($"Touched at position {coordinates.ToString()}");

        var index = coordinates.X + coordinates.Z * _width + coordinates.Z / 2;
        return _hexCells[index];
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;

        //the distance between adjacent hexagon cells in the X direction is equal to twice the inner radius
        //the distance to the next row of cells should be 1.5 times the outer radius
        //The hexagon rows are offset by the along the x axis by the inner radius, hexagonal spacing
        position.x = (x + z * 0.5f - z / 2) * HexMetrics.InnerRadius * 2f;
        position.y = 0f;
        position.z = z * HexMetrics.OuterRadius * 1.5f;

        var hexCell = _hexCells[i] = Instantiate(_hexCellPrefab);
        hexCell.transform.SetParent(transform, false);
        hexCell.transform.localPosition = position;
        hexCell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        var label = Instantiate(_cellLabelPrefab, _gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);

        hexCell.Color = _defaultColor;

        //initialize the neighbor relationship.
        //from left to right find cells to connect to
        if (x > 0) hexCell.SetNeighbor(HexDirection.W, _hexCells[i - 1]);
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                hexCell.SetNeighbor(HexDirection.SE, _hexCells[i - _width]);
                if (x > 0) hexCell.SetNeighbor(HexDirection.SW, _hexCells[i - _width - 1]);
            }
            else
            {
                hexCell.SetNeighbor(HexDirection.SW, _hexCells[i - _width]);
                if (x < _width - 1) hexCell.SetNeighbor(HexDirection.SE, _hexCells[i - _width + 1]);
            }
        }

        label.text = hexCell.Coordinates.ToString();
        hexCell.UIRect = label.rectTransform;
    }

    public void Refresh()
    {
        _hexMesh.Triangulate(_hexCells);
    }
}
using TMPro;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private int _width = 6;
    [SerializeField] private int _height = 6;

    [SerializeField] private HexCell _hexCellPrefab;
    [SerializeField] private TMP_Text _cellLabelPrefab;

    private Canvas _gridCanvas;
    private HexCell[] _hexCells;
    private HexMesh _hexMesh;

    private void Awake()
    {
        _hexCells = new HexCell[_width * _height];
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();

        for (int z = 0, i = 0; z < _height; z++)
        for (var x = 0; x < _width; x++)
            CreateCell(x, z, i++);
    }

    private void Start()
    {
        _hexMesh.Triangulate(_hexCells);
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;

        //the distance between adjacent hexagon cells in the X direction is equal to twice the inner radius
        //the distance to the next row of cells should be 1.5 times the outer radius
        //The hexagon rows are offset by the along the x axis by the inner radius, hexagonal spacing
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        var hexCell = _hexCells[i] = Instantiate(_hexCellPrefab);
        hexCell.transform.SetParent(transform, false);
        hexCell.transform.localPosition = position;
        hexCell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        var label = Instantiate(_cellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);

        label.text = hexCell.Coordinates.ToString();
    }
}
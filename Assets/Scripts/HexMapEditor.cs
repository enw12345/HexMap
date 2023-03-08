using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    [SerializeField] private HexGrid _hexGrid;

    private Color _activeColor;
    private int _activeElevation;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) HandleInput();
    }

    private void HandleInput()
    {
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) EditCell(_hexGrid.GetCell(hit.point));
    }

    public void SelectColor(int index)
    {
        _activeColor = _colors[index];
    }

    public void SetElevation(float elevation)
    {
        _activeElevation = (int) elevation;
    }

    /// <summary>
    ///     Takes care of all the editing of a cell and refreshes the grid
    /// </summary>
    /// <param name="cell"></param>
    private void EditCell(HexCell cell)
    {
        cell.Color = _activeColor;
        cell.Elevation = _activeElevation;
        _hexGrid.Refresh();
    }
}
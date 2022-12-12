using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh _hexMesh;
    private List<int> _triangles;
    private List<Vector3> _vertices;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();
        _hexMesh.name = "Hex Mesh";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells)
    {
        ClearMeshData();

        for (var i = 0; i < cells.Length; i++) Triangulate(cells[i]);

        _hexMesh.vertices = _vertices.ToArray();
        _hexMesh.triangles = _triangles.ToArray();
        _hexMesh.RecalculateNormals();
    }

    private void Triangulate(HexCell cell)
    {
        var center = cell.transform.localPosition;
        for (var i = 0; i < 6; i++)
            AddTriangle(
                center,
                center + HexMetrics.Corners[i],
                center + HexMetrics.Corners[i + 1]
            );
    }

    private void ClearMeshData()
    {
        _hexMesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }
}
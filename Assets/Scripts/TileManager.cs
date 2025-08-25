using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<GameObject> tilePrefabs;

    [SerializeField] private Grid grid;
    [SerializeField] private Vector2Int size = new Vector2Int(23, 15);
    [SerializeField] private int objectsPerColor = 20;

    private List<Vector3Int> cells;

    void OnEnable()
    {
        RemoveTiles();
        Init();
        GenerateTiles();
    }

    void Init()
    {
        cells = new List<Vector3Int>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                cells.Add(new Vector3Int(x, y, -1));
            }
        }
    }

    void GenerateTiles()
    {
        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            GameObject tile = tilePrefabs[i].gameObject;
            for (int j = 0; j < objectsPerColor; j++)
            {
                if (cells.Count == 0)
                {
                    return;
                }

                int index = Random.Range(0, cells.Count);
                Vector3 pos = grid.CellToWorld(cells[index]);

                GameObject newTile = Instantiate(tile, pos, Quaternion.identity, transform);
                newTile.name = $"{tile.name} ({cells[index].x},{cells[index].y})";
                cells.RemoveAt(index);
            }
        }
    }

    void RemoveTiles()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
    }
}
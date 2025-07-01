using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Tiles tilePrefabs;
    private List<Vector3Int> cells;


    [SerializeField] private Vector2Int size = new Vector2Int(23, 15);
    [SerializeField] private int objectsPerColor = 20;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        cells = new List<Vector3Int>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                cells.Add(new Vector3Int(x, y, 0));
            }
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject tile = tilePrefabs.color[i].gameObject;
            for (int j = 0; j < objectsPerColor; j++)
            {
                if (cells.Count == 0)
                {
                    Debug.Log("그리드 셀 부족");
                    return;
                }

                int index = Random.Range(0, cells.Count);
                Vector3Int randomCell = cells[index];
                cells.RemoveAt(index);

                Vector3 pos = grid.CellToWorld(randomCell);
                
                pos.z = -1;
                

                GameObject newTile = Instantiate(tile, pos, Quaternion.identity, transform);
                newTile.name = $"Tile_{i}_({randomCell.x},{randomCell.y})";
            }
        }
    }
}

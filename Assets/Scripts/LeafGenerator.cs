using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeafGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] leafPrefabs;
    public int numberOfLeaves;

    private List<Vector3Int> spawnedLeafPositions = new List<Vector3Int>();

    void Start()
    {
        GenerateLeaves();
    }

    void GenerateLeaves()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int i = 0; i < numberOfLeaves; i++)
        {
            Vector3Int randomCell = GetRandomInnerCell(bounds);

            // Check if the cell is already occupied
            while (spawnedLeafPositions.Contains(randomCell))
            {
                randomCell = GetRandomInnerCell(bounds);
            }

            spawnedLeafPositions.Add(randomCell);

            Vector3 randomPosition = tilemap.GetCellCenterWorld(randomCell);

            int randomPrefabIndex = Random.Range(0, leafPrefabs.Length);
            GameObject selectedLeafPrefab = leafPrefabs[randomPrefabIndex];

            Instantiate(selectedLeafPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3Int GetRandomInnerCell(BoundsInt bounds)
    {
        int x = Random.Range(bounds.x + 1, bounds.x + bounds.size.x - 1);
        int y = Random.Range(bounds.y + 1, bounds.y + bounds.size.y - 1);

        return new Vector3Int(x, y, bounds.z);
    }
}

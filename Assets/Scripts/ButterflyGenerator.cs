using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButterflyGenerator : MonoBehaviour
{
    public GameObject[] butterflyPrefabs; // Array of butterfly prefabs
    public Tilemap tilemap;
    public int numberOfButterflies;

    void Start()
    {
        GenerateButterflies();
    }

    void GenerateButterflies()
    {
        for (int i = 0; i < numberOfButterflies; i++)
        {
            Vector3Int randomPosition = new Vector3Int(
                Random.Range(tilemap.cellBounds.x, tilemap.cellBounds.x + tilemap.cellBounds.size.x),
                Random.Range(tilemap.cellBounds.y, tilemap.cellBounds.y + tilemap.cellBounds.size.y),
                0
            );

            Vector3 spawnPosition = tilemap.GetCellCenterWorld(randomPosition);

            // Randomly choose a butterfly prefab from the array
            GameObject selectedButterflyPrefab = butterflyPrefabs[Random.Range(0, butterflyPrefabs.Length)];

            // Instantiate a butterfly at the random position
            GameObject butterfly = Instantiate(selectedButterflyPrefab, spawnPosition, Quaternion.identity);

            butterfly.SetActive(true);
        }
    }
}

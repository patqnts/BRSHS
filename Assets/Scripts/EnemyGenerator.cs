using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Tilemap tilemap;
    public int maxEnemyCount = 5;

    private int currentEnemyCount = 0;
    public static EnemyGenerator instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Adjust the respawn delay as needed

            // Check if the current enemy count is less than the maximum allowed
            if (currentEnemyCount < maxEnemyCount)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3Int randomTilePosition = GetRandomTilePosition();
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(randomTilePosition);

        // Instantiate the enemy prefab at the random tile position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Increment the current enemy count
        currentEnemyCount++;
    }

    Vector3Int GetRandomTilePosition()
    {
        // Get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Generate a random position within the bounds
        int randomX = Random.Range(bounds.x, bounds.x + bounds.size.x);
        int randomY = Random.Range(bounds.y, bounds.y + bounds.size.y);

        return new Vector3Int(randomX, randomY, 0);
    }

    // Call this method when an enemy dies to decrement the current enemy count
    public void EnemyDied()
    {
        currentEnemyCount--;
    }
}

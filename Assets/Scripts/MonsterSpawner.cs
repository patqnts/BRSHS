using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int maxMonsters = 5;
    [SerializeField] private float spawnRadius = 5f;

    private List<MonsterScript> aliveMonsters = new List<MonsterScript>();

    void Start()
    {
        // Initial spawn up to limit
        for (int i = 0; i < maxMonsters; i++)
        {
            SpawnMonster();
        }
    }

    void Update()
    {
        // Continuously ensure we have up to max
        if (aliveMonsters.Count < maxMonsters)
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        GameObject monsterGO = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        if (monsterGO.TryGetComponent<MonsterScript>(out var monster))
        {
            // Subscribe to death event
            monster.OnMonsterDied += HandleMonsterDeath;
            aliveMonsters.Add(monster);
        }
        else
        {
            Debug.LogError("Spawned prefab does not have a MonsterScript!");
        }
    }

    private void HandleMonsterDeath(MonsterScript monster)
    {
        if (aliveMonsters.Contains(monster))
        {
            aliveMonsters.Remove(monster);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize spawn area
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

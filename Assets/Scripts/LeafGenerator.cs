using cherrydev;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LeafGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] leafPrefabs;
    public int numberOfLeaves;

    private List<Vector3Int> spawnedLeafPositions = new List<Vector3Int>();

    public float gameDuration = 60f; // Set the game duration in seconds
    private float timer;
    public Text timerText;

    [SerializeField] private DialogBehaviour startDialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;

    [SerializeField] private DialogBehaviour finishBehaviour;
    [SerializeField] private DialogNodeGraph winGraph;
    [SerializeField] private DialogNodeGraph loseGraph;

    public bool isStart;
    private bool hasGameEnded = false;

    public RewardUI rewardUI;

    public UserSessionScript user;
    void Start()
    {
        user = FindObjectOfType<UserSessionScript>();
        startDialogBehaviour.StartDialog(dialogGraph);       
    }

    private void Update()
    {
        if (isStart && !hasGameEnded)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        timer = gameDuration;
        isStart = true;
        GenerateLeaves();
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        // Display the timer in the UI text
        timerText.text = $"Time: {Mathf.Ceil(timer)}";
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

    void EndGame()
    {
        hasGameEnded = true;
        LoseGame();
    }

    public void WinGame()
    {
        Debug.Log("You won!");
        // Add your winning logic here
        isStart = false;
        rewardUI.isWin = true;
        user.coins += 10;
        user.clearPick = true;
        user.SavePlayerData();
        finishBehaviour.StartDialog(winGraph);
    }

    void LoseGame()
    {
        rewardUI.isWin = false;
        if(user.currentHealth > 0)
        {
            user.currentHealth--;
        }
        user.SavePlayerData();
        finishBehaviour.StartDialog(loseGraph);
    }
}

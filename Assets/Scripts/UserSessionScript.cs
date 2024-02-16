using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class UserSessionScript : MonoBehaviour
{
    private float sessionStartTime;
    private bool notificationSent = false;

    [Header("Player Data")]
    public int maxHealth;
    public int currentHealth;
    public int coins;
    public Vector2 playerPos;
    public bool clearBroom;
    public bool clearSeg;
    public bool clearPick;

    PlayerData playerData;

    public string[] GetSavedPlayerDataFiles()
    {
        return Directory.GetFiles(Application.persistentDataPath);
    }
    private void Awake()
    {
        sessionStartTime = Time.time;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        LoadPlayerData(playerData);
    }

    private void Update()
    {
        float elapsedTime = Time.time - sessionStartTime;

        if (elapsedTime >= 3600 && !notificationSent)
        {
            NotifyGame("Session time is 1 hour!");
            notificationSent = true;
        }
    }

    void NotifyGame(string message)
    {
        Debug.Log(message);
    }

    // Save player data to JSON
    public void SavePlayerData()
    {
        PlayerData currentPlayerData = new PlayerData()
        {
            MaxHealth = maxHealth,
            CurrentHealth = currentHealth,
            PlayerPos = playerPos,
            Coins = coins,
            ClearBroom = clearBroom,
            ClearPick = clearPick,
            ClearSeg = clearSeg,
        };
        string jsonData = JsonUtility.ToJson(currentPlayerData);
        File.WriteAllText(GetSavePath(), jsonData);
    }

    // Load player data from JSON
    public void LoadPlayerData(PlayerData playerData)
    {

        string filePath = GetSavePath();

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            PopulatePlayerData(playerData);
        }
        else
        {
            // If the save file doesn't exist, create a new PlayerData instance
            playerData = new PlayerData();
            PopulatePlayerData(playerData);
        }
    }
    void PopulatePlayerData(PlayerData playerData)
    {
        maxHealth = playerData.MaxHealth;
        currentHealth = playerData.CurrentHealth;
        coins = playerData.Coins;
        playerPos = playerData.PlayerPos;
        clearBroom = playerData.ClearBroom;
        clearSeg = playerData.ClearSeg;
        clearPick = playerData.ClearPick;
    }

    public void LoadPlayerDataFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            string jsonData = File.ReadAllText(fileName);
            PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);

            PopulatePlayerData(loadedPlayerData);
        }
        else
        {
            Debug.Log($"File not found: {fileName}");
        }

        SceneManager.LoadScene("MainGame");
    }
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "playerData.json");
    }
}

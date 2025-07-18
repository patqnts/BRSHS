using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserSessionScript : MonoBehaviour
{
    public GameObject NotifyUI;

    private float sessionStartTime;
    private bool notificationSent = false;

    [Header("Player Data")]
    public int maxHealth;
    public int currentHealth;
    public int coins;
    public int segregateHighscore;
    public Vector2 playerPos;
    public bool clearBroom;
    public bool clearSeg;
    public bool clearPick;
    public bool clearFan;
    public bool clearPlant;
    public string selectedString;

    public bool isNewGame;

    PlayerData playerData;

    private static UserSessionScript instance;

    public static UserSessionScript Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find the existing instance in the scene
                instance = FindObjectOfType<UserSessionScript>();

                // If no instance exists, create a new one
                if (instance == null)
                {
                    GameObject obj = new GameObject("UserSessionScript");
                    instance = obj.AddComponent<UserSessionScript>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    public string[] GetSavedPlayerDataFiles()
    {
        return Directory.GetFiles(Application.persistentDataPath);
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        sessionStartTime = Time.time;
        DontDestroyOnLoad(gameObject);
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
        NotifyUI.SetActive(true);
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
            SegregateHighScore = segregateHighscore,
            ClearBroom = clearBroom,
            ClearPick = clearPick,
            ClearFan = clearFan,
            ClearSeg = clearSeg,
            ClearPlant = clearPlant
        };
        string jsonData = JsonUtility.ToJson(currentPlayerData);
        File.WriteAllText(selectedString, jsonData);

        MMEventManager.TriggerEvent(new MMGameEvent("Save"));
        Debug.Log("Invetory saved " + Inventory._saveFolderName);

    }

    public void NewPlayerData()
    {
        PlayerData currentPlayerData = new PlayerData()
        {
            MaxHealth = 3,
            CurrentHealth = 3,
            PlayerPos = new Vector2(11.48f, -7.5754f),
            Coins = 0,
            SegregateHighScore = 0,
            ClearBroom = clearBroom,
            ClearPick = clearPick,
            ClearSeg = clearSeg,
            ClearFan = clearFan,
            ClearPlant = clearPlant
        };
        string jsonData = JsonUtility.ToJson(currentPlayerData);
        File.WriteAllText(selectedString, jsonData);
        PopulatePlayerData(currentPlayerData);
    }
    void PopulatePlayerData(PlayerData playerData)
    {
        maxHealth = playerData.MaxHealth;
        currentHealth = playerData.CurrentHealth;
        coins = playerData.Coins;
        segregateHighscore = playerData.SegregateHighScore;
        playerPos = playerData.PlayerPos;
        clearBroom = playerData.ClearBroom;
        clearSeg = playerData.ClearSeg;
        clearPick = playerData.ClearPick;
        clearFan = playerData.ClearFan;
        clearPlant = playerData.ClearPlant;
    }

    public void LoadPlayerDataFromFile(string fileName)
    {
        selectedString = fileName;
        if (File.Exists(fileName))
        {
            MMInventoryLoad(fileName);

            string jsonData = File.ReadAllText(fileName);
            PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);

            PopulatePlayerData(loadedPlayerData);
            isNewGame = false;
        }
        else
        {
            Debug.Log($"File not found: {fileName}");
            isNewGame = true;
            NewPlayerData();
            MMInventorySave(fileName);
        }       
        SceneManager.LoadScene("MainGame");
    }

    private void MMInventoryLoad(string fileName)
    {
        // Extract folder from the full path of the file
        string folderName = Path.GetFileNameWithoutExtension(fileName);
        Inventory._saveFolderName = folderName;
        MMEventManager.TriggerEvent(new MMGameEvent("Load"));
        Debug.Log("Invetory loaded "+ Inventory._saveFolderName);
    }

    private void MMInventorySave(string fileName)
    {
        // Extract folder from the full path of the file
        string folderName = Path.GetFileNameWithoutExtension(fileName);
        Inventory._saveFolderName = folderName;
        MMEventManager.TriggerEvent(new MMGameEvent("Save"));
        Debug.Log("Invetory saved " + Inventory._saveFolderName);

    }


    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "playerData.json");
    }
}

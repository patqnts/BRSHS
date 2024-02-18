using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuData : MonoBehaviour
{
    public UserSessionScript userSession;
    public GameObject saveSlotPrefab; // Prefab for the save slot
    public Transform parent;
    public GridAutoAdjust grid;
    public string selectedSlot;

    void Start()
    {
        grid = FindObjectOfType<GridAutoAdjust>();
        userSession = FindObjectOfType<UserSessionScript>();
        DisplaySavedPlayerDataFiles();
    }

    // Display saved player data files using save slots
    void DisplaySavedPlayerDataFiles()
    {
        string[] savedPlayerDataFiles = userSession.GetSavedPlayerDataFiles();

        foreach (string fileName in savedPlayerDataFiles)
        {
            // Instantiate save slot prefab
            GameObject saveSlot = Instantiate(saveSlotPrefab, parent);

            string name = Path.GetFileNameWithoutExtension(fileName);
            // Get the Text component of the instantiated save slot
            Text saveSlotText = saveSlot.GetComponentInChildren<Text>();

            saveSlotText.text = name;

            // Add a button or click event to load the selected player data
            Button saveSlotButton = saveSlot.GetComponent<Button>();
            saveSlotButton.onClick.AddListener(() => LoadSelectedPlayerData(fileName));

            saveSlot.SetActive(true);
        }
        grid.AdjustGridLayout();
    }

    // Load a specific player data file based on user selection
    public void LoadSelectedPlayerData(string selectedFileName)
    {
        userSession.LoadPlayerDataFromFile(selectedFileName);
    }

    public void StartNewGame()
    {
        string newFileName = GenerateUniqueFileName();
        LoadSelectedPlayerData(newFileName);
    }

    // Generate a unique file name based on timestamp
    private string GenerateUniqueFileName()
    {
        int saveNumber = GetNextSaveNumber();
        string newFileName = $"PlayerSave_{saveNumber}.json";
        return Path.Combine(Application.persistentDataPath, newFileName);
    }

    private int GetNextSaveNumber()
    {
        string[] savedFiles = userSession.GetSavedPlayerDataFiles();
        int maxSaveNumber = 0;

        foreach (string fileName in savedFiles)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            if (name.StartsWith("PlayerSave_"))
            {
                // Extract the number part of the file name and find the maximum
                if (int.TryParse(name.Substring("PlayerSave_".Length), out int number))
                {
                    maxSaveNumber = Mathf.Max(maxSaveNumber, number);
                }
            }
        }

        // Increment the maximum save number to get the next available number
        return maxSaveNumber + 1;
    }
}

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
    public Transform canvasTransform;
    public GridAutoAdjust grid;
    public string selectedSlot;

    private List<GameObject> saveSlots = new List<GameObject>();

    public GameObject renameUIPrefab;

    private GameObject currentRenameUI;

    void Start()
    {
        grid = FindObjectOfType<GridAutoAdjust>();
        userSession = FindObjectOfType<UserSessionScript>();
        DisplaySavedPlayerDataFiles();
    }

    // Display saved player data files using save slots
    void DisplaySavedPlayerDataFiles()
    {
        // Clear existing save slots
        foreach (var saveSlot in saveSlots)
        {
            Destroy(saveSlot);
        }
        saveSlots.Clear();

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

            // Add a delete button and its click event
            Button deleteButton = saveSlot.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteSaveFile(fileName, saveSlot));


            Button renameButton = saveSlot.transform.Find("RenameButton").GetComponent<Button>();
            renameButton.onClick.AddListener(() => OpenRenameUI(fileName));

            saveSlot.SetActive(true);
            saveSlots.Add(saveSlot);
        }
        grid.AdjustGridLayout();
    }

    // Load a specific player data file based on user selection
    public void LoadSelectedPlayerData(string selectedFileName)
    {
        userSession.LoadPlayerDataFromFile(selectedFileName);
    }

    // Delete the selected save file and corresponding prefab
    private void DeleteSaveFile(string fileName, GameObject saveSlot)
    {
        File.Delete(fileName);
        // Remove the corresponding save slot prefab
        saveSlots.Remove(saveSlot);
        Destroy(saveSlot);

        // Optionally, update the UI to reflect the changes after deletion
        grid.AdjustGridLayout();
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
 

    private void OpenRenameUI(string oldFileName)
    {
        // Instantiate the rename UI prefab
        currentRenameUI = Instantiate(renameUIPrefab, canvasTransform);

        // Get the InputField and ConfirmButton components
        InputField inputField = currentRenameUI.GetComponentInChildren<InputField>();
        Button confirmButton = currentRenameUI.transform.Find("ConfirmButton").GetComponent<Button>();

        // Add a listener to the ConfirmButton for renaming
        confirmButton.onClick.AddListener(() => ConfirmRename(oldFileName, inputField));
        currentRenameUI.SetActive(true);

    }

    // Method to confirm the renaming process
    private void ConfirmRename(string oldFileName, InputField inputField)
    {
        string newName = inputField.text;

        // Validate the new name (you can add your own validation logic)

        // Rename the file
        string directory = Path.GetDirectoryName(oldFileName);
        string newFileName = Path.Combine(directory, newName + ".json");
        File.Move(oldFileName, newFileName);

        // Destroy the rename UI and refresh the display
        Destroy(currentRenameUI);
        DisplaySavedPlayerDataFiles();
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}

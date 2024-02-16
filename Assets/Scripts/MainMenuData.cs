using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuData : MonoBehaviour
{
    public UserSessionScript userSession;
    public GameObject saveSlotPrefab; // Prefab for the save slot
    public Transform parent;
    public string selectedSlot;

    void Start()
    {
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

            // Get the Text component of the instantiated save slot
            Text saveSlotText = saveSlot.GetComponentInChildren<Text>();

            saveSlotText.text = fileName;

            // Add a button or click event to load the selected player data
            Button saveSlotButton = saveSlot.GetComponent<Button>();
            saveSlotButton.onClick.AddListener(() => LoadSelectedPlayerData(fileName));

            saveSlot.SetActive(true);
        }
    }

    // Load a specific player data file based on user selection
    public void LoadSelectedPlayerData(string selectedFileName)
    {
        userSession.LoadPlayerDataFromFile(selectedFileName);
    }
}

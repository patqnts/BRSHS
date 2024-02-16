using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuData : MonoBehaviour
{
    public UserSessionScript userSession;

    void Start()
    {
        userSession = FindObjectOfType<UserSessionScript>();
        // Access the UserSessionScript and call the method to get saved player data files
        string[] savedPlayerDataFiles = userSession.GetSavedPlayerDataFiles();

        // Display the list of saved player data files (you can customize this part)
        foreach (string fileName in savedPlayerDataFiles)
        {
            Debug.Log($"Saved Player Data File: {fileName}");
        }
    }

    // Add a method to load a specific player data file based on user selection
    public void LoadSelectedPlayerData(string selectedFileName)
    {
        userSession.LoadPlayerDataFromFile(selectedFileName);
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public UserSessionScript userSessionScript;
    public PlayerScript playerScript;
    public PlayerController playerController;
    void Start()
    {
        userSessionScript = FindObjectOfType<UserSessionScript>();
        playerScript = FindObjectOfType<PlayerScript>();
        playerController = FindObjectOfType<PlayerController>();


        LoadPlayerData();
    }

    public void Save()
    {
        userSessionScript.playerPos = playerController.transform.position;
        userSessionScript.maxHealth = playerScript.MaxHealth;
        userSessionScript.currentHealth = playerScript.CurrentHealth;
        userSessionScript.clearPick = playerScript.ClearPick;
        userSessionScript.clearBroom = playerScript.ClearBroom;
        userSessionScript.clearSeg = playerScript.ClearSeg;
        userSessionScript.coins = playerScript.Coins;

        userSessionScript.SavePlayerData();
    }

    void LoadPlayerData()
    {
        playerController.transform.position = userSessionScript.playerPos;

        playerScript.MaxHealth = userSessionScript.maxHealth;
        playerScript.CurrentHealth = userSessionScript.currentHealth;
        playerScript.ClearPick = userSessionScript.clearPick;
        playerScript.ClearBroom = userSessionScript.clearBroom;
        playerScript.ClearSeg = userSessionScript.clearSeg;
        playerScript.Coins = userSessionScript.coins;
    }
}

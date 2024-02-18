using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public UserSessionScript userSessionScript;
    public PlayerScript playerScript;
    public PlayerController playerController;

    public GameObject intro;
    void Start()
    {
        userSessionScript = FindObjectOfType<UserSessionScript>();
        playerScript = FindObjectOfType<PlayerScript>();
        playerController = FindObjectOfType<PlayerController>();


        LoadPlayerData();
    }
    public void BuyHealth()
    {
        if (playerScript.CurrentHealth >= playerScript.MaxHealth)
        {
            //fullhealth already
            return;
        }

        if (playerScript.Coins < 20)
        {
            //not enough coins
            return;
        }

        if (playerScript.Coins > 20)
        {
            playerScript.Coins -= 20;
            playerScript.CurrentHealth++;
            Save();
        }
    }
    public void GoToMenu()
    {
        Save();

        userSessionScript.playerPos = new Vector2(0,0);
        userSessionScript.maxHealth = 3;
        userSessionScript.currentHealth = 3;
        userSessionScript.clearPick = false;
        userSessionScript.clearBroom = false;
        userSessionScript.clearSeg = false;
        userSessionScript.coins = 0;

        SceneManager.LoadScene("MainMenu");
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

        if (userSessionScript.isNewGame)
        {
            intro.SetActive(true);
        }
    }
}

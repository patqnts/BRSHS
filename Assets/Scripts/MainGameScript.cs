using cherrydev;
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
    public PlantingScript plantingScript;

    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph[] dialogGraph;

    public GameObject intro;
    public GameObject flourish;

    void Start()
    {
        userSessionScript = FindObjectOfType<UserSessionScript>();
        playerScript = FindObjectOfType<PlayerScript>();
        playerController = FindObjectOfType<PlayerController>();
        plantingScript = FindObjectOfType<PlantingScript>();


        LoadPlayerData();
    }
    public void BuyHealth()
    {
        if (playerScript.Coins < 20)
        {
            //not enough coins
            dialogBehaviour.StartDialog(dialogGraph[1]);
            return;
        }

        if (playerScript.CurrentHealth >= playerScript.MaxHealth)
        {
            dialogBehaviour.StartDialog(dialogGraph[0]);
            return;
        }

        if (playerScript.Coins >= 30)
        {
            playerScript.Coins -= 30;
            playerScript.CurrentHealth++;
            Save();
            LoadPlayerData();
            playerScript.LoadData();
        }
    }

    public bool PlantTree()
    {
        if (playerScript.Coins < 20)
        {
            //not enough coins
            dialogBehaviour.StartDialog(dialogGraph[1]);
            return false;
        }

        if (playerScript.Coins >= 20)
        {
            playerScript.Coins -= 20;
            Save();
            LoadPlayerData();
            playerScript.LoadData();
            return true;
        }

        return false;
    }
    public void GoToMenu()
    {
        if (!playerScript.ClearPlant)
        {
            for (int i = 0; i < plantingScript.plants.Length; i++)
            {
                if (plantingScript.plants[i].isPlanted)
                {
                    playerScript.Coins += 20;
                }
            }
        }
        Save();

        userSessionScript.playerPos = new Vector2(0,0);
        userSessionScript.maxHealth = 3;
        userSessionScript.currentHealth = 3;
        userSessionScript.clearPick = false;
        userSessionScript.clearBroom = false;
        userSessionScript.clearSeg = false;
        userSessionScript.clearFan = false;
        userSessionScript.clearPlant = false;
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
        userSessionScript.clearFan = playerScript.ClearFan;
        userSessionScript.clearPlant = playerScript.ClearPlant;

        

        userSessionScript.coins = playerScript.Coins;

        userSessionScript.SavePlayerData();
    }
    public void LoadPlayerData()
    {
        playerController.transform.position = userSessionScript.playerPos;

        playerScript.MaxHealth = userSessionScript.maxHealth;
        playerScript.CurrentHealth = userSessionScript.currentHealth;
        playerScript.ClearPick = userSessionScript.clearPick;
        playerScript.ClearBroom = userSessionScript.clearBroom;
        playerScript.ClearSeg = userSessionScript.clearSeg;
        playerScript.ClearFan = userSessionScript.clearFan;
        playerScript.ClearPlant = userSessionScript.clearPlant;
        playerScript.Coins = userSessionScript.coins;

        if(playerScript.ClearPick &&
           playerScript.ClearBroom &&
           playerScript.ClearSeg &&
           playerScript.ClearPlant &&
           playerScript.ClearFan)
        {
            flourish.SetActive(true);
        }

        if (userSessionScript.isNewGame)
        {
            intro.SetActive(true);
            userSessionScript.isNewGame = false;
        }
    }
}

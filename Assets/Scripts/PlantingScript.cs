using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerScript player;
    public MainGameScript mainGameScript;

    public PlantScript[] plants;
    public int counter;

    public GameObject grow;

    public bool isDone = false;
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        mainGameScript = FindObjectOfType<MainGameScript>();
        CheckPlant();
    }

    public void CheckPlant()
    {
       
        if(!isDone)
        {
            if (counter >= plants.Length)
            {
                player.ClearPlant = true;
                mainGameScript.Save();
                mainGameScript.LoadPlayerData();
            }

            if (player.ClearPlant)
            {
                grow.SetActive(true);
                foreach (var plant in plants)
                {
                    plant.treeObject.SetActive(true);
                }
                isDone = true;
            }
        }      
    }
    public void GrowPlants()
    {
        player.ClearPlant = true;
        mainGameScript.Save();
        mainGameScript.LoadPlayerData();
        CheckPlant();
    }
}

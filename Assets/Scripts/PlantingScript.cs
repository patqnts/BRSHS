using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerScript player;
    public MainGameScript mainGameScript;

    public GameObject grow;
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        mainGameScript = FindObjectOfType<MainGameScript>();
        CheckPlant();
    }

    void CheckPlant()
    {
        if (player.ClearFan)
        {
            grow.SetActive(true);
        }
    }
    public void GrowPlants()
    {
        player.ClearFan = true;
        mainGameScript.Save();
        CheckPlant();
    }
}

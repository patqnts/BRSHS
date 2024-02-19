using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPlanted;
    public GameObject treeObject;
    public PlantingScript plantScript;
    public MainGameScript playerScript;

    private void Start()
    {
        plantScript = FindObjectOfType<PlantingScript>();
        playerScript = FindObjectOfType<MainGameScript>();
    }
    public void PlantTree()
    {
        if(!isPlanted)
        {
            if (playerScript.PlantTree())
            {
                isPlanted = true;
                treeObject.SetActive(true);
                plantScript.counter++;
                plantScript.CheckPlant();
            }
           
        }       
    }
}

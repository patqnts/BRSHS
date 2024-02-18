using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafDestroyer : MonoBehaviour
{
    LeafGenerator leafGenerator;
    public AudioSource audio;
    private void Start()
    {
        leafGenerator = FindObjectOfType<LeafGenerator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Leaf"))
        {
            if(leafGenerator.numberOfLeaves>0)
            {
                audio.Play();
                leafGenerator.numberOfLeaves--;
                Destroy(other.gameObject);
            }

            
           
        }

        if (leafGenerator.numberOfLeaves <= 0)
        {
            leafGenerator.WinGame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafDestroyer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Leaf"))
        {
            Destroy(other.gameObject);
        }
    }
}

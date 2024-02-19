using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaScript : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph[] dialogGraph;

    [SerializeField] public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            dialogBehaviour.StartDialog(dialogGraph[Random.Range(0,dialogGraph.Length)]);
        }
    }
}

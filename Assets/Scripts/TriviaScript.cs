using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaScript : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph[] dialogGraph;

    [SerializeField] public AudioSource audioSource;

    public GameObject questionMark;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(questionMark != null)
        {
            questionMark.SetActive(true);
        }

        if (collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            dialogBehaviour.StartDialog(dialogGraph[Random.Range(0,dialogGraph.Length)]);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (questionMark != null)
        {
            questionMark.SetActive(false);
        }
    }
}

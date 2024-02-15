using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TrashcanScript : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Garbage")
        {
            score++;
            Destroy(collision.gameObject);
        }

        if (score >= 8)
        {
            dialogBehaviour.StartDialog(dialogGraph);
        }
    }
    //this is a test commit/update
}

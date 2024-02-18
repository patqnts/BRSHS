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

    public AudioSource audioSource;

    public UserSessionScript user;
    private void Start()
    {
        user = FindObjectOfType<UserSessionScript>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Garbage")
        {
            audioSource.Play();
            score++;
            Destroy(collision.gameObject);
        }

        if (score >= 12)
        {
            user.coins += 10;
            user.clearBroom = true;
            user.SavePlayerData(); //save
            dialogBehaviour.StartDialog(dialogGraph);
        }
    }
    //this is a test commit/update
}

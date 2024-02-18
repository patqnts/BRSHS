using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    [SerializeField]
    private string garbageType;
    [SerializeField] SegregateGame game;

    public AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == garbageType)
        {
            audioSource.Play();
            Debug.Log("Correct");
            game.AddScore();
            animator.SetTrigger("Open");
            Destroy(collision.gameObject);
        }
        else
        {
            game.MinusScore();
            Debug.Log("Wrong");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    [SerializeField]
    private string garbageType;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == garbageType)
        {
            Debug.Log("Correct");
            animator.SetTrigger("Open");
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("Wrong");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegregateGame : MonoBehaviour
{
    [SerializeField]
    private int highScore;
    [SerializeField]
    private int currentScore;
    public GameObject[] garbagePrefabs;
    private GameObject currentGarbage;
    private bool canMove = true;
    private bool movingRight = true;
    public float speed = 5f;
    public Transform pointA;
    public Transform pointB;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateRandomGarbage();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MoveGarbage();
        }
    }

    void MoveGarbage()
    {
        if (currentGarbage != null)
        {
            // Implement your horizontal movement logic here
            float step = speed * Time.deltaTime;

            if (movingRight)
            {
                currentGarbage.transform.position = Vector3.MoveTowards(currentGarbage.transform.position, pointB.position, step);
            }
            else
            {
                currentGarbage.transform.position = Vector3.MoveTowards(currentGarbage.transform.position, pointA.position, step);
            }

            // Check if the garbage has reached the destination, then change direction
            if (currentGarbage.transform.position == pointB.position)
            {
                movingRight = false;
            }
            else if (currentGarbage.transform.position == pointA.position)
            {
                movingRight = true;
            }

            // Check if a button is pressed to stop movement and drop the garbage
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canMove = false;
                DropGarbage();
            }
        }
    }

    void DropGarbage()
    {
        if (currentGarbage != null)
        {
            Rigidbody2D rb = currentGarbage.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f; // Enable gravity for the garbage
            }

            // Add any additional logic for dropping the garbage

            // Destroy the current garbage after a certain time or condition
            Destroy(currentGarbage, 2f);

            // Instantiate a new random garbage
            InstantiateRandomGarbage();
        }
    }

    void InstantiateRandomGarbage()
    {
        canMove = true; // Enable movement for the new garbage
        int randomIndex = Random.Range(0, garbagePrefabs.Length);
        currentGarbage = Instantiate(garbagePrefabs[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegregateGame : MonoBehaviour
{
    [SerializeField]
    private Text garbageName;
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
    public Text scoreText;
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

    public void AddScore()
    {
        currentScore++;
        scoreText.text = $"Score: {currentScore}";
    }

    public void MinusScore()
    {
        if (currentScore > 0)
        {
            currentScore--;
            scoreText.text = $"Score: {currentScore}";
        }
    }

    void MoveGarbage()
    {
        if (currentGarbage != null)
        {
            garbageName.text = currentGarbage.name.Replace("(Clone)", "");
            float step = speed * Time.deltaTime;

            if (movingRight)
            {
                currentGarbage.transform.position = Vector3.MoveTowards(currentGarbage.transform.position, pointB.position, step);
            }
            else
            {
                currentGarbage.transform.position = Vector3.MoveTowards(currentGarbage.transform.position, pointA.position, step);
            }

            if (currentGarbage.transform.position == pointB.position)
            {
                movingRight = false;
            }
            else if (currentGarbage.transform.position == pointA.position)
            {
                movingRight = true;
            }

            
        }
    }

    public void ButtonFunction()
    {
     
            canMove = false;
            DropGarbage();
    }

     void DropGarbage()
    {
        if (currentGarbage != null)
        {
            Rigidbody2D rb = currentGarbage.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1.5f; 
            }

            Destroy(currentGarbage, 2f);

            InstantiateRandomGarbage();
        }
    }

    void InstantiateRandomGarbage()
    {
        canMove = true;
        int randomIndex = Random.Range(0, garbagePrefabs.Length);
        currentGarbage = Instantiate(garbagePrefabs[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
    }
}

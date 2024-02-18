using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Text highScoreText;
    public Text timerText;

    public float gameDuration = 60f; // Set the game duration in seconds
    private float timer;
    public bool isStart;

    public RewardUI rewardUI;

    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;


    [SerializeField] private DialogBehaviour vinishBehaviour;
    [SerializeField] private DialogNodeGraph winGraph;
    [SerializeField] private DialogNodeGraph loseGraph;
    // Start is called before the first frame update

    public UserSessionScript user;
    void Start()
    {
        user = FindObjectOfType<UserSessionScript>();
        dialogBehaviour.StartDialog(dialogGraph);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (canMove)
            {
                MoveGarbage();
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                EndGame();
            }
        }
    }

    void StartGame()
    {
        isStart = true;
        highScoreText.text = $"High score: {highScore}";
        timer = gameDuration;
        UpdateTimerDisplay();
        InstantiateRandomGarbage();
    }
    void UpdateTimerDisplay()
    {
        // Display the timer in the UI text
        timerText.text = $"Time: {Mathf.Ceil(timer)}";
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
     if(isStart)
        {
            canMove = false;
            DropGarbage();
        }      
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

    void EndGame()
    {
        // Check if the current score is higher than the high score
        if (currentScore > highScore)
        {
            // Call the function for winning
            WinGame();
        }
        else
        {
            // Call the function for losing
            LoseGame();
        }
    }

    void WinGame()
    {
        //OriginalReward();
        rewardUI.isWin = true;
        isStart = false;
        user.coins += 10;
        user.clearSeg = true;
        user.SavePlayerData(); //save
        vinishBehaviour.StartDialog(winGraph);
    }

    void LoseGame()
    {
        Debug.Log("You lost!");
        rewardUI.isWin = false;
        isStart = false;
        if (user.currentHealth > 0)
        {
            user.currentHealth--;
        }
        user.SavePlayerData(); //save
        vinishBehaviour.StartDialog(loseGraph);
    }

    public void StartGameButton()
    {
        StartGame();
    }

    void OriginalReward()
    {
        user.maxHealth++;
        user.currentHealth++;
    }
}

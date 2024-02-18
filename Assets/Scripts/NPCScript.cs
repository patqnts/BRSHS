using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private DialogBehaviour[] dialogBehaviour;
    [SerializeField] private DialogNodeGraph[] dialogGraph;

    public PlayerScript playerScript;
    public bool cleared;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.gameObject.tag == "Player")
        {
            if (playerScript.CurrentHealth <= 0)
            {
                dialogBehaviour[1].StartDialog(dialogGraph[1]);
                return;
            }

            if (cleared)
            {
                dialogBehaviour[1].StartDialog(dialogGraph[1]);
            }
            else
            {
                dialogBehaviour[0].StartDialog(dialogGraph[0]);
            }
            
        }
    }
    public void EnterChallenge(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

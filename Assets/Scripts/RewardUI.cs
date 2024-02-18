using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardUI : MonoBehaviour
{
    public bool isWin;
    public GameObject ui;
    private void Start()
    {
        if(isWin)
        {
            ui.SetActive(true);
        }
        else
        {
            GoBackToMainGame();
        }
    }
    public void GoBackToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}

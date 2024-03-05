using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public PlayerScript player;
    public MainGameScript mainGameScript;
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        mainGameScript = FindObjectOfType<MainGameScript>();
        CheckFan();
    }

    void CheckFan()
    {
        if (player.ClearFan)
        {
            animator.SetBool("Stop", true);
        }
    }
    public void StopFan()
    {
        player.ClearFan = true;
        mainGameScript.Save();
        mainGameScript.LoadPlayerData();
        CheckFan();
    }
}

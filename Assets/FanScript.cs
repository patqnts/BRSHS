using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public UserSessionScript userSession;
    void Start()
    {
        userSession = FindObjectOfType<UserSessionScript>();
        CheckFan();
    }

    void CheckFan()
    {
        if (userSession.clearFan)
        {
            animator.SetBool("Stop", true);
        }
    }
    public void StopFan()
    {
        
            userSession.clearFan = true;
            userSession.SavePlayerData();
        CheckFan();



    }
}

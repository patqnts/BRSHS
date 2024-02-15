using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSessionScript : MonoBehaviour
{
    private float sessionStartTime;
    private bool notificationSent = false;

    void Start()
    {
        sessionStartTime = Time.time;
    }

    void Update()
    {
        float elapsedTime = Time.time - sessionStartTime;

        if (elapsedTime >= 3600 && !notificationSent)
        {
            NotifyGame("Session time is 1 hour!");

            notificationSent = true;
        }
    }

    void NotifyGame(string message)
    {
        Debug.Log(message);
    }
}

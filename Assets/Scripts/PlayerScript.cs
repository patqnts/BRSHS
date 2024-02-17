using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text health;
    public Text coin;

    public int MaxHealth = 3;
    public int CurrentHealth = 3;
    public int Coins = 0;
    public double Progress;
    public bool ClearBroom;
    public bool ClearSeg;
    public bool ClearPick;

    private void Start()
    {
        health.text = $"{CurrentHealth}/{MaxHealth}";
        coin.text = $"{Coins}";
    }

}

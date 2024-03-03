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
    public int SegregateHighScore = 0;
    public double Progress;
    public bool ClearBroom;
    public bool ClearSeg;
    public bool ClearPick;
    public bool ClearFan;
    public bool ClearPlant;

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        health.text = $"{CurrentHealth}/{MaxHealth}";
        coin.text = $"{Coins}";
    }

}

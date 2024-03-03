
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int MaxHealth = 3;
    public int CurrentHealth = 3;
    public int Coins = 0;

    public int SegregateHighScore = 0;
    public Vector2 PlayerPos;
    public double Progress;

    public bool ClearBroom;
    public bool ClearSeg;
    public bool ClearPick;
    public bool ClearFan;
    public bool ClearPlant;
}

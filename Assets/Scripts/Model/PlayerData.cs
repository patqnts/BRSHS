
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int MaxHealth = 3;
    public int CurrentHealth = 3;
    public int Coins = 0;
   public Vector2 PlayerPos { get; set; }
   public double Progress { get; set; }

    public bool ClearBroom;
    public bool ClearSeg;
    public bool ClearPick;
}

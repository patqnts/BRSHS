using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public Collider2D Collider2D;
    public PlayerController Controller;
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

    public void TakeDamage(int damage, Vector2 direction, float knockbackForce)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
            PlayerController.instance.Hurt(damage, direction, knockbackForce);
            if (CurrentHealth <= 0)
            {
                PlayerController.instance.Death();
            }
            LoadData();
        }
    }

    public void Death()
    {
        Debug.Log("Death");
    }

    public void Attack(int damage)
    {
        throw new System.NotImplementedException();
    }
}

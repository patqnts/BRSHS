using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField] private int Health;
    [SerializeField] private Animator animator;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] public SpriteRenderer sprite;

    public static Action OnDeath;
    
    public Sprite[] spriteList;
    public Sprite currentSprite;
    public string[] enemies;
    private string currentEnemy;

    public CircleCollider2D circleCollider2D;
    public BoxCollider2D attackCollider;
    private Rigidbody2D rigidbody;
    private bool isAttacking = false;
    private bool isAlive = true;
    private Transform player;
    void Start()
    {
        if (enemies != null && enemies.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemies.Length);
            currentEnemy = enemies[randomIndex];
            currentSprite = spriteList[randomIndex];
            sprite.sprite = currentSprite;
        }
        else
        {
            Debug.LogError("No enemies defined in the array.");
        }

        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
        animator.Play(currentEnemy);
    }

    void Update()
    {
        if (!isAttacking && isAlive)
        {
            DetectAndChasePlayer();
        }
    }

    private void DetectAndChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer < detectionRadius)
        {
            // Check if the player is within attack range
            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(AttackAnimation());
            }
            else
            {
                // Chase the player
                
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);                
            }
        }       
    }
    public void Attack(int damage)
    {
        StartCoroutine(AttackAnimation());
    }

    private IEnumerator AttackAnimation()
    {
        isAttacking = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        attackCollider.enabled = false;
    }
    private IEnumerator TakeDamageAnimation()
    {
        animator.SetTrigger("TakeDamage");       
        yield return new WaitForSeconds(.3f);
        animator.Play(currentEnemy);
    }

    public void Death()
    {
        StartCoroutine(DeathAnimation());
        EnemyGenerator.instance.EnemyDied();
    }

    public IEnumerator DeathAnimation()
    {
        isAlive = false;
        circleCollider2D.enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject,2f);
    }
    public void TakeDamage(int damage, Vector2 direction, float knockbackForce)
    {
        if(Health > 0)
        {           
            Health -= damage;
            rigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(TakeDamageAnimation());
        }
        if (Health <= 0)
        {
            Death();
        }
    }
}

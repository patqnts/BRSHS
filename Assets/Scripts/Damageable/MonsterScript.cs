using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MonsterScript : MonoBehaviour, IDamageable
{
    [Header("Sprites")]
    public Sprite[] images;

    [Header("References")]
    [SerializeField] private Transform visual; // drag your child sprite here

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int health;

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health <= 0)
            {
                OnDeath();
            }
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = Mathf.Max(1, value);
    }

    [Header("AI Settings")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 1;

    private Transform player;
    private bool isAttacking = false;

    public SpriteRenderer spriteRenderer;
    private Tween idleTween;
    private Tween chaseTween;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = images[Random.Range(1, images.Length)];
        Health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= chaseRange)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }
    private void ClearTweens()
    {
        idleTween?.Kill();
        chaseTween?.Kill();
    }
    #region Idle

    private void Idle()
    {
        // Stop movement
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Kill other tweens
        ClearTweens();

        // Idle bobbing or breathing loop
        idleTween = visual.DOScale(Vector3.one * 1.05f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    #endregion

    #region Chase

    private void Chase()
    {
        // Kill other tweens
        ClearTweens();

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Flip visual to face player
        if (direction.x > 0)
            visual.GetComponent<SpriteRenderer>().flipX = true;
        else if (direction.x < 0)
            visual.GetComponent<SpriteRenderer>().flipX = false;

        // Optional: add chase squash/stretch once
        if (chaseTween == null || !chaseTween.IsActive())
        {
            chaseTween = visual.DOScaleY(0.9f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }
    }


    #endregion

    #region Attack

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;

        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 1);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            var damageable = player.GetComponent<IDamageable>();
            damageable?.OnHit(attackDamage);
        }

        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    #endregion

    #region Damage Handling

    public void OnHit(int damage)
    {
        Health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {Health}");

        // Punch scale for hit reaction
        visual.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 1);

        // Shake position slightly for impact effect
        visual.DOShakePosition(
            duration: 0.2f,     // how long to shake
            strength: 0.1f,     // shake distance
            vibrato: 10,        // how much it jitters
            randomness: 90,     // random factor
            snapping: false,
            fadeOut: true
        );

        // Flash red
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (spriteRenderer != null)
                    spriteRenderer.color = originalColor;
            });
        }
    }


    public void OnHitWithKnockback(Rigidbody2D rb, int damage)
    {
        OnHit(damage);

        if (rb != null)
        {
            Vector2 knockbackDir = (rb.transform.position - transform.position).normalized;
            float knockbackForce = 5f;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void OnDeath()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }

    #endregion
}

using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MonsterScript : MonoBehaviour, IDamageable
{
    public event System.Action<MonsterScript> OnMonsterDied;

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
    private bool isStunned = false;

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

        if (isStunned) return; // 🛑 do nothing while stunned

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
            damageable?.OnHit(attackDamage,null,0);
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

    public void OnHit(int damage, Vector2? hitDir = null, float knockbackForce = 5f)
    {
        Health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {Health}");

        // Stun: stops chase/idle temporarily
        isStunned = true;
        DOVirtual.DelayedCall(0.2f, () => isStunned = false);

        // Hit reaction visuals
        visual.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 1);
        visual.DOShakePosition(0.2f, 0.1f, 10, 90, false, true);

        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            DOVirtual.DelayedCall(0.1f, () => spriteRenderer.color = originalColor);
        }

        // Knockback
        if (hitDir != null && rb != null)
        {
            Vector2 dir = hitDir.Value.normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
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

        // Notify spawner
        OnMonsterDied?.Invoke(this);

        // Disable further logic
        enabled = false;

        // Kill any running tweens
        ClearTweens();

        // Death FX: jump up + spin + fade
        Sequence deathSeq = DOTween.Sequence();

        // Jump up a bit
        deathSeq.Join(transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad));

        // Rotate spin
        deathSeq.Join(visual.DORotate(new Vector3(0, 0, 360f), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad));

        // Fade out sprite
        if (spriteRenderer != null)
        {
            deathSeq.Join(spriteRenderer.DOFade(0f, 0.5f));
        }
        else if (visual.TryGetComponent<SpriteRenderer>(out var visualSprite))
        {
            deathSeq.Join(visualSprite.DOFade(0f, 0.5f));
        }

        // Scale down slightly (optional)
        deathSeq.Join(visual.DOScale(0.5f, 0.5f));

        // Then destroy
        deathSeq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }


    #endregion
}

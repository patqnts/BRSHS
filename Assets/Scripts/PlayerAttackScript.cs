using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button attackButton;
    private Animator animator;
    private PlayerController playerController;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float lungeDistance = 0.5f;      // max lunge distance
    [SerializeField] private float lungeDuration = 0.1f;
    [SerializeField] private float attackRange = 0.8f;         // detect range
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float hitStopDuration = 0.05f;

    private bool canAttack = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (attackButton != null)
            attackButton.onClick.AddListener(AttackMethod);

        canAttack = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackMethod();
        }
    }

    public void AttackMethod()
    {
        if (!canAttack) return;

        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        playerController.canMove = false;
        canAttack = false;

        animator.SetTrigger("attack");

        // Determine attack direction
        Vector2 attackDir = playerController.lastMoveDir != Vector2.zero
            ? playerController.lastMoveDir.normalized
            : Vector2.right;

        // Detect hits
        Vector2 hitPoint = (Vector2)transform.position + attackDir * attackRange;
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint, 0.5f, enemyLayer);

        // Compute lunge target position (fixed)
        Vector3 targetPos = transform.position + (Vector3)(attackDir * lungeDistance);
        transform.DOMove(targetPos, lungeDuration).SetEase(Ease.OutQuad);

        // Wait mid-lunge
        yield return new WaitForSeconds(lungeDuration * 0.5f);

        bool didHitAny = false;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.OnHit(attackDamage);
                didHitAny = true;
            }

            if (hit.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(attackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Apply hit stop if hit any
        if (didHitAny)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(hitStopDuration);
            Time.timeScale = 1f;
        }
        else
        {
            yield return new WaitForSeconds(lungeDuration * 0.5f);
        }

        yield return new WaitForSeconds(attackCooldown);

        playerController.canMove = true;
        canAttack = true;
    }



    private void OnDrawGizmosSelected()
    {
        if (playerController == null) return;

        Vector2 attackDir = playerController.lastMoveDir != Vector2.zero ? playerController.lastMoveDir.normalized : Vector2.right;
        Vector2 hitPoint = (Vector2)transform.position + attackDir * attackRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint, 0.5f);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.InventoryEngine;
using Unity.VisualScripting;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button attackButton;
    [SerializeField] Inventory weaponItem;
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

        // Find targets
        Vector2 hitPoint = (Vector2)transform.position + attackDir * attackRange;
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint, 0.5f, enemyLayer);

        bool didHitAny = false;

        if (hits.Length > 0)
        {
            // Lunge only if hitting something
            Vector3 targetPos = transform.position + (Vector3)(attackDir * lungeDistance);
            transform.DOMove(targetPos, lungeDuration).SetEase(Ease.OutQuad);

            // Wait mid-lunge
            yield return new WaitForSeconds(lungeDuration * 0.5f);

            foreach (var hit in hits)
            {
                if (hit == null) continue; // Skip if the collider is destroyed

                // Optional: check if the GameObject itself is null or destroyed
                if (hit.gameObject == null) continue;

                if (hit.TryGetComponent<IDamageable>(out var damageable))
                {
                    

                    WeaponItem weap = (WeaponItem)weaponItem.Content[0];
                    var damage = 0;
                    Debug.Log(weap.ItemName);
                    if(weap != null)
                    {
                        damage = weap.AttackDamage;
                    }
                    damageable.OnHit(damage, attackDir, knockbackForce);
                    didHitAny = true;
                }
            }

        }
        else
        {
            // No hit: no lunge
            yield return new WaitForSeconds(lungeDuration * 0.5f);
        }

        // HIT STOP if we hit anything
        if (didHitAny)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(hitStopDuration);
            Time.timeScale = 1f;
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

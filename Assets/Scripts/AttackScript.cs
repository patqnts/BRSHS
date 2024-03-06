using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public enum ObjectEntity
    {
        Player,
        Enemy
    }
    public ObjectEntity entity;

    public BoxCollider2D attackCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (attackCollider != null && damageable != null && collision.gameObject.tag == entity.ToString())
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            float knockbackForce = 25f;

            damageable.TakeDamage(1, knockbackDirection, knockbackForce);
        }
    }
}

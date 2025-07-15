using UnityEngine;

public interface IDamageable 
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public void OnHit(int damage, Vector2? hitDir, float knockbackForce);
    public void OnHitWithKnockback(Rigidbody2D rb, int damage);
    public void OnDeath();

}

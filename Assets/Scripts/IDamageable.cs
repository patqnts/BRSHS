
using UnityEngine;

public interface IDamageable 
{
    public void TakeDamage(int damage,Vector2 direction, float knockbackForce);
    public void Death();
    public void Attack(int damage);
}

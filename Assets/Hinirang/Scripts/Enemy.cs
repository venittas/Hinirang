using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public float attackTimer;
    public float attackDuration;
    public float chaseRange;
    public float roamRange;


    public abstract void Chase();
    public abstract void Roam();
    public abstract void Attack();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

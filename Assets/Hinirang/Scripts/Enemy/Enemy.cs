using System.Collections;
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
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 1f;
    public float flashInterval = 0.2f;


    public abstract void Chase();
    public abstract void Roam();
    public abstract void Attack();
    public abstract void TakeDamage(float damage);

    public IEnumerator Flash()
    {
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Visual effect
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

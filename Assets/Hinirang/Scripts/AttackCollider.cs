using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    private void Update()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        Vector2 playerDirection = Player.Instance.GetLastLookDirection();
        if (playerDirection == Vector2.down)
        {

            transform.localPosition = new Vector3(0.002f, 0.02f, 0);
        }
        else if (playerDirection == Vector2.right)
        {

            transform.localPosition = new Vector3(0.12f, 0.117f, 0);

        }
        else if (playerDirection == Vector2.up)
        {

            transform.localPosition = new Vector3(0, 0.259f, 0);
        }
        else if (playerDirection == Vector2.left)
        {

            transform.localPosition = new Vector3(-0.12f, 0.117f, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Player.Instance.GetWeaponDamage());
        }
        
    }
}

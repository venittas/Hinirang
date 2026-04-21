using UnityEngine;

public class WhipCollider : MonoBehaviour
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

            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0f);

        }
        else if (playerDirection == Vector2.right)
        {

            transform.localPosition = new Vector3(0.41f, 0.03f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, -90f);

        }
        else if (playerDirection == Vector2.up)
        {

            transform.localPosition = new Vector3(0, 0.43f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0f);
        }
        else if (playerDirection == Vector2.left)
        {

            transform.localPosition = new Vector3(-0.06f, 0.03f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
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

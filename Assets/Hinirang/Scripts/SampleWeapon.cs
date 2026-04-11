using UnityEngine;

public class SampleWeapon : InventoryItem
{
    public static SampleWeapon Instance;
    Collider2D stickCollider;
    Renderer weaponRenderer;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        stickCollider = GetComponent<PolygonCollider2D>();
        weaponRenderer = GetComponent<Renderer>();
        stickCollider.enabled = false;
    }

    private void Update()
    {
        if (isPickedUp)
        {
            UpdatePosition();
        }
        else
        {
            stickCollider.enabled = true;
        }
    }

    public new void PickUp()
    {
        gameObject.transform.SetParent(Player.Instance.transform);
        isPickedUp = true;
    }

    private void UpdatePosition()
    {
        Vector2 playerDirection = Player.Instance.GetLastLookDirection();
        if (playerDirection == Vector2.down)
        {

            transform.localPosition = new Vector3(-0.0551000014f, -0.0724000037f, 0.730000019f);
            transform.localRotation = Quaternion.Euler(0, 0, -180f);
        }
        else if (playerDirection == Vector2.right)
        {

            transform.localPosition = new Vector3(0.0399999991f, -0.0520000011f, 0.730000019f);
            transform.localRotation = Quaternion.Euler(0, 0, -90f);
            weaponRenderer.sortingLayerName = "Player";
            
        }
        else if (playerDirection == Vector2.up)
        {

            transform.localPosition = new Vector3(0.0535000004f, 0.00850000046f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            weaponRenderer.sortingLayerName = "Default";
        }
        else if (playerDirection == Vector2.left)
        {

            transform.localPosition = new Vector3(-0.0436999984f, -0.0244999994f, 0.730000019f);
            transform.localRotation = Quaternion.Euler(0, 0, 90f);
            weaponRenderer.sortingLayerName = "Default";
        }


    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Stick hit an enemy!");
    //        if (enemy != null)
    //        {
    //            enemy.TakeDamage(2);
    //        }
    //    }
    //}

    public void Attack()
    {
        stickCollider.enabled = true;
        transform.localScale = new Vector3(1, 2, 1);
        Invoke(nameof(DisableCollider), 0.2f);
    }

    private void DisableCollider()
    {
        stickCollider.enabled = false;
        transform.localScale = new Vector3(1, 1, 1);
    }
}

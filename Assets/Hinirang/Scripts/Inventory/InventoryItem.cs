using UnityEngine;

public class InventoryItem : Interactable
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int maxStack = 99;
    public bool isPickedUp = false;
    public Renderer itemRenderer;
    public Collider2D stickCollider;
    public bool isWeapon = false;
    public float damage = 2f;

    public void Start()
    {
        base.Start();
    }

    public void Awake()
    {
        itemRenderer = GetComponent<Renderer>();
        stickCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isPickedUp)
        {
            UpdatePosition();
            stickCollider.enabled = false;
        }
        else
        {
            stickCollider.enabled = true;
        }
    }

    public void PickUp()
    {
        gameObject.transform.SetParent(Player.Instance.transform);
        isPickedUp = true;
        stickCollider.enabled = false;
    }

    public new void ShowIndicator()//pwede pala yon, gagamit ng new pag override
    {
        if (!isPickedUp)
        {
            base.ShowIndicator();
        }
    }

    public new void HideIndicator()
    {
        if (!isPickedUp)
        {
            base.HideIndicator();
        }
    }

    public override void Interact(string eventName)
    {
        InventorySystem.Instance.AddItem(this, 1);
        Equip();
    }

    public virtual void Equip()
    {
        Debug.Log("Equipping item: " + itemName);
        gameObject.transform.SetParent(Player.Instance.transform);
        isPickedUp = true;
        stickCollider.enabled = false;  
        gameObject.SetActive(true);
    }

    public virtual void Unequip()
    {
        gameObject.transform.SetParent(null);
        stickCollider.enabled = true;  
        isPickedUp = false;
        gameObject.SetActive(false);  
    }

    private void UpdatePosition()
    {
        if (isWeapon) return;
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
            itemRenderer.sortingLayerName = "Player";

        }
        else if (playerDirection == Vector2.up)
        {

            transform.localPosition = new Vector3(0.0535000004f, 0.00850000046f, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            itemRenderer.sortingLayerName = "Default";
        }
        else if (playerDirection == Vector2.left)
        {

            transform.localPosition = new Vector3(-0.0436999984f, -0.0244999994f, 0.730000019f);
            transform.localRotation = Quaternion.Euler(0, 0, 90f);
            itemRenderer.sortingLayerName = "Default";
        }
    }
}

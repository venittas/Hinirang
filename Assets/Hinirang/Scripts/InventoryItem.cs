using UnityEngine;

public class InventoryItem : Interactable
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int maxStack = 99;
    public bool isPickedUp = false;

    public void Start()
    {
        base.Start();
    }


    public override void Interact(string eventName)
    {
        InventorySystem.Instance.AddItem(this, 1);
    }
}

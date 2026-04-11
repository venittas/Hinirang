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

    public void Equip()
    {
        //gagana lang pag meron nung UpdatePosition gaya ng sa sample weapon
        //pero pwede naman maglagay ng ibang logic dito depende sa item
        if (gameObject.name == "Sample Weapon")
        {
            gameObject.transform.SetParent(Player.Instance.transform);
            isPickedUp = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}

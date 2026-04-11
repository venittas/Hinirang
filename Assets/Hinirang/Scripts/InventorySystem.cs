using System.Collections.Generic;
using UnityEngine;
public class InventorySystem : MonoBehaviour
{

    public static int maxSlots = 5;
    public List<InventorySlot> slots = new List<InventorySlot>(maxSlots);
    public List<GameObject> inventoryUI = new List<GameObject>(maxSlots);
    public GameObject InventorySlot1;
    public GameObject InventorySlot2;
    public GameObject InventorySlot3;
    public GameObject InventorySlot4;
    public GameObject InventorySlot5;
    public static InventorySystem Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        inventoryUI.Add(InventorySlot1);
        inventoryUI.Add(InventorySlot2);
        inventoryUI.Add(InventorySlot3);
        inventoryUI.Add(InventorySlot4);
        inventoryUI.Add(InventorySlot5);
    }
    public bool AddItem(InventoryItem item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    slot.quantity += amount;
                    return true;
                }
            }
        }
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot { item = item, quantity = amount });
            GameObject imgObj = new GameObject("InventoryItem");
            UnityEngine.UI.Image uiImage = imgObj.AddComponent<UnityEngine.UI.Image>();
            uiImage.sprite = item.icon;
            if (slots.Count != 0)
            {
                imgObj.transform.SetParent(inventoryUI[slots.Count - 1].transform, false);
            }
            else
            {
                imgObj.transform.SetParent(inventoryUI[0].transform, false);
            }
                return true;
        }
        return false;
    }

}

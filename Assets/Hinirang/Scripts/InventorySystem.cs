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
            Debug.Log("Trying to add stackable item: " + item.itemName + " with quantity: " + amount);
            foreach (var slot in slots)
            {
                if (slot.item.itemName == item.itemName && slot.quantity < item.maxStack)
                {
                    Debug.Log("Else Adding stackable item: " + item.itemName + " with quantity: " + amount);
                    slot.quantity += amount;
                    GameObject quantityObj = inventoryUI[slots.IndexOf(slot)].transform.Find("QuantityText").gameObject;
                    UnityEngine.UI.Text quantityText = quantityObj.GetComponent<UnityEngine.UI.Text>();
                    quantityText.text = slot.quantity.ToString();
                    
                    return true;
                }
            }
        }
        if (slots.Count < maxSlots) //displaying item in inventory ui
        {
            Debug.Log("Adding new item: " + item.itemName + " with quantity: " + amount);
            slots.Add(new InventorySlot { item = item, quantity = amount });
            GameObject imgObj = new GameObject("InventoryItem");
            UnityEngine.UI.Image uiImage = imgObj.AddComponent<UnityEngine.UI.Image>();
            imgObj.transform.localScale = new Vector3(0.65f, 0.65f, 1f);
            uiImage.sprite = item.icon;
            if (slots.Count != 0)
            {
                imgObj.transform.SetParent(inventoryUI[slots.Count - 1].transform, false);
            }
            else
            {
                imgObj.transform.SetParent(inventoryUI[0].transform, false);
            }

            if (item.isStackable)
            {
                Debug.Log("Adding quantity text for new stackable item: " + item.itemName + " with quantity: " + amount);
                GameObject quantityObj = new GameObject("QuantityText");
                UnityEngine.UI.Text quantityText = quantityObj.AddComponent<UnityEngine.UI.Text>();
                quantityText.text = amount.ToString();
                quantityText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                quantityText.fontSize = 35;
                quantityText.color = Color.black;
                quantityText.alignment = TextAnchor.UpperRight;
                quantityObj.transform.SetParent(inventoryUI[slots.Count - 1].transform, false);
                RectTransform rectTransform = quantityObj.GetComponent<RectTransform>();
                //aayusin yung position ng quantity text sa inventory slot
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.anchoredPosition = new Vector2(-68.3f, 1.599998f);
            }
            return true;
        }
        return false;
    }

}

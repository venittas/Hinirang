using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventorySystem : MonoBehaviour
{
    public int selectedSlotIndex = -1;
    public static int maxSlots = 5;
    public List<InventorySlot> slots = new List<InventorySlot>(maxSlots);
    public List<GameObject> inventoryUI = new List<GameObject>(maxSlots);
    public GameObject InventorySlot1;
    public GameObject InventorySlot2;
    public GameObject InventorySlot3;
    public GameObject InventorySlot4;
    public GameObject InventorySlot5;
    public GameObject SelectedItemIndicator;
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
        EventSystem.current.SetSelectedGameObject(null);
    }
    public bool AddItem(InventoryItem item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item.itemName == item.itemName && slot.quantity < item.maxStack)
                {
                    slot.quantity += amount;
                    GameObject quantityObj = inventoryUI[slots.IndexOf(slot)].transform.Find("QuantityText").gameObject;
                    UnityEngine.UI.Text quantityText = quantityObj.GetComponent<UnityEngine.UI.Text>();
                    quantityText.text = slot.quantity.ToString();
                    SelectedItemIndicator.transform.SetParent(inventoryUI[slots.IndexOf(slot)].transform, false);
                    SelectedItemIndicator.SetActive(true);
                    int index = slots.IndexOf(slot);
                    SetEquippedItem(index);
                    return true;
                }
            }
        }
        if (slots.Count < maxSlots) //displaying item in inventory ui
        {
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
            SelectedItemIndicator.transform.SetParent(inventoryUI[slots.Count - 1].transform, false);
            SelectedItemIndicator.SetActive(true);
            int newIndex = slots.Count - 1;
            SetEquippedItem(newIndex);
            return true;
        }
        return false;
    }

    public void SetEquippedItem(int index)
    {
        if (index == 0)
        {
            Debug.LogError("WHY IS 0 BEING CALLED RIGHT NOW?");
        }
        Debug.Log("CLICKED INDEX: " + index + " | CURRENT: " + selectedSlotIndex);

        if (index < 0 || index >= slots.Count)
        {
            Debug.LogWarning("Invalid inventory slot index: " + index);
            return;
        }

        if (index == selectedSlotIndex)
        {
            Debug.Log("TOGGLING OFF index: " + index);
            slots[index].item.Unequip();
            selectedSlotIndex = -1;
            SelectedItemIndicator.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        if (selectedSlotIndex >= 0)
        {
            Debug.Log("Unequipping previous: " + selectedSlotIndex);
            slots[selectedSlotIndex].item.Unequip();
        }

        selectedSlotIndex = index;

        Debug.Log("Equipping new: " + index);

        SelectedItemIndicator.transform.SetParent(inventoryUI[index].transform, false);
        SelectedItemIndicator.SetActive(true);

        slots[index].item.Equip();
    }

}

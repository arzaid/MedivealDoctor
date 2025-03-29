using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSlots = 10;
    public InventoryUI inventoryUI; // Assigned by PlayerMovement
    public delegate void InventoryChangedEventHandler();
    public event InventoryChangedEventHandler OnInventoryChanged;
    private void NotifyInventoryChanged() { if (OnInventoryChanged != null) OnInventoryChanged(); }
    public bool AddItem(Item newItem)
    {
        foreach (Item item in items)
        {
            if (item.name == newItem.name)
            {
                item.quantity += newItem.quantity;
                if (inventoryUI != null) inventoryUI.UpdateUI();
                return true;
            }
        }
        if (items.Count < maxSlots)
        {
            items.Add(newItem);
            if (inventoryUI != null) inventoryUI.UpdateUI();
            return true;
        }
        Debug.Log("Inventory full!");
        return false;
    }
    public void RemoveItem(string itemName, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemName)
            {
                items[i].quantity -= amount;
                if (items[i].quantity <= 0)
                    items.RemoveAt(i);
                if (inventoryUI != null) inventoryUI.UpdateUI();
                return;
            }
        }
        Debug.Log("Item not found!");
    }
    public bool HasItem(string name, int quantity)
    {
        foreach (Item item in items)
        {
            if (item.name == name && item.quantity >= quantity)
                return true;
        }
        return false;
    }
}
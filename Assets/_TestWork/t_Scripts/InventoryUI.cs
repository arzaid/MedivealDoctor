using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Drag player’s Inventory here
    public GameObject slotPrefab; // Drag your slot prefab here
    public Transform slotContainer; // Drag Grid Layout Group here

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Clear old slots
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        // Create new slots
        foreach (Item item in inventory.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            slot.GetComponentInChildren<Image>().sprite = item.icon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = item.quantity.ToString();
        }
    }
}
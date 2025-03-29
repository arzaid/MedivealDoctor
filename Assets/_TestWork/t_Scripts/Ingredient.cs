using UnityEngine;

[System.Serializable] // Allows this to show in Unity Inspector
public class Ingredient
{
    public string name;       // e.g., "Herb", "Root"
    public Sprite icon;       // For UI display
    public int quantity;      // Stackable amount

    public Ingredient(string name, Sprite icon, int quantity = 1)
    {
        this.name = name;
        this.icon = icon;
        this.quantity = quantity;
    }
}
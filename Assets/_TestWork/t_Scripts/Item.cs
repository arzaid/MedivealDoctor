using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public Sprite icon;
    public int quantity;
    public ItemType type;

    public Item(string name, Sprite icon, ItemType type, int quantity = 1)
    {
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.quantity = quantity;
    }
}

public enum ItemType
{
    Ingredient,
    Potion
}
public class IngredientRequirement
{
    public string ingredientName;
    public int quantity;

    public IngredientRequirement(string ingredientName, int quantity)
    {
        this.ingredientName = ingredientName;
        this.quantity = quantity;
    }
}
public class Recipe
{
    public string potionName;
    public Sprite potionIcon;
    public List<IngredientRequirement> ingredients;

    public Recipe(string potionName, Sprite potionIcon, params IngredientRequirement[] ingredients)
    {
        this.potionName = potionName;
        this.potionIcon = potionIcon;
        this.ingredients = new List<IngredientRequirement>(ingredients);
    }
}
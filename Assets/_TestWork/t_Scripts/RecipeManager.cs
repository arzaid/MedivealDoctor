using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;
    void Awake() { instance = this; }
    public List<Recipe> recipes = new List<Recipe>();

    void Start()
    {
        recipes.Add(new Recipe("Healing Potion", Resources.Load<Sprite>("HealingPotion"),
            new IngredientRequirement("Herb", 2), new IngredientRequirement("Root", 1)));
        // Add more recipes as needed, e.g., "Cure Flu" requiring Herb x1, Berry x2
    }
}
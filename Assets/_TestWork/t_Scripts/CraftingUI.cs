using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftingUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform recipeSlotContainer;
    public RectTransform recipeSlotPrefab;
    private List<(Button button, Recipe recipe)> craftButtons = new List<(Button, Recipe)>();

    void Start()
    {
        CreateRecipeSlots();
        if (inventory != null)
            inventory.OnInventoryChanged += UpdateCraftButtons;
    }

    void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateCraftButtons;
    }

    void CreateRecipeSlots()
    {
        foreach (var recipe in RecipeManager.instance.recipes)
        {
            var slot = Instantiate(recipeSlotPrefab, recipeSlotContainer);
            slot.Find("PotionName").GetComponent<Text>().text = recipe.potionName;
            slot.Find("PotionIcon").GetComponent<Image>().sprite = recipe.potionIcon;

            var ingredientList = slot.Find("IngredientList").GetComponent<RectTransform>();
            foreach (var req in recipe.ingredients)
            {
                var textObj = new GameObject("IngredientText");
                textObj.AddComponent<RectTransform>();
                var text = textObj.AddComponent<Text>();
                text.text = $"{req.ingredientName}: {req.quantity}";
                textObj.transform.SetParent(ingredientList);
                textObj.transform.localScale = Vector3.one;
            }

            var craftButton = slot.Find("CraftButton").GetComponent<Button>();
            craftButton.onClick.AddListener(() => CraftPotion(recipe));
            craftButtons.Add((craftButton, recipe));
            UpdateCraftButton(craftButton, recipe);
        }
    }

    void UpdateCraftButtons()
    {
        foreach (var pair in craftButtons)
            UpdateCraftButton(pair.button, pair.recipe);
    }

    void UpdateCraftButton(Button button, Recipe recipe)
    {
        bool canCraft = true;
        foreach (var req in recipe.ingredients)
            if (!inventory.HasItem(req.ingredientName, req.quantity))
            { canCraft = false; break; }
        button.interactable = canCraft;
    }

    void CraftPotion(Recipe recipe)
    {
        bool canCraft = true;
        foreach (var req in recipe.ingredients)
            if (!inventory.HasItem(req.ingredientName, req.quantity))
            { canCraft = false; break; }
        if (!canCraft) { Debug.Log("Missing ingredients!"); return; }

        foreach (var req in recipe.ingredients)
            inventory.RemoveItem(req.ingredientName, req.quantity);
        Item newPotion = new Item(recipe.potionName, recipe.potionIcon, ItemType.Potion);
        inventory.AddItem(newPotion);
        UpdateCraftButtons();
    }
}

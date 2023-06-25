// CraftingSystem.cs - A class representing a crafting system in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The CraftingSystem class represents a crafting system in a game. It allows the addition of crafting recipes and the crafting of items
// based on those recipes. The class maintains a dictionary of crafting recipes, where the key is the recipe name and the value is an
// instance of the CraftingRecipe class.
//
// To use the crafting system, you can add crafting recipes using the AddCraftingRecipe method, passing in an instance of CraftingRecipe
// that defines the recipe name, required ingredients, crafted item, and the character performing the crafting.
//
// The CraftItem method takes a recipe name and attempts to craft the corresponding item. It checks if the recipe exists and if the
// crafting requirements are met, including the availability of ingredients in the crafting character's inventory. If all conditions are
// satisfied, the required ingredients are consumed, and the crafted item is added to the character's inventory.
//
// The CraftingSystem class is designed to be used in conjunction with other classes and systems in a game context.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters
{
    public class CraftingSystem
    {
        private readonly Dictionary<string, CraftingRecipe> _craftingRecipes;

        public CraftingSystem()
        {
            _craftingRecipes = new Dictionary<string, CraftingRecipe>();
        }

        public void AddCraftingRecipe(CraftingRecipe recipe)
        {
            _craftingRecipes.Add(recipe.RecipeName, recipe);
        }

        public Item CraftItem(string recipeName)
        {
            if (_craftingRecipes.TryGetValue(recipeName, out CraftingRecipe recipe))
            {
                if (recipe.CanCraft())
                {
                    // Check if the crafting character's inventory is available
                    Inventory craftingCharacterInventory = recipe.CraftingCharacter.GetInventory();
                    if (craftingCharacterInventory != null)
                    {
                        // Check if the crafting character has all the required ingredients
                        foreach (KeyValuePair<Item, int> ingredient in recipe.Ingredients)
                        {
                            int itemCount = craftingCharacterInventory.GetItemCount(ingredient.Key);
                            if (itemCount < ingredient.Value)
                            {
                                Debug.LogWarning($"Cannot craft item {recipeName}. Insufficient ingredient: {ingredient.Key.Name}. Required: {ingredient.Value}, Available: {itemCount}");
                                return null;
                            }
                        }
                        // Consume required ingredients
                        foreach (KeyValuePair<Item, int> ingredient in recipe.Ingredients)
                        {
                            for (int i = 0; i < ingredient.Value; i++)
                            {
                                craftingCharacterInventory.RemoveItemFromInventory(ingredient.Key);
                            }
                        }
                        // Create and return the crafted item
                        Item craftedItem = recipe.CraftItem();
                        if (craftedItem != null)
                        {
                            craftingCharacterInventory.AddItemToInventory(craftedItem);
                            return craftedItem;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Crafting character does not have an inventory.");
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot craft item. Requirements not met.");
                }
            }
            else
            {
                Debug.LogWarning($"Crafting recipe '{recipeName}' not found.");
            }
            return null;
        }
    }

    public abstract class CraftingRecipe
    {
        public string RecipeName { get; private set; }
        public Dictionary<Item, int> Ingredients { get; private set; }
        public Item CraftedItem { get; private set; }
        public Character CraftingCharacter { get; private set; }

        public CraftingRecipe(string recipeName, Dictionary<Item, int> ingredients, Item craftedItem, Character craftingCharacter)
        {
            RecipeName = recipeName;
            Ingredients = ingredients;
            CraftedItem = craftedItem;
            CraftingCharacter = craftingCharacter;
        }

        public bool CanCraft()
        {
            // Check if the crafting character has all the required ingredients
            foreach (KeyValuePair<Item, int> ingredient in Ingredients)
            {
                if (CraftingCharacter.GetInventory().GetItemCount(ingredient.Key) < ingredient.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public Item CraftItem()
        {
            // Create a new instance of the crafted item
            Item craftedItem = new Item(CraftedItem.Name, CraftedItem.Slot, CraftedItem.StatModifiers,
                CraftedItem.Weight, CraftedItem.RequiredLevel, CraftedItem.ProficiencyRequirements,
                CraftedItem.IsTwoHanded, CraftedItem.Rarity); // Include item rarity
            // Add the crafted item to the character's inventory
            CraftingCharacter.GetInventory().AddItemToInventory(craftedItem);
            return craftedItem;
        }
    }
}
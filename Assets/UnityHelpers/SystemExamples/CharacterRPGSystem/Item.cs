// Item.cs - A class representing an item in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Item class represents an item that can be used or equipped by a character in a game. Items have various properties such as name, slot,
// stat modifiers, weight, required level, proficiency requirements, whether they are two-handed, and rarity. The class provides methods to check
// if an item can be equipped by a character based on their level and proficiency, as well as utility methods for item consumption.
//
// The ProficiencyRequirement class is an abstract base class for representing a proficiency requirement that an item may have. It provides a
// ProficiencyName property and an abstract IsMet method to check if the requirement is met by a character.
//
// The LevelRequirement class is a subclass of ProficiencyRequirement and represents a specific type of proficiency requirement based on the
// character's level. It overrides the IsMet method to check if the character's level meets the required level.
//
// The ConsumableItem class is an abstract subclass of Item that represents a consumable item that can be used by a character. It adds a RemainingUses
// property to track the number of uses remaining for the item and provides an abstract Consume method to perform the item's consumption logic.
//
// The HealthPotion class is a subclass of ConsumableItem and represents a specific type of consumable item that restores health to a character.
// It adds a HealingAmount property to specify the amount of health restored and implements the Consume method to restore the character's health
// and decrement the RemainingUses.
//
// The ManaPotion class is another subclass of ConsumableItem and represents a specific type of consumable item that restores mana to a character.
// It adds a RestoreAmount property to specify the amount of mana restored and implements the Consume method to restore the character's mana
// and decrement the RemainingUses.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    
    public class Item
    {
        public string Name { get; private set; }
        public EquipmentSlot Slot { get; private set; }
        public Dictionary<string, float> StatModifiers { get; private set; }
        public float Weight { get; private set; }
        public int RequiredLevel { get; private set; }
        public bool IsTwoHanded { get; private set; }
        public List<ProficiencyRequirement> ProficiencyRequirements { get; private set; }
        public ItemRarity Rarity { get; private set; }

        public Item(string name, EquipmentSlot slot, Dictionary<string, float> statModifiers, float weight, int requiredLevel, List<ProficiencyRequirement> proficiencyRequirements, bool isTwoHanded, ItemRarity rarity)
        {
            Name = name;
            Slot = slot;
            StatModifiers = statModifiers;
            Weight = weight;
            RequiredLevel = requiredLevel;
            ProficiencyRequirements = proficiencyRequirements;
            IsTwoHanded = isTwoHanded;
            Rarity = rarity;
        }

        public bool CanBeEquipped(Character character)
        {
            // Check if the character meets the required level for the item
            if (RequiredLevel > character.Level)
            {
                Debug.LogWarning($"Cannot equip item {Name}. Required level: {RequiredLevel}");
                return false;
            }
            // Check if the character meets all proficiency requirements
            foreach (ProficiencyRequirement requirement in ProficiencyRequirements)
            {
                if (!requirement.IsMet(character))
                {
                    Debug.LogWarning($"Cannot equip item {Name}. Proficiency requirement not met: {requirement}");
                    return false;
                }
            }
            return true;
        }
    }

    public abstract class ProficiencyRequirement
    {
        public string ProficiencyName { get; private set; }

        public ProficiencyRequirement(string proficiencyName)
        {
            ProficiencyName = proficiencyName;
        }

        public abstract bool IsMet(Character character);

        public override string ToString()
        {
            return $"Proficiency: {ProficiencyName}";
        }
    }

    public class LevelRequirement : ProficiencyRequirement
    {
        public int RequiredLevel { get; private set; }

        public LevelRequirement(int requiredLevel) : base("Level")
        {
            RequiredLevel = requiredLevel;
        }

        public override bool IsMet(Character character)
        {
            return character.Level >= RequiredLevel;
        }

        public override string ToString()
        {
            return $"Proficiency: {ProficiencyName}, Required Level: {RequiredLevel}";
        }
    }

    public abstract class ConsumableItem : Item
    {
        public int RemainingUses { get; set; }

        public ConsumableItem(string name, Dictionary<string, float> statModifiers, float weight, bool isTwoHanded, ItemRarity rarity, int remainingUses)
            : base(name, EquipmentSlot.None, statModifiers, weight, 1, new List<ProficiencyRequirement>(), isTwoHanded, rarity)
        {
            RemainingUses = remainingUses;
        }

        public abstract void Consume(Character character);
    }

    public class HealthPotion : ConsumableItem
    {
        public int HealingAmount { get; private set; }

        public HealthPotion(string name, int healingAmount, float weight, bool isTwoHanded, ItemRarity rarity, int remainingUses)
            : base(name, new Dictionary<string, float>(), weight, isTwoHanded, rarity, remainingUses)
        {
            HealingAmount = healingAmount;
        }

        public override void Consume(Character character)
        {
            if (RemainingUses <= 0) return;
            
            character.Stats.Health.Heal(HealingAmount);
            RemainingUses--;
        }
    }

    public class ManaPotion : ConsumableItem
    {
        public int RestoreAmount { get; private set; }

        public ManaPotion(string name, int amount, float weight, bool isTwoHanded, ItemRarity rarity, int remainingUses)
            : base(name, new Dictionary<string, float>(), weight, isTwoHanded, rarity, remainingUses)
        {
            RestoreAmount = amount;
        }

        public override void Consume(Character character)
        {
            if (RemainingUses <= 0) return;
            
            character.RestoreMana(RestoreAmount);
            RemainingUses--;
        }
    }
}
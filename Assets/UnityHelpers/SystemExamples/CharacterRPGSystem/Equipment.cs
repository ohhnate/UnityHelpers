// Equipment.cs - A class representing equipment in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Equipment class represents the equipment system in a game. It allows a character to equip and unequip items based on their properties
// and requirements. The class maintains a dictionary of equipped items, where the key is the equipment slot and the value is the item
// currently equipped in that slot.
//
// To use the equipment system, you need to call the Initialize method, passing in the character that will be equipped with items.
//
// The EquipItem method is used to equip an item to the character. It performs various checks to ensure the item can be equipped, such as
// compatibility with the character's stats, availability of the item slot, and meeting proficiency and level requirements. If all checks
// pass, the method unequips any existing item in the same slot, equips the new item, and updates the character's stats accordingly.
//
// The UnequipItem method is used to unequip an item from the character. It removes the item from the equipped items dictionary and removes
// the item's stats from the character.
//
// The GetEquippedItems method returns the dictionary of currently equipped items.
//
// The Equipment class is designed to be used in conjunction with other classes and systems in a game context.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public class Equipment
    {
        private Character _character;
        private Dictionary<EquipmentSlot, Item> _equippedItems;

        public void Initialize(Character character)
        {
            _character = character;
            _equippedItems = new Dictionary<EquipmentSlot, Item>();
        }

        public void EquipItem(Item item)
        {
            if (item == null) return;

            // Check if the item can be equipped by this character
            if (!item.CanBeEquipped(_character))
            {
                Debug.LogWarning($"Cannot equip item {item.Name}. Incompatible with character stats.");
                return;
            }
            // Check if the item slot is available
            if (item.Slot == EquipmentSlot.None)
            {
                Debug.LogWarning($"Cannot equip item {item.Name}. Item is not equippable.");
                return;
            }
            // Check if the item meets all proficiency requirements and level requirements
            if (!item.ProficiencyRequirements.All(requirement => requirement.IsMet(_character)))
            {
                Debug.LogWarning($"Cannot equip item {item.Name}. Proficiency requirements or level requirement not met.");
                return;
            }
    
            // Unequip any existing item in the same slot
            if (_equippedItems.ContainsKey(item.Slot))
            {
                UnequipItem(_equippedItems[item.Slot]);
            }
            // Check if the item is a one-handed weapon and there is a similar item in the opposite hand
            if (!item.IsTwoHanded && item.Slot == EquipmentSlot.MainHand && _equippedItems.ContainsKey(EquipmentSlot.OffHand))
            {
                Item offhandItem = _equippedItems[EquipmentSlot.OffHand];
                if (offhandItem.Slot == EquipmentSlot.OffHand && offhandItem.Name == item.Name)
                {
                    UnequipItem(offhandItem);
                }
            }
            // Equip the item and update character stats
            _equippedItems[item.Slot] = item;
            ApplyItemStats(item);
        }

        public void UnequipItem(Item item)
        {
            // if item doesn't exist return
            if (item == null || !_equippedItems.ContainsValue(item)) return;

            // Remove the item from the equipped items dictionary
            foreach (KeyValuePair<EquipmentSlot, Item> eqItem in _equippedItems)
            {
                if (eqItem.Value == item)
                {
                    _equippedItems.Remove(eqItem.Key);
                    break;
                }
            }
            // Remove the item's stats from the character
            RemoveItemStats(item);
        }

        public Dictionary<EquipmentSlot, Item> GetEquippedItems()
        {
            return _equippedItems;
        }

        private void ApplyItemStats(Item item)
        {
            foreach (KeyValuePair<string, float> statModifier in item.StatModifiers)
            {
                dynamic statProperty = _character.Stats.StatMap[statModifier.Key];
                statProperty.Increase(statModifier.Value);
            }
        }

        private void RemoveItemStats(Item item)
        {
            foreach (KeyValuePair<string, float> statModifier in item.StatModifiers)
            {
                dynamic statProperty = _character.Stats.StatMap[statModifier.Key];
                statProperty.Decrease(statModifier.Value);
            }
        }
    }

    public enum EquipmentSlot
    {
        None,
        Head,
        Chest,
        Legs,
        Hands,
        Feet,
        MainHand,
        OffHand,
        Accessory
    }
}
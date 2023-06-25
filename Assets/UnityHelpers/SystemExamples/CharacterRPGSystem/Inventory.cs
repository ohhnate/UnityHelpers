// Inventory.cs - A class representing a character's inventory in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Inventory class represents a character's inventory in a game. It allows items to be added, removed, equipped, and unequipped
// from the inventory. The class also tracks the carry capacity and weight of the inventory, and applies an encumbered status effect
// if the carry capacity is exceeded.
//
// To use the inventory system, you need to call the Initialize method, passing in the character that owns the inventory.
//
// The GetItemCount method returns the count of a specified item in the inventory.
//
// The AddItemToInventory method adds an item to the inventory. It checks if the item can be carried based on the carry capacity and
// applies the encumbered status effect if necessary.
//
// The RemoveItemFromInventory method removes an item from the inventory.
//
// The EquipItem method equips an item from the inventory. It checks if the item is already equipped, handles two-handed weapons and
// one-handed weapons with similar counterparts in the opposite hand, and applies item bonuses to the character's stats.
//
// The UnequipItem method unequips an item and returns it to the inventory. It removes the item bonuses from the character's stats.
//
// The Inventory class is designed to be used in conjunction with other classes and systems in a game context.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public class Inventory
    {
        private Character _character;
        private List<Item> _inventoryItems;
        private Dictionary<EquipmentSlot, Item> _equippedItems;
        private float _carryCapacity;
        private float _totalWeight;
        private EncumberedStatusEffect _encumberedEffect;

        public void Initialize(Character character)
        {
            _character = character;
            _inventoryItems = new List<Item>();
            _equippedItems = character.GetEquipment().GetEquippedItems();
            _encumberedEffect = new EncumberedStatusEffect(_character);
            CalculateCarryCapacity();
        }
        
        // Returns how many of a specified item
        public int GetItemCount(Item item)
        {
            return _inventoryItems.Count(inventoryItem => inventoryItem == item);
        }

        public void AddItemToInventory(Item item)
        {
            if (item == null) return;

            if (!CanCarry(item))
            {
                Debug.LogWarning($"Cannot add item {item.Name}. Exceeds carry capacity.");
                _character.ApplyStatusEffect(_encumberedEffect);
                return;
            }
            _inventoryItems.Add(item);
            _totalWeight += item.Weight;
            Debug.Log($"Added item {item.Name} to the inventory.");
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (item == null || !_inventoryItems.Contains(item)) return;

            _inventoryItems.Remove(item);
            _totalWeight -= item.Weight;
            Debug.Log($"Removed item {item.Name} from the inventory.");
            if (!IsEncumbered())
            {
                _character.RemoveStatusEffect(_encumberedEffect);
            }
        }

        public void EquipItem(Item item)
        {
            if (item == null || !_inventoryItems.Contains(item)) return;

            if (_equippedItems.ContainsValue(item))
            {
                Debug.LogWarning($"Item {item.Name} is already equipped.");
                return;
            }
            // Check if the item is a two-handed weapon and unequip the offhand item if necessary
            if (item.IsTwoHanded && _equippedItems.ContainsKey(EquipmentSlot.OffHand))
            {
                Item currentOffhandItem = _equippedItems[EquipmentSlot.OffHand];
                UnequipItem(currentOffhandItem);
            }
            // Check if the item is a one-handed weapon and there is a similar item in the opposite hand
            if (!item.IsTwoHanded && item.Slot == EquipmentSlot.MainHand && _equippedItems.ContainsKey(EquipmentSlot.OffHand))
            {
                Item currentOffhandItem = _equippedItems[EquipmentSlot.OffHand];
                if (currentOffhandItem.Slot == EquipmentSlot.OffHand && currentOffhandItem.Name == item.Name)
                {
                    UnequipItem(currentOffhandItem);
                }
            }
            _inventoryItems.Remove(item);
            _equippedItems[item.Slot] = item;
            Debug.Log($"Equipped item {item.Name}.");
            ApplyItemBonuses(item);
        }

        public void UnequipItem(Item item)
        {
            if (item == null || !_equippedItems.ContainsValue(item)) return;

            _equippedItems.Remove(item.Slot);
            _inventoryItems.Add(item);
            Debug.Log($"Unequipped item {item.Name}.");
            RemoveItemBonuses(item);
        }

        private bool CanCarry(Item item)
        {
            return _totalWeight + item.Weight <= _carryCapacity;
        }

        private void CalculateCarryCapacity()
        {
            // Calculate the base carry capacity based on the character's strength stat
            float baseCarryCapacity = _character.Stats.Strength.Value;
            // Apply logarithmic growth to the base carry capacity
            const float scalingFactor = 10f; // Adjust this value based on your desired scaling
            float scaledCarryCapacity = scalingFactor * Mathf.Log(baseCarryCapacity + 1);
            _carryCapacity = scaledCarryCapacity;
        }

        private bool IsEncumbered()
        {
            return _totalWeight > _carryCapacity;
        }

        private void ApplyItemBonuses(Item item)
        {
            foreach (KeyValuePair<string, float> bonus in item.StatModifiers)
            {
                if (!_character.Stats.StatMap.ContainsKey(bonus.Key)) continue;

                dynamic statProperty = _character.Stats.StatMap[bonus.Key];
                statProperty.Increase(bonus.Value);
            }
        }

        private void RemoveItemBonuses(Item item)
        {
            foreach (KeyValuePair<string, float> bonus in item.StatModifiers)
            {
                if (!_character.Stats.StatMap.ContainsKey(bonus.Key)) continue;

                dynamic statProperty = _character.Stats.StatMap[bonus.Key];
                statProperty.Decrease(bonus.Value);
            }
        }
    }
}
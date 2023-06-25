// Character.cs - An abstract class for creating character entities in a RPG system.
// Version 1.1.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Character abstract class serves as a base for creating character entities in a role-playing game (RPG) system.
// It provides properties and methods for managing the character's name, level, stats, and experience. It also includes
// functionality for taking damage, healing, attacking, defending, managing equipment and inventory, applying status
// effects, and handling character proficiencies. The class implements the leveling system by calculating base stats
// based on the character's level using a logarithmic formula. Derived classes can override methods to provide specific
// behavior for different character types (e.g., warrior, mage, etc.).
//
// Version History:
// - 1.0.0: Initial version
// - 1.1.0: Added equipment and inventory management, status effects, and character proficiencies.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using UnityEngine;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base
{
    public abstract class Character : MonoBehaviour
    {
        public string Name { get; set; }
        public int Level { get; private set; }
        public Stats Stats { get; private set; }
        public Experience CharacterExperience { get; set; }
        public Race CharacterRace { get; set; }
        private IBaseStatCalculator _baseStatCalculator;
        private Inventory _inventory;
        private Equipment _equipment;
        private List<string> _proficiencies;
        private List<StatusEffect> _statusEffects;

        private void OnDestroy()
        {
            CharacterExperience.OnLevelUp -= HandleLevelUp;
        }
        
        private void Update()
        {
            UpdateStatusEffects(Time.deltaTime);
        }

        public Character(string name, int level, Race race, IBaseStatCalculator calculator)
        {
            Initialize(name, level, race, calculator);
        }

        public void Initialize(string cName, int level, Race race, IBaseStatCalculator baseStatCalculator)
        {
            Name = cName;
            Level = Mathf.Max(1, level);
            Stats = new Stats();
            _proficiencies = new List<string>();
            CharacterExperience = new Experience();
            CharacterExperience.OnLevelUp += HandleLevelUp;
            CharacterRace = race;
            _baseStatCalculator = baseStatCalculator;
            ApplyRaceBonuses();
            _equipment = new Equipment();
            _equipment.Initialize(this);
            _statusEffects = new List<StatusEffect>();
            _inventory = new Inventory();
            _inventory.Initialize(this);
        }

        public Equipment GetEquipment()
        {
            return _equipment;
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }

        public virtual void RestoreMana(int amount)
        {
            Stats.CurrentMana.Value = Mathf.Clamp(Stats.CurrentMana.Value + amount, 0, Stats.CurrentMana.Value);
        }

        public virtual void Attack() { }

        public virtual void Defend() { }

        private void HandleLevelUp(int newLevel)
        {
            Level = newLevel;
            SetBaseStats();
        }

        private void SetBaseStats()
        {
            _baseStatCalculator.CalculateBaseStats(Stats, Level);
        }

        private void ApplyRaceBonuses()
        {
            foreach (KeyValuePair<string, float> bonus in CharacterRace.StatBonuses)
            {
                if (!Stats.StatMap.ContainsKey(bonus.Key)) continue;

                dynamic statProperty = Stats.StatMap[bonus.Key];
                statProperty.Increase(bonus.Value);
            }
        }

        public void EquipItem(Item item)
        {
            _equipment.EquipItem(item);
        }

        public void UnequipItem(Item item)
        {
            _equipment.UnequipItem(item);
        }

        public void AddItemToInventory(Item item)
        {
            _inventory.AddItemToInventory(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            _inventory.RemoveItemFromInventory(item);
        }

        public bool HasProficiency(string proficiencyName)
        {
            return _proficiencies.Contains(proficiencyName);
        }

        public void AddProficiency(string proficiencyName)
        {
            if (!_proficiencies.Contains(proficiencyName))
            {
                _proficiencies.Add(proficiencyName);
            }
        }

        public void RemoveProficiency(string proficiencyName)
        {
            if (_proficiencies.Contains(proficiencyName))
            {
                _proficiencies.Remove(proficiencyName);
            }
        }
        
        private void UpdateStatusEffects(float deltaTime)
        {
            for (int i = _statusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect effect = _statusEffects[i];
                effect.Update(deltaTime);
                if (!effect.IsActive)
                {
                    _statusEffects.RemoveAt(i);
                }
            }
        }
        
        public void ApplyStatusEffect(StatusEffect effect)
        {
            if (_statusEffects.Contains(effect)) return;
            
            effect.Apply();
            _statusEffects.Add(effect);
        }

        public void RemoveStatusEffect(StatusEffect effect)
        {
            if (!_statusEffects.Contains(effect)) return;
            
            effect.Remove();
            _statusEffects.Remove(effect);
        }

        public bool HasEffect(StatusEffect effect)
        {
            return _statusEffects.Contains(effect);
        }
    }
}
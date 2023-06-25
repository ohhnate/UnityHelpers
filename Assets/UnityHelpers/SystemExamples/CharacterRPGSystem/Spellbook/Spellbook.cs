// Spellbook.cs - A class representing a spellbook for a character in a game.
// Version 1.1.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Spellbook class represents a spellbook associated with a character in a game. It allows the character to store and cast different spells.
// The class maintains a list of spells and provides methods to add, remove, and cast spells from the spellbook.
// Each spell has a type, level, mana cost, and associated element.
// The spellbook ensures that a spell can only be cast if the character has enough mana for its mana cost.
// The class also provides a method to list the spells currently in the spellbook.
//
// The Spell class represents an individual spell in the spellbook. It contains properties for the spell's type, level, mana cost, damage amount, heal amount, and element.
// The Spell class also provides a static method to retrieve the name of a spell based on its type.
//
// Version History:
// - 1.0.0 (Initial Version)
//   - Basic functionality of spellbook class with spell management and casting.
// - 1.1.0
//   - Added damage and healing properties to the Spell class.
//   - Renamed the `Element` enum member "IceMissile" to "IceMissile" for consistency.
//   - Updated the `GetSpellElement` method to use the renamed "IceMissile" enum member.
//   - Added the `SpellNameAttribute` to provide custom spell names for each `SpellType`.
//
// No accreditation is required, but it would be appreciated.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Spellbook
{
    [CreateAssetMenu(fileName = "Spellbook", menuName = "Spells/Spellbook")]
    public class Spellbook : ScriptableObject
    {
        [field: SerializeField]
        public string DisplayName { get; private set; }
        [SerializeField]
        private List<Spell> spells;
        private readonly Character _owner;

        public Spellbook(Character owner)
        {
            _owner = owner;
            spells = new List<Spell>();
        }

        public void AddSpell(Spell spell)
        {
            foreach (Spell s in spells)
            {
                if (s.Type == spell.Type)
                {
                    Debug.LogWarning("Spell is already in the spell book");
                    return;
                }
            }
            spells.Add(spell);
        }

        public void RemoveSpell(Spell spell)
        {
            spells.Remove(spell);
        }

        public void CastSpell(SpellType spellType)
        {
            Spell spell = spells.FirstOrDefault(s => s.Type == spellType);
            if (spell == null)
            {
                Debug.Log($"Spell '{Spell.GetSpellName(spellType)}' not found in the spellbook.");
                return;
            }
            if (_owner.Stats.CurrentMana.Value < spell.ManaCost) return;
            
            _owner.Stats.CurrentMana.Decrease(spell.ManaCost);
            spell.Cast();
        }

        public void ListSpells()
        {
            Debug.Log("Spellbook Contents:");
            foreach (Spell spell in spells)
            {
                Debug.Log($"- {Spell.GetSpellName(spell.Type)} (Level {spell.LevelRequirement})");
            }
        }
    }

    [Serializable]
    public class Spell
    {
        [field: SerializeField]
        public SpellType Type { get; private set; }
        [field: SerializeField]
        public int LevelRequirement { get; private set; }
        [field: SerializeField]
        public int ManaCost { get; private set; }
        [field: SerializeField]
        public int DamageAmount { get; private set; }
        [field: SerializeField]
        public Element Element { get; private set; }
        [field: SerializeField]
        public int HealAmount { get; private set; }

        private static readonly Dictionary<SpellType, Element> SpellTypeElements = new()
        {
            { SpellType.Fireball, Element.Fire },
            { SpellType.Firebolt, Element.Fire },
            { SpellType.Heal, Element.None },
            { SpellType.IceMissile, Element.Ice },
            { SpellType.Icebolt, Element.Ice }
        };

        public Spell(SpellType type, int levelRequirement, int manaCost, int damageAmount, int healAmount)
        {
            Type = type;
            LevelRequirement = levelRequirement;
            ManaCost = manaCost;
            Element = GetSpellElement(type);
            DamageAmount = damageAmount;
            HealAmount = healAmount;
        }
        
        private static Element GetSpellElement(SpellType type)
        {
            return SpellTypeElements.ContainsKey(type) ? SpellTypeElements[type] : Element.None;
        }

        public void Cast()
        {
            // Perform the casting logic for the spell
            // Implement Spell casting logic here
            Debug.Log($"Casting {GetSpellName(Type)} (Level {LevelRequirement})");
        }
        
        public static string GetSpellName(SpellType spellType)
        {
            MemberInfo[] memberInfo = typeof(SpellType).GetMember(spellType.ToString());
            SpellNameAttribute attribute = memberInfo[0].GetCustomAttributes(typeof(SpellNameAttribute), false).FirstOrDefault() as SpellNameAttribute;
            return attribute?.Name;
        }
    }
    
    [Serializable]
    public enum Element
    {
        None,
        Fire,
        Ice,
        Lightning
    }
    
    [Serializable]
    public enum SpellType
    {
        [SpellName("Fireball")]
        Fireball,
        [SpellName("Firebolt")]
        Firebolt,
        [SpellName("Heal")]
        Heal,
        [SpellName("Ice Missle")]
        IceMissile,
        [SpellName("Icebolt")]
        Icebolt
    }

    public class SpellNameAttribute : Attribute
    {
        public string Name { get; }

        public SpellNameAttribute(string name)
        {
            Name = name;
        }
    }
}
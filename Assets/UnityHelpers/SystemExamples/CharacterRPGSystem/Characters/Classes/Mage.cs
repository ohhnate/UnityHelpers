// Mage.cs - A class representing a mage character in a RPG system.
// Version 1.1.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Mage class extends the Character class and represents a mage character in a role-playing game (RPG) system.
// It inherits properties and methods from the base Character class and adds specific functionality and customization
// for the mage character type. The class overrides the `SetBaseStats` method to provide a customized calculation
// for the mage's base stats, taking into account the intelligence modifier specific to mages. Additionally, the class
// provides empty implementations for the abstract methods inherited from the Character class, allowing further
// customization and implementation for mage-specific actions such as taking damage, healing, attacking, and defending.
// The class also includes a Spellbook instance, which enables the mage to cast spells using the associated spellbook.
//
// Version History:
// - 1.0.0: Initial version
// - 1.1.0: Added Spellbook instance and CastSpell method.
//
// No accreditation is required, but it would be appreciated.

using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Spellbook;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Classes
{
    public class Mage : Character
    {
        private readonly Spellbook.Spellbook _spellbook;

        public Mage(string cName, int level, Race race, IBaseStatCalculator calculator) : base(cName, level, race, calculator)
        {
            _spellbook = ScriptableObject.CreateInstance<Spellbook.Spellbook>();
        }
        
        public override void RestoreMana(int amount) { }

        public override void Attack() { }

        public void CastSpell()
        {
            _spellbook.CastSpell(SpellType.Fireball);
        }

        public override void Defend() { }
    }
}
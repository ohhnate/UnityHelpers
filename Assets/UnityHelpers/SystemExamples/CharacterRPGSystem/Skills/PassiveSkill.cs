// PassiveSkill.cs - A class representing a passive skill in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The PassiveSkill class extends the Skill class and represents a passive skill in a game. Passive skills are persistent abilities that provide ongoing effects
// or modifications to the character without the need for activation. The class provides methods to apply and remove passive effects from the character.
// Subclasses can override the `ApplyPassiveEffects` and `RemovePassiveEffects` methods to implement specific passive effects logic for the skill.
//
// No accreditation is required, but it would be appreciated.

using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Skills
{
    public class PassiveSkill : Skill
    {
        protected PassiveSkill(Character character, string name, string description, int levelRequirement) : base(character, name, description, levelRequirement) 
        { }

        public virtual void ApplyPassiveEffects(Character character)
        {
            // Implement the specific passive effects logic for the skill
            // Modify character's stats, behaviors, or other properties as needed
        }

        public virtual void RemovePassiveEffects(Character character)
        {
            // Implement the logic to remove the passive effects from the character
            // Revert any modifications made to the character's stats or behaviors
        }
    }
}
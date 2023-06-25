// ActiveSkill.cs - A class representing an active skill in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The ActiveSkill class extends the Skill class and represents an active skill in a game. Active skills are abilities that can be manually activated
// by the player or an AI-controlled character. The class provides methods to activate and deactivate the skill, as well as a cooldown duration
// for managing the skill's usage frequency. Subclasses can override the `Activate` and `Deactivate` methods to implement specific logic for the
// skill's active effect and cleanup, respectively.
//
// No accreditation is required, but it would be appreciated.

using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Skills
{
    public class ActiveSkill : Skill
    {
        public float Cooldown { get; } = 10f;

        protected ActiveSkill(Character character, string name, string description, int levelRequirement) : base(character, name, description, levelRequirement) { }

        public override void Activate(Character character)
        {
            // Perform the skill's active effect
        }

        public override void Deactivate(Character character)
        {
            // Perform any cleanup or deactivation logic
        }
    }
}
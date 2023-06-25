// Warrior.cs - A class representing a warrior character in a RPG system.
// Version 1.1.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Warrior class extends the Character class and represents a warrior character in a role-playing game (RPG) system.
// It inherits properties and methods from the base Character class and adds specific functionality and customization
// for the warrior character type. The class overrides the `SetBaseStats` method to provide a customized calculation
// for the warrior's base stats, taking into account the strength and health modifiers specific to warriors. Additionally,
// the class provides empty implementations for the abstract methods inherited from the Character class, allowing further
// customization and implementation for warrior-specific actions such as taking damage, healing, attacking, and defending.
// The Warrior class introduces a skill tree and provides methods for learning, activating, and deactivating skills. It
// also includes a method for applying passive skills, which are skills that provide ongoing effects or bonuses to the
// warrior. The class includes examples of two specific skills: "Second Wind" and "Whirlwind". The "Second Wind" skill
// is a passive skill that restores health when the warrior's health is below a certain threshold. The "Whirlwind" skill
// is an active skill that unleashes a powerful spinning attack, hitting all nearby enemies. Derived classes can further
// customize and add additional skills specific to the warrior character type.
//
// Version History:
// - 1.0.0: Initial version
// - 1.1.0: Added skill tree, skill learning, activation, and deactivation, as well as passive and active skill examples.
//
// No accreditation is required, but it would be appreciated.

using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Skills;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Classes
{
    public class Warrior : Character
    {
        private readonly SkillTree _skillTree;

        public Warrior(string cName, int level, Race race, IBaseStatCalculator calculator) : base(cName, level, race, calculator)
        {
            _skillTree = new SkillTree(new SkillTree.SkillNode(new Skill(this,"Root Skill", "This is the root skill", 0)));
            LearnSkills();
        }

        private void LearnSkills()
        {
            // Create instances of the skills
            SecondWindSkill secondWindSkill = new(this,"Second Wind", "Restores health when below a certain threshold.", 5);
            WhirlwindSkill whirlwindSkill = new(this,"Whirlwind", "Unleash a powerful spinning attack, hitting all nearby enemies.", 10);
            // Learn the skills by adding them to the skill tree
            SkillTree.SkillNode secondWindNode = new(secondWindSkill);
            _skillTree.AddSkillNode(secondWindNode);
            SkillTree.SkillNode whirlwindNode = new(whirlwindSkill);
            _skillTree.AddSkillNode(whirlwindNode);
        }

        public void ActivateSkill(string skillName)
        {
            SkillTree.SkillNode skillNode = _skillTree.GetSkillNodeByName(_skillTree.Root, skillName);
            if (skillNode is { IsUnlocked: true, Skill: ActiveSkill activeSkill })
            {
                activeSkill.Activate(this);
            }
        }

        public void DeactivateSkill(string skillName)
        {
            SkillTree.SkillNode skillNode = _skillTree.GetSkillNodeByName(_skillTree.Root, skillName);
            if (skillNode is { IsUnlocked: true, Skill: ActiveSkill activeSkill })
            {
                activeSkill.Deactivate(this);
            }
        }

        public void ApplyPassiveSkills()
        {
            foreach (SkillTree.SkillNode skillNode in _skillTree.UnlockedSkills)
            {
                if (skillNode.Skill is PassiveSkill passiveSkill)
                {
                    passiveSkill.ApplyPassiveEffects(this);
                }
            }
        }
    }
    
    public class SecondWindSkill : PassiveSkill
    {
        public int HealingAmount { get; } = 50;

        public SecondWindSkill(Character character, string name, string description, int levelRequirement) : base(character, name, description, levelRequirement) { }

        public override void ApplyPassiveEffects(Character character)
        {
            base.ApplyPassiveEffects(character);

            // Apply the SecondWind effect, such as healing the character
            character.Stats.Health.Heal(HealingAmount);
        }

        public override void RemovePassiveEffects(Character character)
        {
            base.RemovePassiveEffects(character);

            // Implement logic to remove the SecondWind effect if needed
        }
    }
    
    public class WhirlwindSkill : ActiveSkill
    {
        public int Damage { get; } = 30;
        public float Radius { get; } = 5f;
        
        public WhirlwindSkill(Character character, string name, string description, int levelRequirement) : base(character, name, description, levelRequirement) { }

        public override void Activate(Character character)
        {
            // Perform the Whirlwind attack, damaging nearby enemies within the specified radius
            // You can implement the specific logic for the Whirlwind attack here
        }

        public override void Deactivate(Character character)
        {
            // Stop the Whirlwind attack and perform any necessary cleanup
        }
    }
}
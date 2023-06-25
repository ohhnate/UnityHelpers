// SkillTree.cs - An abstract class representing a skill tree in a game.
// Version 1.1.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The SkillTree class represents a skill tree in a game, allowing players to unlock and progress through different skills and abilities.
// It provides a hierarchical structure of interconnected skill nodes. Each skill node has a name, description, and a list of child and parent nodes.
// The class keeps track of unlocked skills and provides methods to unlock new skills based on the prerequisites of parent skills.
// When a skill is unlocked, it is automatically connected to other unlocked skills by adding it as a child to compatible parent skills.
// The SkillTree class also offers a way to retrieve a list of available skills that can be unlocked based on the currently unlocked skills.
//
// Version History:
// - 1.0.0: Initial version
// - 1.1.0: Added CanUnlock and GetAvailableSkills methods.
//
// No accreditation is required, but it would be appreciated.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Skills
{
    public class SkillTree
    {
        public class SkillNode
        {
            public List<SkillNode> Children { get; } = new List<SkillNode>();
            public List<SkillNode> Parents { get; } = new List<SkillNode>();
            public bool IsUnlocked { get; set; }
            public Skill Skill { get; }

            public SkillNode(Skill skill)
            {
                Skill = skill;
            }

            public void AddChild(SkillNode child)
            {
                Children.Add(child);
                child.Parents.Add(this);
            }
        }

        public SkillNode Root { get; }
        public List<SkillNode> UnlockedSkills { get; } = new List<SkillNode>();

        public SkillTree(SkillNode root)
        {
            Root = root;
        }

        public void AddSkillNode(SkillNode skillNode)
        {
            int levelRequirement = skillNode.Skill.LevelRequirement;
            // If the skill tree is empty, add the node as the root
            if (UnlockedSkills.Count == 0)
            {
                UnlockedSkills.Add(skillNode);
                skillNode.IsUnlocked = true; // Unlock level 1 skills by default
                return;
            }
            // Find the appropriate parent node to attach the skill node
            SkillNode parentNode = FindParentNode(levelRequirement);
            // Add the skill node to the parent node's children
            parentNode.AddChild(skillNode);
            // Unlock the skill node if the player meets the level requirement
            if (levelRequirement <= 1)
            {
                skillNode.IsUnlocked = true;
            }
            // Add the skill node to the unlocked skills list
            UnlockedSkills.Add(skillNode);
        }

        private SkillNode FindParentNode(int levelRequirement)
        {
            // Use a breadth-first search to find the parent node with the highest level requirement
            Queue<SkillNode> queue = new Queue<SkillNode>();
            queue.Enqueue(Root);
            SkillNode parentNode = Root;
            while (queue.Count > 0)
            {
                SkillNode currentNode = queue.Dequeue();
                // Update the parent node if the current node's level requirement is less than or equal to the new skill's requirement
                if (currentNode.Skill.LevelRequirement <= levelRequirement)
                {
                    parentNode = currentNode;
                }
                foreach (SkillNode child in currentNode.Children)
                {
                    queue.Enqueue(child);
                }
            }
            return parentNode;
        }

        public SkillNode GetSkillNodeByName(SkillNode currentNode, string skillName)
        {
            if (currentNode.Skill.Name == skillName)
            {
                return currentNode;
            }
            foreach (SkillNode child in currentNode.Children)
            {
                SkillNode foundNode = GetSkillNodeByName(child, skillName);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null; // Skill node not found
        }

        public bool UnlockSkill(SkillNode skill, int playerLevel)
        {
            if (!CanUnlock(skill, playerLevel)) return false;

            skill.IsUnlocked = true;
            UnlockedSkills.Add(skill);
            foreach (SkillNode unlockedSkill in UnlockedSkills)
            {
                if (unlockedSkill != skill && !unlockedSkill.Children.Contains(skill))
                {
                    unlockedSkill.AddChild(skill);
                }
            }
            return true;
        }

        public bool CanUnlock(SkillNode skill, int playerLevel)
        {
            return skill.Skill.LevelRequirement <= playerLevel && skill.Parents.All(parent => UnlockedSkills.Contains(parent));
        }

        public List<SkillNode> GetAvailableSkills()
        {
            List<SkillNode> availableSkills = new List<SkillNode>();
            foreach (SkillNode skill in UnlockedSkills)
            {
                foreach (SkillNode child in skill.Children)
                {
                    if (!UnlockedSkills.Contains(child) && !availableSkills.Contains(child))
                    {
                        availableSkills.Add(child);
                    }
                }
            }
            return availableSkills;
        }
    }

    [Serializable]
    public class Skill
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField, TextArea]
        public string Description { get; private set; }
        [field: SerializeField]
        public int LevelRequirement { get; private set; }
        private Character _character;

        public Skill(Character character, string name, string description, int levelRequirement)
        {
            _character = character;
            Name = name;
            Description = description;
            LevelRequirement = levelRequirement;
        }

        public virtual void Activate(Character character)
        {
            // Implement skill activation logic here
        }

        public virtual void Deactivate(Character character)
        {
            // Implement skill deactivation logic here
        }
    }
}
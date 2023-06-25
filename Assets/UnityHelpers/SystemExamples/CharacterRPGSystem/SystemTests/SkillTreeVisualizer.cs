// SkillTree.cs - A MonoBehaviour class representing a visualization of creating random skill trees
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Skills;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.SystemTests
{
    public class SkillTreeVisualizer : MonoBehaviour
    {
        [SerializeField, Tooltip("reference to skills")]
        private List<Skill> skills = new();
        
        private void Awake()
        {
            // Create the root skill node
            SkillTree.SkillNode rootSkillNode = new(skills[0]);
            // Create the skill tree with the root node
            SkillTree skillTree = new(rootSkillNode);
            // Add the remaining skills to the skill tree
            foreach (Skill skill in skills)
            {
                SkillTree.SkillNode skillNode = new(skill);
                skillTree.AddSkillNode(skillNode);
            }
            // Visualize the skill tree
            VisualizeSkillTree(rootSkillNode);
        }

        private void VisualizeSkillTree(SkillTree.SkillNode skillNode, Transform parentTransform = null, int level = 0)
        {
            // Create the top-level "SkillTree" object if it doesn't exist
            if (parentTransform == null)
            {
                GameObject skillTreeObject = new GameObject("SkillTree");
                parentTransform = skillTreeObject.transform;
            }

            // Instantiate a GameObject to represent the skill node
            GameObject skillNodeObject = new GameObject(skillNode.Skill.Name);
            skillNodeObject.transform.SetParent(parentTransform);

            // Position the skill node horizontally based on its level
            float horizontalPosition = level * 2f; // Adjust the spacing between levels as needed
            skillNodeObject.transform.localPosition = new Vector3(horizontalPosition, 0f, 0f);

            // Recursively visualize the children of the skill node
            foreach (SkillTree.SkillNode childNode in skillNode.Children)
            {
                VisualizeSkillTree(childNode, skillNodeObject.transform, level + 1);
            }
        }
    }
}
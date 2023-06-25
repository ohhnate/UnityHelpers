// Experience.cs - A class representing the experience and leveling system in a RPG.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Experience class represents the experience and leveling system in a role-playing game (RPG).
// It tracks the current experience points and the amount of experience required to level up.
// The class provides a method to gain experience points, which increases the current experience.
// When the current experience reaches or exceeds the required experience to level up, the class triggers
// a level up event, allowing other components to handle the level up logic. The level up event passes the new level as a parameter.
// The class also includes methods to calculate the current level and the required experience to level up based on a given level.
// The experience thresholds follow a logarithmic progression, where the required experience increases as the level grows.
//
// No accreditation is required, but it would be appreciated.

using System;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public class Experience
    {
        public int CurrentExperience { get; private set; }
        public int ExperienceToLevelUp { get; private set; }
        // Pass the new level to the event
        public event Action<int> OnLevelUp;

        public Experience()
        {
            CurrentExperience = 0;
            ExperienceToLevelUp = CalculateExperienceToLevelUp(1);
        }

        public void GainExperience(int amount)
        {
            CurrentExperience += amount;
            if (CurrentExperience >= ExperienceToLevelUp)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            // Increase level and update experience properties
            int currentLevel = CalculateCurrentLevel();
            CurrentExperience -= ExperienceToLevelUp;
            ExperienceToLevelUp = CalculateExperienceToLevelUp(currentLevel + 1);
            OnLevelUp?.Invoke(currentLevel + 1);
        }

        private int CalculateCurrentLevel()
        {
            // Calculate and return the current level based on experience thresholds
            // You can implement your own logic here
            return (int)Math.Floor(Math.Sqrt(CurrentExperience / 100f));
        }
        
        private static int CalculateExperienceToLevelUp(int level)
        {
            // Constants for experience calculation
            const int baseExperience = 100;
            const float experienceMultiplier = 1.5f; // Adjust this value to control the rate of experience growth
            // Logarithmic progression
            // Calculate and return the required experience to level up based on the given level
            return (int)(baseExperience * Math.Pow(experienceMultiplier, level - 1));
        }
    }
}
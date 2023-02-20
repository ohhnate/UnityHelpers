// Quest.cs - A class representing a quest with objectives and achievements.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Quest class provides methods for managing a quest, including adding objectives and achievements, tracking progress, and marking
// the quest as completed. Quests are initialized with a title and description, and can be marked as completed when all objectives have been
// completed. Quests maintain a list of objectives, which are represented by the Objective class, and a list of achievements, which are
// represented by the AchievementReference class. Achievements associated with the completion of the quest itself are represented by the
// Achievement class, which is stored in the _achievement field. The AchievementHandler instance is used to register and retrieve achievements.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;
using System.Linq;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class Quest
    {
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; set; }
        public List<IHasAchievement> Achievements { get; }
        private readonly Achievement _achievement;
        public List<Objective> Objectives { get; } = new();

        /// <summary>
        /// Initializes a new instance of the Quest class with the specified title and description.
        /// </summary>
        /// <param name="title">The title of the quest.</param>
        /// <param name="description">The description of the quest.</param>
        public Quest(string title, string description)
        {
            Title = title;
            Description = description;
            IsCompleted = false;
            _achievement = new Achievement(Title + " Completed", "Complete the " + Title + " quest!");
            Achievements = new List<IHasAchievement>();
            AchievementHandler.Instance.RegisterAchievement(_achievement);
        }
    
        public void AddObjective(Objective objective)
        {
            Objectives.Add(objective);
        }
    
        public void AddAchievement(IHasAchievement achievement)
        {
            Achievements.Add(achievement);
        }

        public int GetObjectiveProgress()
        {
            return Objectives.Count(objective => objective.IsCompleted());
        }
    
        public int GetObjectiveCount()
        {
            return Objectives.Count;
        }

        /// <summary>
        /// Marks the quest as completed if all objectives have been completed, and updates associated achievements.
        /// </summary>
        public void MarkCompleted()
        {
            if (IsCompleted) return;
        
            IsCompleted = true;
            foreach (IHasAchievement achievementReference in Achievements)
            {
                achievementReference.GetAchievement()?.MarkCompleted();
            }
        }

        public Achievement GetAchievement()
        {
            return _achievement;
        }
    }
}
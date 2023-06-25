// ExampleQuest.cs - An example script for creating and starting a new quest in a Unity scene.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The ExampleQuest script demonstrates how to create a new quest with objectives and achievements, and then start it using the QuestHandler.
// In this example, the quest is called "Save the Princess" and has two objectives: defeat the dragon and find the princess.
// The quest also has two associated achievements: "Dragon Slayer" for defeating the dragon and "Knight in Shining Armor" for saving the princess.
//
// The script registers the achievements with the AchievementHandler and adds them to the quest using AchievementReferences.
// Finally, the script starts the quest using the QuestHandler.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class ExampleQuest : MonoBehaviour
    {
        private void Start()
        {
            CreateQuest();
        }
    
        private void CreateQuest()
        {
            // Creates a new Quest instance with the title "Save the Princess" and description "Rescue the princess from the evil dragon!"
            Quest myQuest = new Quest("Save the Princess", "Rescue the princess from the evil dragon!");

            // Defines two Objectives for the Quest, "Defeat the dragon" and "Find the princess", each with a goal of 1 completion.
            Objective defeatDragon = new Objective("Defeat the dragon", "Slay the fire-breathing dragon", 1);
            Objective findPrincess = new Objective("Find the princess", "Locate the princess in the dragon's lair", 1);

            myQuest.AddObjective(defeatDragon);
            myQuest.AddObjective(findPrincess);

            // Creates two Achievement instances, "Dragon Slayer" and "Knight in Shining Armor", with descriptions related to completing the Quest.
            Achievement defeatDragonAchievement = new Achievement("Dragon Slayer", "Defeat the evil dragon!");
            Achievement savePrincessAchievement = new Achievement("Knight in Shining Armor", "Save the princess from the dragon!");

            // Registers the Achievements with the AchievementHandler to track progress towards completing them.
            AchievementHandler.Instance.RegisterAchievement(defeatDragonAchievement);
            AchievementHandler.Instance.RegisterAchievement(savePrincessAchievement);

            // Adds references to the two Achievements to the Quest.
            myQuest.AddAchievement(new AchievementReference("Dragon Slayer"));
            myQuest.AddAchievement(new AchievementReference("Knight in Shining Armor"));
            
            // Make the created quest an available quest to start.
            QuestHandler.Instance.AddAvailableQuest(myQuest);
            // Starts the Quest through the QuestHandler.
            QuestHandler.Instance.StartQuest(myQuest);
        }
    }
}
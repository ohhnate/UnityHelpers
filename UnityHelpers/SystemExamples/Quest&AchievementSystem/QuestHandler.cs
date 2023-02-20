// QuestHandler.cs - A class for managing quests and their progress.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The QuestHandler class provides methods for starting and completing quests, tracking quest progress, and retrieving information
// about active, completed, and available quests. The class also handles dependencies between quests and maintains a quest log
// to track the player's progress. The class uses a Dictionary to store the quest progress and a List to store the active, completed,
// and available quests. The UpdateQuestProgress method recursively updates the progress of all quests that depend on a completed quest.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;
using UnityEngine;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class QuestHandler : Handler<QuestHandler>
    {
        private readonly List<Quest> _activeQuestsList = new();
        private readonly List<Quest> _completedQuestsList = new();
        private readonly Dictionary<Quest, int> _questProgress = new();
        private readonly List<Quest> _availableQuestsList = new();
        private readonly Dictionary<Quest, List<Quest>> _questDependenciesList = new();
        private readonly Dictionary<Quest, string> _questLog = new();

        public void StartQuest(Quest quest)
        {
            if (!_availableQuestsList.Contains(quest))
            {
                Debug.LogWarning($"Quest '{quest.Title}' is not available to start.");
                return;
            }
            _activeQuestsList.Add(quest);
            _availableQuestsList.Remove(quest);
            _questProgress[quest] = 0;

            Debug.Log($"Quest '{quest.Title}' started.");

            UpdateQuestLog(quest, $"Started quest '{quest.Title}'.");
            UpdateQuestProgress(quest);
        }

        public void CompleteQuest(Quest quest)
        {
            if (!_activeQuestsList.Contains(quest))
            {
                Debug.LogWarning($"Quest '{quest.Title}' is not active and cannot be completed.");
                return;
            }
            _activeQuestsList.Remove(quest);
            _completedQuestsList.Add(quest);

            Debug.Log($"Quest '{quest.Title}' completed.");

            UpdateQuestLog(quest, $"Completed quest '{quest.Title}'.");
            UpdateQuestProgress(quest);
            quest.MarkCompleted();
        }

        public bool IsQuestActive(Quest quest)
        {
            return _activeQuestsList.Contains(quest);
        }

        public bool IsQuestCompleted(Quest quest)
        {
            return _completedQuestsList.Contains(quest);
        }

        public int GetQuestProgress(Quest quest)
        {
            return !_activeQuestsList.Contains(quest) ? 0 : _questProgress[quest];
        }

        public List<Quest> GetActiveQuests()
        {
            return new List<Quest>(_activeQuestsList);
        }

        public List<Quest> GetCompletedQuests()
        {
            return new List<Quest>(_completedQuestsList);
        }

        public List<Quest> GetAvailableQuests()
        {
            return new List<Quest>(_availableQuestsList);
        }

        public List<Quest> GetDependencies(Quest quest)
        {
            return _questDependenciesList.ContainsKey(quest) ? _questDependenciesList[quest] : new List<Quest>();
        }

        public string GetQuestLog(Quest quest)
        {
            return _questLog.ContainsKey(quest) ? _questLog[quest] : "";
        }

        private void UpdateQuestProgress(Quest quest)
        {
            // Check if the quest has dependent quests and update their progress first
            if (_questDependenciesList.ContainsKey(quest))
            {
                foreach (Quest dependentQuest in _questDependenciesList[quest])
                {
                    UpdateQuestProgress(dependentQuest);
                }
            }
            // Check if the current quest is active
            if (!_activeQuestsList.Contains(quest)) return; // Do not update progress if the quest is not active
    
            int progress = quest.GetObjectiveProgress();
            // Check if the quest has been completed
            if (progress == quest.GetObjectiveCount())
            {
                CompleteQuest(quest);
            }
            else
            {
                // Update the quest progress
                _questProgress[quest] = progress;
            }
        }

        private void UpdateQuestLog(Quest quest, string message)
        {
            if (!_questLog.ContainsKey(quest))
            {
                _questLog[quest] = "";
            }
            _questLog[quest] += $"{message}\n";
        }
    }
}
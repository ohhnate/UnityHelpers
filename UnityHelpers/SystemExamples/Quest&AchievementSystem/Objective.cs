// Objective.cs - A class representing an objective with progress tracking and completion.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Objective class provides methods for managing an objective, including updating progress, tracking completion, and marking the objective
// as completed. Objectives are initialized with a title, description, and required progress. Progress is tracked with the Progress property,
// which can be updated using the UpdateProgress method. The objective is considered completed when its Progress property is equal to its
// RequiredProgress property. The IsCompleted method is provided for checking completion status.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class Objective
    {
        public string Title { get; }
        public string Description { get; }
        public int Progress { get; private set; }
        public int RequiredProgress { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Objective"/> class with the specified title, description, and required progress.
        /// </summary>
        /// <param name="title">The title of the objective.</param>
        /// <param name="description">The description of the objective.</param>
        /// <param name="requiredProgress">The amount of progress required to complete the objective.</param>
        public Objective(string title, string description, int requiredProgress)
        {
            Title = title;
            Description = description;
            RequiredProgress = requiredProgress;
            Progress = 0;
        }

        /// <summary>
        /// Updates the progress of the objective by the specified amount, and marks the objective as completed if the required progress is reached.
        /// </summary>
        /// <param name="amount">The amount by which to update the progress.</param>
        public void UpdateProgress(int amount)
        {
            Progress = Mathf.Clamp(Progress + amount, 0, RequiredProgress);
            if (Progress == RequiredProgress)
            {
                MarkCompleted();
            }
        }

        public void MarkCompleted()
        {
            Progress = RequiredProgress;
        }

        public bool IsCompleted()
        {
            return Progress >= RequiredProgress;
        }
    }
}
// Achievement.cs - A class for defining and tracking achievements.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Achievement class provides a simple structure for defining achievements. An achievement is defined by a title
// and a description and can be marked as completed with the MarkCompleted method. The class also provides an OnComplete event
// that can be subscribed to in order to trigger an action when the achievement is completed.
//
// No accreditation is required but it would be highly appreciated <3

using System;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class Achievement
    {
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; private set; }
        public event Action OnComplete;

        public Achievement(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public void MarkCompleted()
        {
            if (IsCompleted) return;
            
            IsCompleted = true;
            OnComplete?.Invoke();
        }
    }
}
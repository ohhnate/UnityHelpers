// AchievementHandler.cs - A class for managing achievements and their registration.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The AchievementHandler class provides methods for registering and retrieving achievements. It maintains a Dictionary to store all registered
// achievements and provides a GetAchievement method to retrieve a specific achievement by its title. Achievements are represented by the Achievement class.
//
// The AchievementReference class is an implementation of the IHasAchievement interface, which provides a GetAchievement method for returning
// the achievement associated with the reference. This allows other classes to reference achievements without requiring direct access to the
// AchievementHandler instance.
//
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;

namespace UnityHelpers.SystemExamples.Quest_AchievementSystem
{
    public class AchievementHandler : Handler<AchievementHandler>
    {
        private readonly Dictionary<string, Achievement> _achievements = new();

        /// <summary>
        /// Registers an achievement with the AchievementHandler. The achievement is added to the _achievements Dictionary
        /// if it is not already present.
        /// </summary>
        /// <param name="achievement">The achievement to be registered</param>
        public void RegisterAchievement(Achievement achievement)
        {
            if (!_achievements.ContainsKey(achievement.Title))
            {
                _achievements.Add(achievement.Title, achievement);
            }
        }

        /// <summary>
        ///  Returns the achievement with the specified title, or null if the achievement is not registered with the AchievementHandler.
        /// </summary>
        /// <param name="title">The title of the achievement</param>
        /// <returns></returns>
        public Achievement GetAchievement(string title)
        {
            return _achievements.TryGetValue(title, out Achievement achievement) ? achievement : null;
        }
    }
    
    /// <summary>
    /// The IHasAchievement interface is implemented by classes that need to reference an achievement. The interface provides a single method
    /// for retrieving the referenced achievement.
    /// </summary>
    public interface IHasAchievement
    {
        Achievement GetAchievement();
    }

    /// <summary>
    /// The AchievementReference class implements the IHasAchievement interface and provides a way to reference an achievement by its title.
    /// The GetAchievement method returns the achievement with the specified title by calling the GetAchievement method of the AchievementHandler class.
    /// </summary>
    internal class AchievementReference : IHasAchievement
    {
        private readonly string _title;

        public AchievementReference(string title)
        {
            _title = title;
        }
        
        /// <summary>
        /// // Returns the achievement with the specified title by calling the GetAchievement method of the AchievementHandler class.
        /// </summary>
        /// <returns></returns>
        public Achievement GetAchievement()
        {
            return AchievementHandler.Instance.GetAchievement(_title);
        }
    }
}
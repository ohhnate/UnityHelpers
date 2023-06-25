// Reputation.cs - A class representing reputation with factions in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Reputation class represents the reputation of a character with different factions in a game. It tracks the character's reputation with each
// faction as an integer value and provides methods to modify the reputation and calculate the reputation level based on predefined thresholds.
// The class also supports registering reputation calculators for different action types to customize the reputation changes for specific actions.
//
// The ActionType enum is used to define different types of actions that can affect reputation, such as completing quests, defeating enemies, or
// purchasing items. Additional action types can be added as needed.
//
// The IReputationCalculator interface defines the contract for reputation calculators. Reputation calculators implement the CalculateReputation
// method, which takes the current reputation as input and returns the reputation change for a specific action.
//
// The BaseReputationCalculator class is an abstract base class that provides a default implementation for reputation calculators. It calculates
// the reputation change based on a logarithmic function and exposes a protected abstract method to be implemented by derived classes for custom
// reputation calculations.
//
// The QuestCompletedRc, EnemyDefeatedRc, and ItemPurchasedRc classes are concrete implementations of reputation calculators for specific action
// types. They inherit from the BaseReputationCalculator class and override the CalculateReputationWithLog method to provide the specific
// reputation change formulas for completing quests, defeating enemies, and purchasing items, respectively.
//
// No accreditation is required, but it would be appreciated.

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters
{
    public class Reputation
    {
        private readonly Dictionary<string, int> _currentReputation;
        private readonly Dictionary<ActionType, IReputationCalculator> _reputationCalculators;
        // Define the reputation level thresholds and their corresponding labels
        private readonly Dictionary<int, string> _reputationLevels = new Dictionary<int, string>
        {
            { 100, "Paragon" },
            { 75, "Venerated" },
            { 25, "Respected" },
            { 0, "Neutral" },
            { -25, "Notorious" },
            { -50, "Infamous" }
        };

        public Reputation()
        {
            _currentReputation = new Dictionary<string, int>();
            _reputationCalculators = new Dictionary<ActionType, IReputationCalculator>();
        }

        // Returns the integer representation for a given faction
        private int GetReputation(string faction)
        {
            return _currentReputation.TryGetValue(faction, out int reputation) ? reputation : 0;
        }
        
        public string GetReputationLevel(string faction)
        {
            int reputation = GetReputation(faction);
            // Check if the reputation exceeds any of the defined thresholds
            foreach (KeyValuePair<int, string> level in _reputationLevels.OrderByDescending(l => l.Key))
            {
                if (reputation >= level.Key)
                {
                    return level.Value;
                }
            }
            // If no specific level is reached, the default is "Neutral"
            return "Neutral";
        }

        private void ModifyReputation(string faction, int amount)
        {
            if (!_currentReputation.ContainsKey(faction))
            {
                _currentReputation[faction] = 0;
            }
            _currentReputation[faction] += amount;
        }

        public void ModifyReputationWithMultiplier(string faction, int amount, float multiplier)
        {
            int modifiedAmount = (int)(amount * multiplier);
            ModifyReputation(faction, modifiedAmount);
        }

        public void RegisterReputationCalculator(ActionType actionType, IReputationCalculator calculator)
        {
            _reputationCalculators[actionType] = calculator;
        }

        public void CalculateReputationForAction(string faction, ActionType actionType)
        {
            int currentReputation = GetReputation(faction);
            //if action doesn't exist just use the base log calculation
            if (_reputationCalculators.TryGetValue(actionType, out IReputationCalculator calculator))
            {
                int reputationChange = calculator.CalculateReputation(currentReputation);
                ModifyReputation(faction, reputationChange);
            }
            else
            {
                // Use the base reputation calculation for unrecognized action types
                int reputationChange = CalculateBaseReputation(currentReputation);
                ModifyReputation(faction, reputationChange);
            }
        }

        private static int CalculateBaseReputation(int currentReputation)
        {
            // Increase reputation logarithmically
            double logReputation = Math.Log(currentReputation + 1, 10) + 1;
            return (int)(logReputation * 5); // Adjust the reputation change value as desired
        }
    }
    
    public enum ActionType
    {
        QuestCompleted,
        EnemyDefeated,
        ItemPurchased
        // Add more action types as needed
    }

    public interface IReputationCalculator
    {
        int CalculateReputation(int currentReputation);
    }

    public abstract class BaseReputationCalculator : IReputationCalculator
    {
        public int CalculateReputation(int currentReputation)
        {
            // Increase reputation logarithmically
            double logReputation = Math.Log(currentReputation + 1, 10) + 1;
            return CalculateReputationWithLog(logReputation);
        }

        protected abstract int CalculateReputationWithLog(double logReputation);
    }

    public class QuestCompletedRc : BaseReputationCalculator
    {
        protected override int CalculateReputationWithLog( double logReputation)
        {
            return (int)(logReputation * 50); // Reputation change for completing a quest
        }
    }

    public class EnemyDefeatedRc : BaseReputationCalculator
    {
        protected override int CalculateReputationWithLog(double logReputation)
        {
            return (int)(logReputation * 10); // Reputation change for defeating an enemy
        }
    }

    public class ItemPurchasedRc : BaseReputationCalculator
    {
        protected override int CalculateReputationWithLog(double logReputation)
        {
            return (int)(logReputation * 20); // Reputation change for purchasing an item
        }
    }
}
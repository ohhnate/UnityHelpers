// Stats.cs - A class representing the statistics of a character in an RPG system.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Stats class represents the various statistics (stats) of a character in a role-playing game (RPG) system.
// It provides properties for each individual stat, such as current health, maximum health, current mana, maximum mana,
// strength, intelligence, agility, stamina, physical defense, and magic defense. The class also includes methods to
// increase or decrease the value of a stat. The stats are represented using the generic `IStat<T>` interface and the
// concrete implementation `Stat<T>`. The `Stat<T>` class allows for dynamic manipulation of the stat values, such as
// increasing or decreasing them by a specified amount. The `Stats` class is designed to be inherited to provide
// customized implementations of stats for specific character types or roles in the game.
//
// The `Stats` class also includes a `Dictionary<string, dynamic>` property named `StatMap`, which provides a mapping of
// stat names to their corresponding `IStat<T>` properties. This allows for easy access to stats by name, which can be useful
// for dynamic stat manipulation or serialization.
//
// The `BaseStatCalculator` class is included as part of the `Stats` namespace. It is responsible for calculating the base
// stats for a character based on their level. It uses a logarithmic formula to determine the values of each stat. The
// `BaseStatCalculator` can be customized by adjusting the modifier values for each stat.
//
// The `IBaseStatCalculator` interface is provided as a contract for implementing custom base stat calculators.
//
// No accreditation is required, but it would be appreciated.

using System.Collections.Generic;
using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public class Stats
    {
        public Health Health { get; }
        public IStat<float> CurrentMana { get; }
        public IStat<float> MaxMana { get; }
        public IStat<float> Strength { get; }
        public IStat<float> Intelligence { get; }
        public IStat<float> Agility { get; }
        public IStat<float> Stamina { get; }
        public IStat<float> PhysicalDefense { get; }
        public IStat<float> MagicDefense { get; }
        public Dictionary<string, dynamic> StatMap { get; }

        public Stats()
        {
            Health = new Health();
            CurrentMana = new Stat<float>(0);
            MaxMana = new Stat<float>(0);
            Strength = new Stat<float>(0);
            Intelligence = new Stat<float>(0);
            Agility = new Stat<float>(0);
            Stamina = new Stat<float>(0);
            PhysicalDefense = new Stat<float>(0);
            MagicDefense = new Stat<float>(0);

            StatMap = new Dictionary<string, dynamic>
            {
                { "CurrentHealth", Health.CurrentHealth },
                { "MaxHealth", Health.MaxHealth },
                { "CurrentMana", CurrentMana },
                { "MaxMana", MaxMana },
                { "Strength", Strength },
                { "Intelligence", Intelligence },
                { "Agility", Agility },
                { "Stamina", Stamina },
                { "PhysicalDefense", PhysicalDefense },
                { "MagicDefense", MagicDefense }
            };
        }
    }

    public interface IStat<T>
    {
        public T Value { get; set; }
        public void Increase(T amount);
        public void Decrease(T amount);
    }

    public class Stat<T> : IStat<T>
    {
        public T Value { get; set; }

        public Stat(T value)
        {
            Value = value;
        }

        public void Increase(T amount)
        {
            dynamic currentValue = Value;
            dynamic incrementValue = amount;
            Value = currentValue + incrementValue;
        }

        public void Decrease(T amount)
        {
            dynamic currentValue = Value;
            dynamic decrementValue = amount;
            Value = currentValue - decrementValue;
        }
    }

    public class BaseStatCalculator : IBaseStatCalculator
    {
        private const float ModBase = 1;
        private const float LOGBase = 2;

        private readonly float _maxHealthModifier;
        private readonly float _maxManaModifier;
        private readonly float _intModifier;
        private readonly float _strModifier;
        private readonly float _agiModifier;
        private readonly float _staminaModifier;
        private readonly float _physicalDefenseModifier;
        private readonly float _magicDefenseModifier;

        public BaseStatCalculator(float maxHealthModifier = ModBase, float maxManaModifier = ModBase, float intModifier = ModBase, float strModifier = ModBase,
            float agiModifier = ModBase, float staminaModifier = ModBase, float physicalDefenseModifier = ModBase, float magicDefenseModifier = ModBase)
        {
            _maxHealthModifier = Mathf.Max(0, maxHealthModifier);
            _maxManaModifier = Mathf.Max(0, maxManaModifier);
            _intModifier = Mathf.Max(0, intModifier);
            _strModifier = Mathf.Max(0, strModifier);
            _agiModifier = Mathf.Max(0, agiModifier);
            _staminaModifier = Mathf.Max(0, staminaModifier);
            _physicalDefenseModifier = Mathf.Max(0, physicalDefenseModifier);
            _magicDefenseModifier = Mathf.Max(0, magicDefenseModifier);
        }

        public void CalculateBaseStats(Stats stats, int level)
        {
            // Calculate base stats for all stats using the desired logarithmic formula
            // Only for stats you want to change based on level
            // Some stats could change via items or buffs or whatever
            stats.Health.SetMaxHealth(Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _maxHealthModifier);
            stats.Health.SetHealth(stats.Health.MaxHealth.Value);
            stats.MaxMana.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _maxManaModifier;
            stats.CurrentMana.Value = stats.MaxMana.Value;
            stats.Intelligence.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _intModifier;
            stats.Strength.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _strModifier;
            stats.Agility.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _agiModifier;
            stats.Stamina.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _staminaModifier;
            stats.PhysicalDefense.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _physicalDefenseModifier;
            stats.MagicDefense.Value = Mathf.RoundToInt(Mathf.Log(level + 1, LOGBase)) * _magicDefenseModifier;
        }
    }

    public interface IBaseStatCalculator
    {
        public void CalculateBaseStats(Stats stats, int level);
    }
}
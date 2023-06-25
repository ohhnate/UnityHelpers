// Health.cs - A class representing the health of a character in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The Health class represents the health of a character in a game. It tracks the current health and maximum health as IStat<float> objects,
// which allows for easy modification and event handling. The class provides methods to increase or decrease the maximum health, take damage,
// heal, and set the health and maximum health to specific values. It also exposes events for health changes and death.
//
// The class relies on the IStat<T> interface and Stat<T> class to encapsulate the current health and maximum health values. This allows for
// additional functionality and abstraction, such as clamping the health values to valid ranges and invoking events when they change.
//
// The Health class is part of a larger character RPG system and is designed to be used in conjunction with other classes and systems in that
// context.
//
// No accreditation is required, but it would be appreciated.

using System;
using UnityEngine;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters
{
    public class Health
    {
        public IStat<float> CurrentHealth { get; private set; }
        public IStat<float> MaxHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action OnDeath;

        public Health()
        {
            CurrentHealth = new Stat<float>(0);
            MaxHealth = new Stat<float>(0);
        }

        public void IncreaseMaxHealth(float amount)
        {
            MaxHealth.Increase(amount);
            CurrentHealth.Increase(amount);
            OnHealthChanged?.Invoke(CurrentHealth.Value);
        }

        public void DecreaseMaxHealth(float amount)
        {
            MaxHealth.Decrease(amount);
            CurrentHealth.Decrease(amount);
            OnHealthChanged?.Invoke(CurrentHealth.Value);
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth.Decrease(amount);
            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0, MaxHealth.Value);
            OnHealthChanged?.Invoke(CurrentHealth.Value);
            if (CurrentHealth.Value <= 0)
            {
                OnDeath?.Invoke();
            }
        }
        
        public void Heal(float amount)
        {
            CurrentHealth.Increase(amount);
            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0, MaxHealth.Value);
            OnHealthChanged?.Invoke(CurrentHealth.Value);
        }
        
        public void SetHealth(float amount)
        {
            CurrentHealth.Value = Mathf.Clamp(amount, 0, MaxHealth.Value);
        }
        
        public void SetMaxHealth(float amount)
        {
            MaxHealth.Value = amount;
        }
    }
}
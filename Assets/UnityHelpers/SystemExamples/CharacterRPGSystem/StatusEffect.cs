// StatusEffect.cs - A class representing a status effect in a game.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// The StatusEffect class represents a status effect that can be applied to a character in a game. Status effects can modify various aspects
// of a character, such as their attributes, behavior, or abilities. The class provides methods to apply and remove the status effect, as well
// as an optional duration to specify the duration of the effect. Subclasses can override the `Apply`, `Remove`, and `Update` methods to implement
// specific logic for applying and removing the status effect, as well as performing any necessary updates during the effect's duration.
//
// The EncumberedStatusEffect class is a subclass of StatusEffect and represents a specific type of status effect where the character is encumbered.
// It overrides the `Apply` and `Remove` methods to modify the character's agility stat when the effect is applied or removed.
//
// The PoisonedStatusEffect class is another subclass of StatusEffect and represents a specific type of status effect where the character is poisoned.
// It overrides the `Apply` method to start a timer and the `Update` method to periodically apply damage to the character. The effect's damage amount
// and interval can be specified when creating an instance of the class.
//
// No accreditation is required, but it would be appreciated.

using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem
{
    public class StatusEffect
    {
        public string Name { get; }
        public float Duration { get; private set; }
        public bool HasDuration => Duration > 0;
        public bool IsActive { get; private set; }
        protected readonly Character Character;

        public StatusEffect(string name, float duration, Character character)
        {
            Name = name;
            Duration = duration;
            Character = character;
        }

        public virtual void Apply()
        {
            IsActive = true;
            Debug.Log($"Applied status effect: {Name}");
        }

        public virtual void Remove()
        {
            IsActive = false;
            Debug.Log($"Removed status effect: {Name}");
        }

        public virtual void Update(float deltaTime)
        {
            if (!HasDuration) return;

            Duration -= deltaTime;
            if (Duration <= 0)
            {
                Remove();
            }
        }
    }

    public class EncumberedStatusEffect : StatusEffect
    {
        private int _encumberedModifier;
    
        public EncumberedStatusEffect(Character character) : base("Encumbered", 0, character) { }

        public override void Apply()
        {
            base.Apply();
            // Apply specific effects of being encumbered
            Character.Stats.Agility.Value -= _encumberedModifier;
        }

        public override void Remove()
        {
            base.Remove();
            // Remove specific effects of being encumbered
            Character.Stats.Agility.Value += _encumberedModifier;
        }
    }

    public class PoisonedStatusEffect : StatusEffect
    {
        private readonly int _damageAmount;
        private readonly int _damageInterval;
        private float _timer;
        
        //default to damaging every 1 second
        public PoisonedStatusEffect(float duration, Character character, int damageAmount, int damageInterval = 1) : base("Poisoned", duration, character)
        {
            _damageAmount = damageAmount;
            _damageInterval = damageInterval;
        }

        public override void Apply()
        {
            base.Apply();
            // Apply specific effects of being poisoned

            // Start the timer
            _timer = _damageInterval;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (!IsActive) return;
            
            // Update the timer
            _timer -= deltaTime;
            // Check if it's time to apply damage
            if (_timer <= 0)
            {
                // Apply damage to the character
                Character.Stats.Health.TakeDamage(_damageAmount);
                // Reset the timer
                _timer = _damageInterval;
            }
        }

        public override void Remove()
        {
            base.Remove();
            // Remove specific effects of being poisoned
        }
    }
}
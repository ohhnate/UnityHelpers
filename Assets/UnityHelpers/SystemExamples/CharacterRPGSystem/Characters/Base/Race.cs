using System.Collections.Generic;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base
{
    public class Race
    {
        public string Name { get; }
        public List<RacialTrait> Traits { get; }
        public Dictionary<string, float> StatBonuses { get; }

        public Race(string name)
        {
            Name = name;
            Traits = new List<RacialTrait>();
            StatBonuses = new Dictionary<string, float>();
        }

        public void AddTrait(RacialTrait trait)
        {
            Traits.Add(trait);
            // Add the racial bonuses from the trait to the overall stat bonuses of the race
            foreach (KeyValuePair<string, float> bonus in trait.RacialBonuses)
            {
                if (StatBonuses.ContainsKey(bonus.Key))
                {
                    StatBonuses[bonus.Key] += bonus.Value;
                }
                else
                {
                    StatBonuses.Add(bonus.Key, bonus.Value);
                }
            }
        }
    }
    
    public abstract class RacialTrait
    {
        public string Name { get; }
        public string Description { get; }
        public Dictionary<string, float> RacialBonuses { get; }

        public RacialTrait(string name, string description)
        {
            Name = name;
            Description = description;
            RacialBonuses = new Dictionary<string, float>();
        }

        public void AddRacialBonus(string statName, float bonus)
        {
            if (RacialBonuses.ContainsKey(statName))
            {
                RacialBonuses[statName] += bonus;
            }
            else
            {
                RacialBonuses.Add(statName, bonus);
            }
        }
    }

    public class HumanRace : Race
    {
        public HumanRace() : base("Human")
        {
            // Add the specific racial bonuses for the Human race
            StatBonuses.Add("Strength", 1.0f);
            StatBonuses.Add("Agility", 1.0f);
        }
    }
    
    public class ElfRace : Race
    {
        public ElfRace() : base("Elf")
        {
            // Add the specific racial bonuses for the Elf race
            StatBonuses.Add("Agility", 2.0f);
            StatBonuses.Add("Intelligence", 1.0f);
        }
    }

    public class DwarfRace : Race
    {
        public DwarfRace() : base("Dwarf")
        {
            // Add the specific racial bonuses for the Dwarf race
            StatBonuses.Add("Strength", 2.0f);
            StatBonuses.Add("Stamina", 1.0f);
            StatBonuses.Add("PhysicalDefense", 1.0f);
        }
    }
}
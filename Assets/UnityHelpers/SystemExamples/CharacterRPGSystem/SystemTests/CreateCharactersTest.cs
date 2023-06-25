using UnityEngine;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Classes;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.SystemTests
{
    public class CreateCharactersTest : MonoBehaviour
    {
        private void Start()
        {
            // Create instances of a race
            HumanRace humanRace = new HumanRace();
            DwarfRace dwarfRace = new DwarfRace();
            ElfRace elfRace = new ElfRace();
            
            // Create a warrior example
            IBaseStatCalculator warriorCalculator = new BaseStatCalculator(maxHealthModifier: 1.2f, strModifier: 1.9f);
            GameObject warriorObject = new GameObject();
            Warrior warrior = warriorObject.AddComponent<Warrior>();
            warrior.Initialize("Poppy", 1, dwarfRace, warriorCalculator);
            warriorCalculator.CalculateBaseStats(warrior.Stats, 1);
            warriorObject.name = "Poppy";
            warriorObject.transform.SetParent(transform);

            // Create a mage example
            IBaseStatCalculator mageCalculator = new BaseStatCalculator(maxManaModifier: 1.2f, intModifier: 1.9f);
            GameObject mageObject = new GameObject();
            Mage mage = mageObject.AddComponent<Mage>();
            mage.Initialize("Saito", 1, humanRace, mageCalculator);
            mageCalculator.CalculateBaseStats(mage.Stats, 1);
            mageObject.name = "Saito";
            mageObject.transform.SetParent(transform);

            // Create a rogue example
            IBaseStatCalculator rogueCalculator = new BaseStatCalculator(staminaModifier: 1.2f, agiModifier: 1.9f);
            GameObject rogueObject = new GameObject();
            Rogue rogue = rogueObject.AddComponent<Rogue>();
            rogue.Initialize("Nate", 1, elfRace, rogueCalculator);
            rogueCalculator.CalculateBaseStats(rogue.Stats, 1);
            rogueObject.name = "Nate";
            rogueObject.transform.SetParent(transform);
            
            // Create a rogue example
            IBaseStatCalculator rogueCalculator2 = new BaseStatCalculator(staminaModifier: 1.2f, agiModifier: 1.9f);
            GameObject rogueObject2 = new GameObject();
            Rogue rogue2 = rogueObject2.AddComponent<Rogue>();
            rogue2.Initialize("TJ", 69, elfRace, rogueCalculator2);
            rogueCalculator2.CalculateBaseStats(rogue2.Stats, 1);
            rogueObject2.name = "TJ";
            rogueObject2.transform.SetParent(transform);
        }
    }
}

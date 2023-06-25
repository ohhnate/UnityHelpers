using UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Base;
using UnityHelpers.SystemExamples.CharacterRPGSystem.Skills;

namespace UnityHelpers.SystemExamples.CharacterRPGSystem.Characters.Classes
{
    public class Rogue : Character
    {
        private readonly SkillTree _skillTree;
        
        public Rogue(string cName, int level, Race race, BaseStatCalculator statCalculator) : base(cName, level, race, statCalculator) { }

        public override void Attack() { }

        public override void Defend() { }
    }
}
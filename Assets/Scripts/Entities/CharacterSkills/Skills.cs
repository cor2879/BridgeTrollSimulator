using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills
{
    [System.Serializable]
    public class Skills
    {
        [SerializeField] private int persuade;
        [SerializeField] private int intimidate;
        [SerializeField] private int riddle;

        // Future:
        // [SerializeField] private int barter;
        // [SerializeField] private int bridgeMaintenance;
        // [SerializeField] private int deception;
        // etc.

        public int Persuade => persuade;
        public int Intimidate => intimidate;
        public int Riddle => riddle;

        public int Get(SkillType type)
        {
            return type switch
            {
                SkillType.Persuade => persuade,
                SkillType.Intimidate => intimidate,
                SkillType.Riddle => riddle,
                _ => 0
            };
        }

        public void Increase(SkillType type, int amount = 1)
        {
            switch (type)
            {
                case SkillType.Persuade: persuade += amount; break;
                case SkillType.Intimidate: intimidate += amount; break;
                case SkillType.Riddle: riddle += amount; break;
            }
        }
    }
}
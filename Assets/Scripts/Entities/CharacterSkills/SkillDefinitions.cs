using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills
{
    public static class SkillDefinitions
    {
        public static StatType GetAffinity(SkillType skill)
        {
            return skill switch
            {
                SkillType.Persuade => StatType.Charisma,
                SkillType.Intimidate => StatType.Strength,
                SkillType.Riddle => StatType.Intelligence,
                _ => StatType.Luck
            };
        }
    }
}
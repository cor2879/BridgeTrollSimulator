using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    public class SocialDuelContext
    {
        public EntityController Player { get; }
        public EntityController Npc { get; }

        public SkillType? LastSkillUsed;

        public SocialDuelResolutionData Resolution { get; set; }

        public SocialDuelContext(EntityController player, EntityController npc)
        {
            Player = player;
            Npc = npc;
        }
    }
}
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    public class SocialDuelContext
    {
        public EntityController Player { get; }
        public EntityController Npc { get; }

        public int PlayerMaxResolve { get; set;}
        public int PlayerCurrentResolve { get; set;}

        public int NpcMaxResolve { get; set; }
        public int NpcCurrentResolve { get; set; }

        public SkillType? LastSkillUsed;

        public SocialDuelContext(EntityController player, EntityController npc)
        {
            Player = player;
            Npc = npc;

            PlayerMaxResolve = CalculateResolve(player);
            NpcMaxResolve = CalculateResolve(npc);

            PlayerCurrentResolve = PlayerMaxResolve;
            NpcCurrentResolve = NpcMaxResolve;
        }

        private int CalculateResolve(EntityController entity)
        {
            return 20 + entity.BaseStats.Charisma + entity.BaseStats.Constitution;
        }
    }
}
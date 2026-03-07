using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    public class SocialExchangeOutcome
    {
        public SkillType GoverningSkill { get; }
        public SocialExchangeResult Result { get; }
        public int ResolveAmount { get; }
        public bool DamageSelf { get; }
        public bool IsCritical { get; }
        public int Margin { get; }

        public SocialExchangeOutcome(
            SkillType governingSkill,
            SocialExchangeResult result,
            int resolveAmount,
            bool damageSelf,
            bool isCritical,
            int margin)
        {
            GoverningSkill = governingSkill;
            Result = result;
            ResolveAmount = resolveAmount;
            DamageSelf = damageSelf;
            IsCritical = isCritical;
            Margin = margin;
        }
    }
}
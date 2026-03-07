using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.Personalities
{
    [System.Serializable]
    public class PersonalityReactionSet
    {
        public SkillType skill;
        public SocialExchangeResult result;

        [TextArea]
        public List<string> lines;
    }
}
// Personality.cs
using System.Collections.Generic;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.Personalities
{
    [CreateAssetMenu(menuName = "BridgeTroll/Social/Personality")]
    public class Personality : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string personalityName;

        [Header("Stat Affinities")]
        [Tooltip("Modifier applied when defending against actions using this stat")]
        public StatModifier[] defensiveBiases;

        [Header("Emotional Tendencies")]
        [Range(0f, 1f)] public float pride;
        [Range(0f, 1f)] public float caution;
        [Range(0f, 1f)] public float volatility;

        [Header("Reaction Sets")]
        public List<PersonalityReactionSet> reactions;

        [Header("Dialog")]
        [TextArea]
        [SerializeField]
        private string[] leaveDialog;

        [TextArea]
        [SerializeField]
        private string[] attackDialog;

        [TextArea]
        [SerializeField]
        private string[] greetDialog;

        [TextArea]
        [SerializeField]
        private string[] agreeDialog;

        [TextArea]
        [SerializeField]
        private string[] payTollDialog;

        [TextArea]
        [SerializeField]
        private string[] socialDuelVictoryDialog;

        [TextArea]
        [SerializeField]
        private string[] surrenderDialog;

        [TextArea]
        [SerializeField]
        private string[] acceptSurrenderDialog;

        [TextArea]
        [SerializeField]
        private string[] denySurrenderDialog;

        // Runtime lookup
        private Dictionary<(SkillType, SocialExchangeResult), List<string>> lookup;

        public string Name { get => personalityName; }

        private void OnEnable()
        {
            BuildLookup();
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<(SkillType, SocialExchangeResult), List<string>>();

            if (reactions == null)
                return;

            foreach (var reaction in reactions)
            {
                var key = (reaction.skill, reaction.result);

                if (!lookup.TryGetValue(key, out var list))
                {
                    list = new List<string>();
                    lookup[key] = list;
                }

                if (reaction.lines != null)
                {
                    list.AddRange(reaction.lines);
                }
            }
        }

        public string GetReaction(
            SkillType skill,
            SocialExchangeResult result)
        {
            if (lookup == null)
                BuildLookup();

            if (lookup.TryGetValue((skill, result), out var lines) &&
                lines.Count > 0)
            {
                return lines[Random.Range(0, lines.Count)];
            }

            return string.Empty;
        }

        public int GetModifierFor(StatType stat)
        {
            if (defensiveBiases == null)
                return 0;

            foreach (var bias in defensiveBiases)
            {
                if (bias.stat == stat)
                    return bias.modifier;
            }

            return 0;
        }

        public int ModifyResolveImpact(
            int baseAmount,
            StatType governingStat,
            EntityController defender)
        {
            // Keep this simple for now.

            if (governingStat == StatType.Intelligence && pride > 0.7f)
            {
                return baseAmount + 1;
            }

            return baseAmount;
        }

        public override string ToString()
        {
            return Name;
        }

        public string GetLeaveDialog()
        {
            return Pick(leaveDialog);
        }

        public string GetAttackDialog()
        {
            return Pick(attackDialog);
        }

        public string GetGreetDialog()
        {
            return Pick(greetDialog);
        }

        public string GetAgreeDialog()
        {
            return Pick(agreeDialog);
        }

        public string GetPayTollDialog()
        {
            return Pick(payTollDialog);
        }

        public string GetSocialDuelVictoryDialog()
        {
            return Pick(socialDuelVictoryDialog);
        }

        public string GetSurrenderDialog()
        {
            return Pick(surrenderDialog);
        }

        public string GetAcceptSurrenderDialog()
        {
            return Pick(acceptSurrenderDialog);
        }

        public string GetDenySurrenderDialog()
        {
            return Pick(denySurrenderDialog);
        }

        private string Pick(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "...";
            }

            return array[Random.Range(0, array.Length)];
        }
    }
}
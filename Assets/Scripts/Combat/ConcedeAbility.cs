using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Abilities/Concede")]
    public class ConcedeAbility : Ability
    {
        private void OnEnable()
        {
            isOffensive = false;
        }

        public override bool CanExecute(EntityController initiator)
        {
            // Player can always concede
            return true;
        }

        public override bool TryExecuteSpecial(
            EntityController initiator, 
            EntityController target)
        {
            CombatSystem.Instance.ConcedeCombat(initiator, target);

            return true;
        }

        public override float Evaluate(EntityController initiator, EntityController target)
        {
            float healthRatio = (float)initiator.CurrentHealth / initiator.MaxHealth;
            float opponentHealthRatio = (float)target.CurrentHealth / target.MaxHealth;

            float score = -50f;

            // 🔹 Gradual pressure toward conceding
            score += (1f - healthRatio) * 40f;

            var personality = initiator.Personality;

            if (personality != null)
            {
                score += personality.caution * 30f;     // cautious → more likely to concede
                score -= personality.pride * 40f;       // pride → resists surrender

                // volatility adds noise
                score += Random.Range(
                    -personality.volatility * 10f,
                    personality.volatility * 10f);
            }

            // 🔹 If opponent is weak, less reason to concede
            score -= (1f - opponentHealthRatio) * 30f;

            // 🔥 CRITICAL: Threshold spike to compete with Defend (~25)
            if (healthRatio < 0.15f)
            {
                score += 40f; // enough to beat Defend in most cases
            }

            // 🔥 Extra desperation layer (optional but nice)
            if (healthRatio < 0.08f)
            {
                score += 60f; // near-guaranteed concede
            }

        #if UNITY_EDITOR
            Debug.Log($"[AI] Concede Eval | HP: {healthRatio:F2} | OppHP: {opponentHealthRatio:F2} | Score: {score:F2}");
        #endif

            return score;
        }
    }
}
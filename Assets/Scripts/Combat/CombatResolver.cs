using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public static class CombatResolver
    {
        public static void ResolveAbility(
            EntityController initiator,
            EntityController target,
            Ability ability,
            IEventSource eventSource)
        {
            if (!ability.CanExecute(initiator))
            {
                GameEventBus.Publish(
                    new CombatLogEvent(
                        $"{initiator.Name} is unable to use the {ability.Name} ability!",
                        eventSource,
                        Time.frameCount));
                return;
            }

            var exhausted = initiator.CurrentStamina < ability.StaminaCost;

            if (!exhausted)
            {
                initiator.SpendStamina(ability.StaminaCost);
            }

            if (ability.FollowUpSynergies != null &&
                ability.FollowUpSynergies.Count > 0)
            {
                var synergy = ability.FollowUpSynergies[0];

                if (initiator.HasAbility(synergy.ability))
                {
                    initiator.PrimeAbility(synergy);
                }
            }

            var finalDamage = 0;
            var isCritical = false;

            if (ability.IsOffensive)
            {
                var baseDamage = ability.GetBaseDamage(initiator, target);
                baseDamage = Mathf.RoundToInt(baseDamage * ability.DamageMultiplier);

                var critChance = initiator.CritChance + initiator.GetTemporaryCritBonus();

                if (exhausted)
                {
                    baseDamage /= 2;
                    critChance *= 0.5f;
                }

                isCritical = Random.value < critChance;

                finalDamage = isCritical
                    ? Mathf.RoundToInt(baseDamage * initiator.CritMultiplier)
                    : baseDamage;

                target.TakeDamage(finalDamage);
            }

            ability.ApplySecondaryEffects(initiator, target);

            var critText = isCritical ? " (CRITICAL!)" : "";
            var exhaustedText = exhausted ? " (EXHAUSTED)" : "";
            initiator.ConsumePrimeBonus(ability);

            GameEventBus.Publish(
                new CombatLogEvent(
                    $"{initiator.Name} uses {ability.Name}{critText}{exhaustedText}" +
                    (finalDamage > 0 ? $" and deals {finalDamage} damage to {target.Name}." : ""),
                    eventSource,
                    Time.frameCount));
        }
    }
}
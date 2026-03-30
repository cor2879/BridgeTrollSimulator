using System;
using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces
{
    public interface IActor : IReceiver
    {
        int Attack { get; }
        int Defense { get; }

        int CurrentStamina { get; }
        int MaxStamina { get; }
        int Momentum { get; }
        int Level { get; }
        bool IsSurrendering { get; }

        Stats BaseStats { get; }
        Skills BaseSkills { get; }

        void Defend();
        void TakeDamage(int amount, bool isCrit = false);
        void RestoreStamina(int amount, bool isCrit = false);
        void RestoreHealth(int amount, bool isCrit = false);
        void RemoveStatusEffect<TStatusEffect>() where TStatusEffect : StatusEffect;
        void RemoveStatusEffects(Func<StatusEffect, bool> predicate);

        bool CanExecute(Ability ability);

        float GetPrimeBonus(Ability ability);

        IReadOnlyCollection<Ability> ActiveAbilities { get; } // Supports current Ability system and will likely be used to select which abilities are "Active for Combat/etc"
        AbilityComponent AbilityComponent { get; }
    }
}
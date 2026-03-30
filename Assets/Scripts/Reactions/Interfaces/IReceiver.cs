using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface IReceiver : IReactor
    {
        void Receive<TEvent>(TEvent evt) where TEvent : ITargetedEvent;
        public void AddStatusEffect(StatusEffect newEffect, EffectStackingType stackingType);
    }
}
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events
{
    public class StatusEffectExpiredEvent : GameEvent
    {
        public IActor Target => (IActor)Sender;
        public EffectDefinition Effect { get; }
        public int Value { get; }

        public StatusEffectExpiredEvent(
            IActor target,
            EffectDefinition effect,
            int value)
            : base(target, Time.frameCount)
        {
            Effect = effect;
            Value = value;
        } 

        public override string ToString()
        {
            return $"{nameof(StatusEffectExpiredEvent)}" +
                $"::Target:{Target.SourceName}::Effect:{Effect}" +
                $"::Value:{Value} @ Frame {Frame}";
        }
    }
}
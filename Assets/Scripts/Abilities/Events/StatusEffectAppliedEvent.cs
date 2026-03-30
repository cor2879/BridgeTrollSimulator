using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events
{
    public class StatusEffectAppliedEvent : GameEvent
    {
        public IActor Initiator => (IActor)Sender;
        public IActor Target { get; }
        public StatusEffect StatusEffect { get; }
        public AudioClip SoundEffect { get; }

        public StatusEffectAppliedEvent(
            IActor initiator,
            IActor target,
            StatusEffect statusEffect,
            AudioClip soundEffect)
            : base(initiator, Time.frameCount)
        {
            Target = target;
            StatusEffect = statusEffect;
            SoundEffect = soundEffect;
        } 

        public override string ToString()
        {
            return $"{nameof(StatusEffectAppliedEvent)}::Initiator:{Sender.SourceName}" +
                $"::Target:{Target.SourceName}::StatusEffect:{StatusEffect.EffectId}" +
                $"::SoundEffect:{SoundEffect.name} @ Frame {Frame}";
        }
    }
}
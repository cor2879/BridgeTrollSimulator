using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class StaminaRestoredEvent : GameEvent
    {
        public IReactor Target => (IReactor)Sender;
        public int Amount { get; }
        public bool IsCrit { get; }

        public StaminaRestoredEvent(
            IReactor target,
            int amount,
            bool isCrit = false)
            : base(target, Time.frameCount)
        {
            Amount = amount;
            IsCrit = isCrit;
        }

        public override string ToString()
        {
            return $"{nameof(StaminaRestoredEvent)}::Target:{Target.SourceName}::Amount:{Amount}::" +
                $"IsCrit:{IsCrit} @ Frame {Frame}";
        }
    }
}
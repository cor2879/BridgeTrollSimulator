using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EncounterDemandRefusedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        public int Amount { get; }
        
        public EncounterDemandRefusedEvent(
            IReceiver initiator,
            IReceiver target,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(EncounterDemandRefusedEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName}::Amount:{Amount} @ Frame {Frame}";
        }
    }
}
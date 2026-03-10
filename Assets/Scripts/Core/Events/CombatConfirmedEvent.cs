using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatConfirmedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        
        public CombatConfirmedEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(CombatStartedEvent)}: {Initiator?.SourceName} vs {Target?.SourceName} @ Frame {Frame}";
        }
    }
}
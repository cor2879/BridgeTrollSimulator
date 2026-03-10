using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatEndedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        public CombatOutcome Outcome { get; }
        
        public CombatEndedEvent(
            IReceiver initiator,
            IReceiver target,
            CombatOutcome combatOutcome,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
            Outcome = combatOutcome;
        }

        public override string ToString()
        {
            return $"{nameof(CombatEndedEvent)}: {Initiator?.SourceName} vs {Target?.SourceName} Outcome: {Outcome} @ Frame {Frame}";
        }
    }
}
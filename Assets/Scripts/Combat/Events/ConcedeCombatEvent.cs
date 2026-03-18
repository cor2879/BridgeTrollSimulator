using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Events
{
    public class ConcedeCombatEvent : ConcedeEvent
    {
        public IDemand Concession { get; }

        public ConcedeCombatEvent(
            IReceiver initiator,
            IReceiver target,
            IDemand concession,
            int frame)
            : base(initiator, target, frame)
        {
            Concession = concession;
        }

        public override string ToString()
        {
            return $"{nameof(ConcedeCombatEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName}::Concession:{Concession} @ Frame {Frame}";
        }
    }
}
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatResolutionCompletedEvent : GameEvent
    {
        public CombatResolutionData Data { get; }

        public CombatResolutionCompletedEvent(
            IEventSource subject,
            CombatResolutionData data,
            int frame)
            : base(subject, frame)
        { 
            Data = data;
        }         

        public override string ToString()
        {
            return $"{nameof(CombatResolutionCompletedEvent)}::Initiator:{Sender.SourceName}::Data:{Data} @ Frame {Frame}";
        }
    }
}
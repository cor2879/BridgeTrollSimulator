using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class DialogEndedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator { get; }
        public IReceiver Target { get; }

        public DialogEndedEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            Initiator = initiator;
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(DialogEndedEvent)}::Initiator:{Initiator.SourceName}::Target:{Target.SourceName} @ Frame {Frame}";
        }
    }
}
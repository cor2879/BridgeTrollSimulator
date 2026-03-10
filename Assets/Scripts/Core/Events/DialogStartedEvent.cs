using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DialogStartedEvent : GameEvent, ITargetedEvent
    {
        public DialogNode RootNode { get; }
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }

        public DialogStartedEvent(
            DialogNode rootNode,
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            RootNode = rootNode;
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(DialogStartedEvent)}::Initiator:{Initiator.SourceName}::Target:{Target.SourceName}::" +
                $"Dialog:\"{RootNode.Text}\" @ Frame {Frame}";
        }
    }
}
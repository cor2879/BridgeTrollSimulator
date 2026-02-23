using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DialogStartedEvent : GameEvent, ITargetedEvent
    {
        public DialogNode RootNode { get; }
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }

        public DialogStartedEvent(
            DialogNode rootNode,
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            RootNode = rootNode;
            Target = target;
        }
    }
}
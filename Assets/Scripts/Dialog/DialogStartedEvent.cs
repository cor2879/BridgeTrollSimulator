using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class DialogStartedEvent : GameEvent
    {
        public DialogNode RootNode { get; }
        public EntityController Initiator { get; }
        public EntityController Target { get; }

        public DialogStartedEvent(
            DialogNode rootNode,
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            RootNode = rootNode;
            Initiator = initiator;
            Target = target;
        }
    }
}
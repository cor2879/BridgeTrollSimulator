using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class DialogStartedEvent : GameEvent
    {
        public DialogSequence Sequence { get; }
        public EntityController Initiator { get; }
        public EntityController Target { get; }

        public DialogStartedEvent(
            DialogSequence sequence,
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            Sequence = sequence;
            Initiator = initiator;
            Target = target;
        }
    }
}
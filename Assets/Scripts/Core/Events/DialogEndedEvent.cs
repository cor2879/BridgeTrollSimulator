using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class DialogEndedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator { get; }
        public EntityController Target { get; }

        public DialogEndedEvent(
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            Initiator = initiator;
            Target = target;
        }
    }
}
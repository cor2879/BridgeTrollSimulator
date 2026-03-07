using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialDuelStartedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }

        public SocialDuelStartedEvent(
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
        }
    }
}
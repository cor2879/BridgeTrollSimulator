using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpConfirmedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Target { get; }
        public int NewLevel { get; }
        public int PointsGranted { get; }

        public LevelUpConfirmedEvent(
            IEventSource sender,
            EntityController target,
            int frame)
            : base(sender, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpConfirmedEvent)}::Target:{Target.Name}:: @ Frame {Frame}";
        }
    }
}
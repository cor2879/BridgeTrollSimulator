using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpEvent : GameEvent, ITargetedEvent
    {
        public EntityController Target { get; }
        public int NewLevel { get; }
        public int PointsGranted { get; }

        public LevelUpEvent(
            IEventSource sender,
            EntityController target,
            int newLevel,
            int pointsGranted,
            int frame)
            : base(sender, frame)
        {
            Target = target;
            NewLevel = newLevel;
            PointsGranted = pointsGranted;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpEvent)}::Target:{Target.Name}::NewLevel:{NewLevel}::" +
                $"PointsGraned:{PointsGranted}:: @ Frame {Frame}";
        }
    }
}
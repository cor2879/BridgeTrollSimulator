using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DefendEvent : GameEvent
    {
        public EntityController Target => (EntityController)Sender;
        public int Amount { get; }

        public bool IsCrit { get; }

        public DefendEvent(
            EntityController subject,
            int frame)
            : base(subject, frame)
        { }         
    }
}
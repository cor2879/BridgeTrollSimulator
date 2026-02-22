using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DamageTakenEvent : GameEvent
    {
        public EntityController Target => (EntityController)Sender;
        public int Amount { get; }

        public bool IsCrit { get; }

        public DamageTakenEvent(
            EntityController subject,
            int amount,
            int frame,
            bool isCrit = false)
            : base(subject, frame)
        {
            this.Amount = amount;
            this.IsCrit = isCrit;
        } 
    }
}
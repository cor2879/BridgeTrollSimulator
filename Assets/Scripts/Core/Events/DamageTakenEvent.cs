using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DamageTakenEvent : GameEvent
    {
        public int Amount { get; }

        public bool IsCrit { get; }

        public DamageTakenEvent(
            IEventSource initiator,
            int amount,
            int frame,
            bool isCrit = false)
            : base(initiator, frame)
        {
            this.Amount = amount;
            this.IsCrit = isCrit;
        } 

        public override string ToString()
        {
            return $"{nameof(DamageTakenEvent)}::Initiator:{Sender.SourceName}::Amount:{Amount}::IsCrit:{IsCrit} @ Frame {Frame}";
        }
    }
}
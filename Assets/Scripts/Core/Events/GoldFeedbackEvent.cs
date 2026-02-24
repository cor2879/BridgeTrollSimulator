using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldFeedbackEvent : GameEvent
    {
        public int PreviousAmount;
        public int NewAmount;
        public int Delta;

        public EntityController Target => (EntityController)Sender;

        public GoldFeedbackEvent(
            EntityController target,
            int previous,
            int current,
            int delta,
            int frame)
            : base(target, frame)
        {
            PreviousAmount = previous;
            NewAmount = current;
            Delta = delta;
        }

        public override string ToString()
        {
            return $"{nameof(GoldFeedbackEvent)} {Target.Name} :: Previous Amount {PreviousAmount} :: NewAmount {NewAmount} :: Delta {Delta} @ Frame {Frame}";
        }
    }
}
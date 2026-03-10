using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldFeedbackEvent : GameEvent
    {
        public int PreviousAmount;
        public int NewAmount;
        public int Delta;

        public GoldFeedbackEvent(
            IEventSource target,
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
            return $"{nameof(GoldFeedbackEvent)}::Sender:{Sender.SourceName} :: Previous Amount {PreviousAmount} ::" +
                $" NewAmount {NewAmount} :: Delta {Delta} @ Frame {Frame}";
        }
    }
}
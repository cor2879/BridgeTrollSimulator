using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class ResolveChangedEvent : GameEvent
    {
        public IReactor Entity => (IReactor)Sender;
        public int Amount { get; }

        public ResolveChangedEvent(
            IReactor entity,
            int amount,
            int frame)
            : base(entity, frame)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(ResolveChangedEvent)}::Entity:{Entity.SourceName}" +
                $"::Amount:{Amount} @ Frame {Frame}";
        }
    }
}
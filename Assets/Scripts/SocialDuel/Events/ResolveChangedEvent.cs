using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class ResolveChangedEvent : GameEvent
    {
        public EntityController Entity => (EntityController)Sender;
        public int Amount { get; }

        public ResolveChangedEvent(
            EntityController entity,
            int amount,
            int frame)
            : base(entity, frame)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(ResolveChangedEvent)}::Entity:{Entity.Name}" +
                $"::Amount:{Amount} @ Frame {Frame}";
        }
    }
}
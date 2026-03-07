using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialActionAttemptedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Attacker => (EntityController)Sender;
        public EntityController Target { get; }
        public string ActionName { get; }
        public bool Success { get; }

        public SocialActionAttemptedEvent(
            EntityController attacker,
            EntityController target,
            string actionName,
            bool success,
            int frame)
            : base(attacker, frame)
        {
            Target = target;
            ActionName = actionName;
            Success = success;
        }

        public override string ToString()
        {
            return $"{nameof(SocialActionAttemptedEvent)}::Attacker:{Attacker.Name}" +
                $"::Target:{Target.Name}::Action:{ActionName}::Success:{Success} @ Frame {Frame}";
        }
    }
}
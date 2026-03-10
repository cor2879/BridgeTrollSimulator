using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpConfirmedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }

        public LevelUpConfirmedEvent(
            IEventSource sender,
            IReceiver target,
            int frame)
            : base(sender, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpConfirmedEvent)}::Target:{Target.SourceName}::" +
                $" @ Frame {Frame}";
        }
    }
}
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatRewardEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }
        public int Experience { get; }
        public int Fame { get; }
        public int Respect { get; }
        public int Reputation { get; }
        public int Gold { get; }

        public CombatRewardEvent(
            IEventSource sender,
            IReceiver target,
            int experience,
            int fame,
            int respect,
            int reputation,
            int gold,
            int frame)
            : base(sender, frame)
        {
            Target = target;
            Experience = experience;
            Fame = fame;
            Respect = respect;
            Reputation = reputation;
            Gold = gold;
        }

        public override string ToString()
        {
            return $"{nameof(CombatRewardEvent)}::Sender:{Sender.SourceName}::Target:{Target.SourceName}::" +
                $"Experience:{Experience}::Fame:{Fame}::Respect:{Respect}::Reputation:{Reputation}::Gold:{Gold}" +
                $" @ Frame {Frame}";
        }
    }
}
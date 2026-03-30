using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }
        public int NewLevel { get; }
        public int StatPointsGranted { get; }
        public int AbilityPointsGranted { get; }

        public LevelUpEvent(
            IEventSource sender,
            IReceiver target,
            int newLevel,
            int statPointsGranted,
            int abilityPointsGranted,
            int frame)
            : base(sender, frame)
        {
            Target = target;
            NewLevel = newLevel;
            StatPointsGranted = statPointsGranted;
            AbilityPointsGranted = abilityPointsGranted;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpEvent)}::Target:{Target.SourceName}::NewLevel:{NewLevel}::" +
                $"StatPointsGraned:{StatPointsGranted}::AbilityPointsGranted:{AbilityPointsGranted}" +
                $" @ Frame {Frame}";
        }
    }
}
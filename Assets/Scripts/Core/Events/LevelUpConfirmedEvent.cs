using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpConfirmedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }

        public IReadOnlyDictionary<StatType, int> StatAllocations { get; }
        public IReadOnlyList<AbilityNode> SelectedAbilities { get; }

        public LevelUpConfirmedEvent(
            IEventSource sender,
            IReceiver target,
            Dictionary<StatType, int> statAllocations,
            List<AbilityNode> selectedAbilities,
            int frame)
            : base(sender, frame)
        {
            Target = target;
            StatAllocations = statAllocations;
            SelectedAbilities = selectedAbilities;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpConfirmedEvent)}::Target:{Target.SourceName}::" +
                $"Payload includes {StatAllocations.Count} stat allocations and" +
                $"{SelectedAbilities.Count} ability choices. @ Frame {Frame}";
        }
    }
}
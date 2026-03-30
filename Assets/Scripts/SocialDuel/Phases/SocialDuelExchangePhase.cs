using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Phases
{
    public class SocialDuelExchangePhase : SocialDuelPhaseBase, IEventSource
    {

        #region IEventSource

        public string SourceName => nameof(SocialDuelExchangePhase);
        public GameSystemType SystemType => GameSystemType.System;

        #endregion

        public override void Enter(SocialDuelSystem system, SocialDuelContext context)
        {
            base.Enter(system, context);

            system.UI.Initialize(context);
            system.UI.Show();
            system.UI.EnableInput(true);
        }

        public override void OnAbilityChosen(SocialAbility ability)
        {
            System.UI.EnableInput(false);

            var attacker = Context.Player;
            var defender = Context.Npc;

            if (ability.TryExecuteSpecial(attacker, defender))
            {
                return;
            }

            var outcome = ability.ResolveExchange(attacker, defender);

            System.ApplyOutcome(attacker, defender, outcome);

            if (System.CheckForEnd())
            {
                return;
            }

            System.ShowAttackerLine(attacker, ability, outcome);
            Context.LastSkillUsed = ability.GoverningSkill;

            GameEventBus.Publish(
                new SocialActionAttemptedEvent(
                    attacker,
                    defender,
                    ability.Name,
                    outcome.Result <= SocialExchangeResult.WeakSuccess,
                    Time.frameCount));            
        }
    }
}
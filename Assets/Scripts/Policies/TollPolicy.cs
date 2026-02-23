using UnityEngine;

using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Policies
{
    [CreateAssetMenu(menuName = "BridgeTroll/Policies/Toll Policy")]
    public class TollPolicy : OptionPolicy
    {
        public override List<GeneratedOption> GetAvailableOptions(EntityController initiator, EntityController target)
        {
            var results = new List<GeneratedOption>();

            var maxDemand = CalculateMaxDemand(initiator);
            var npcGold = target.Gold;

            var amount = 10;

            while (amount <= maxDemand && amount <= npcGold)
            {
                var capturedAmount = amount;

                results.Add(new GeneratedOption
                {
                    Label = $"Demand {capturedAmount} Gold",
                    Execute = (initiator, target) =>
                    {
                        GameEventBus.Publish(
                            new TollDemandedEvent(initiator, target, amount, Time.frameCount));
                    }
                });

                amount *= 2;
            }

            results.Add(new GeneratedOption
            {
                Label = "Cancel",
                Execute = (i, t) => DialogSystem.Instance.GoBack()
            });

            return results;
        }

        private int CalculateMaxDemand(EntityController initiator)
        {
            return initiator.Level * 10; // simple starting rule
        }
    }
}
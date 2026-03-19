using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands
{
    public class DemandComponent : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private readonly Queue<IDemand> demands = new();

        public IReadOnlyList<IDemand> Demands => demands.ToList();

        public void AddDemand(IDemand demand)
        {
            demands.Enqueue(demand);
        }

        public void ResolveNextDemand(IResolver resolver)
        {
            Debug.Log($"{nameof(DemandComponent)}::{nameof(ResolveNextDemand)}::DemandCount:{demands.Count}");

            if (demands.Count == 0)
            {
                return;
            }

            var demand = demands.Dequeue();

            if (demand.CanResolve(resolver))
            {
                demand.OnAccepted(resolver);
            }
            else
            {
                GameEventBus.Publish(
                    new DemandRefusedEvent(
                        resolver,
                        demand.Source,
                        demand,
                        Time.frameCount));
            }
        }

        public void ResolveDemands(IResolver resolver)
        {
            while (demands.Count > 0)
            {
                var demand = demands.Dequeue();

                if (demand.CanResolve(resolver))
                {
                    demand.OnAccepted(resolver);
                }
                else
                {
                     GameEventBus.Publish(
                        new DemandRefusedEvent(
                            resolver,
                            demand.Source,
                            demand,
                            Time.frameCount));   
                }
            }
        }

        public void Clear()
        {
            demands.Clear();
        }

        public bool HasDemands => demands.Count > 0;
    }
}
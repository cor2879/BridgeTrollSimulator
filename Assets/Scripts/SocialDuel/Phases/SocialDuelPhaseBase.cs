using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Phases
{
    public abstract class SocialDuelPhaseBase : ISocialDuelPhase
    {
        protected SocialDuelSystem System;
        protected SocialDuelContext Context;

        private static readonly Dictionary<System.Type, ISocialDuelPhase> instances = new();

        protected SocialDuelPhaseBase() {}

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetCache()
        {
            instances.Clear();
        }

        public virtual void Enter(SocialDuelSystem system, SocialDuelContext context)
        {
            System = system;
            Context = context;
            #if UNITY_EDITOR
            Debug.Log($"{this.GetType().Name}::Enter @ Frame {Time.frameCount}");
            #endif
        }

        public virtual void Exit() 
        { 
        }

        public virtual void OnAbilityChosen(SocialAbility ability)
        {
        }

        public virtual void OnAdvance()
        {}

        public static TSocialDuelPhase GetInstance<TSocialDuelPhase>() 
            where TSocialDuelPhase : ISocialDuelPhase, new()
        {
            var type = typeof(TSocialDuelPhase);

            if (instances.TryGetValue(type, out var existing))
            {
                return (TSocialDuelPhase)existing;
            }

            instances.Add(type, new TSocialDuelPhase());
            return (TSocialDuelPhase)instances[type];            
        }
    }
}
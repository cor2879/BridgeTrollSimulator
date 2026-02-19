using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class StaminaDamageTakenEvent : GameEvent
    {
        public EntityController Target => (EntityController)Sender;
        public int Amount { get; private set; }

        public StaminaDamageTakenEvent(
            EntityController target,
            int amount,
            int frame)
            : base(target, frame)
        {
            Amount = amount;
        }
    }
}
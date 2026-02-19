using System;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public class InitiativeEntry : IComparable<InitiativeEntry>
    {
        public EntityController Entity { get; }

        public InitiativeEntry(EntityController entity)
        {
            Entity = entity;
        }

        public int CompareTo(InitiativeEntry other)
        {
            return other.Entity.InitiativeRoll.CompareTo(this.Entity.InitiativeRoll);
        }
    }
}
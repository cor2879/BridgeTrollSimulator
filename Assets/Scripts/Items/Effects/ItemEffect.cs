using System;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Items.Effects
{
    [Serializable]
    public abstract class ItemEffect
    {
        public int BaseValue;
        public float Percentage;
        public DiceType Dice;
        public int DiceCount = 1;
        
        

        public virtual void OnUse(IActor target) { }

        public virtual void OnEquip(IActor target) { }

        public virtual void OnUnequip(IActor target) { }
    }
}
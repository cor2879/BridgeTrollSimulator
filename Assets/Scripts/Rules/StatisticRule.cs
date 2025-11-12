/**************************************************
 *  StatisticRule.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rules
{
    using System;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    public class StatisticRule<TValue> : Rule where TValue : struct, IComparable<TValue>
    {
        public StatisticRule(string name, Statistic<TValue> statistic, TValue value)
            : base(name)
        {
            this.Statistic = statistic;
            this.Statistic.OnUpdate += this.UpdateListeners;
            this.Value = value;
        }

        public Statistic<TValue> Statistic
        {
            get; private set;
        }

        public TValue Value
        {
            get; private set;
        }

        public override bool Evaluate()
        {
            return this.Statistic.Value.CompareTo(this.Value) >= 0;
        }
    }
}

/**************************************************
 *  MultiStatisticRule.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    /// <summary>
    /// Defines a rule that is made up of multiple statistics which is evaluated as true when any
    /// of the statistics it is monitoring meets or exceeds its sentinel value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.Rules.Rule" />
    public class MultiStatisticRule<TValue> : Rule where TValue : struct, IComparable<TValue>
    {
        public MultiStatisticRule(string name, IEnumerable<Statistic<TValue>> statistics, TValue value)
            : base(name)
        {
            this.Statistics = statistics.ToArray();

            foreach (var statistic in this.Statistics)
            {
                statistic.OnUpdate += this.UpdateListeners;
            }

            this.Value = value;
        }

        public Statistic<TValue>[] Statistics
        {
            get; private set;
        }

        public TValue Value
        {
            get; private set;
        }

        public override bool Evaluate()
        {
            return this.Statistics.Any(statistic => statistic.Value.CompareTo(this.Value) >= 0);
        }
    }
}
/**************************************************
 *  WaitAction.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class WaitDuration
    {
        /// <summary>
        /// Gets the predicate.
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        public float Duration
        {
            get; set;
        }

        /// <summary>
        /// Gets the do action.
        /// </summary>
        /// <value>
        /// The do action.
        /// </value>
        public Action DoAction
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAction"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="doAction">The do action.</param>
        public WaitDuration(float duration, Action doAction)
        {
            Validator.ArgumentIsNotNull(doAction, nameof(doAction));

            this.Duration = duration;
            this.DoAction = doAction;
        }
    }
}

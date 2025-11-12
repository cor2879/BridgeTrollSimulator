/**************************************************
 *  StackExtension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using System.Collections.Generic;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Extends the <see cref="Stack{T}" /> class
    /// </summary>
    public static class StackExtension
    {
        /// <summary>
        /// Pushes the specified values.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="stack">The stack.</param>
        /// <param name="values">The values.</param>
        public static void Push<TValue>(this Stack<TValue> stack, IEnumerable<TValue> values)
        {
            Validator.ArgumentIsNotNull(stack, $"{nameof(stack)}");

            if (values == null)
            {
                return;
            }

            foreach (var value in values)
            {
                stack.Push(value);
            }
        }
    }
}

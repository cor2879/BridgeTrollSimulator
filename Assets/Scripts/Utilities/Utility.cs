/**************************************************
 *  Utility.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    using System;
    using System.Linq;

    /// <summary>
    /// Static class which defines some general utility functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Determines whether or not the specified value is between the lower and upper bound values, inclusive.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns></returns>
        public static bool Between<TValue>(TValue value, TValue lowerBound, TValue upperBound)
            where TValue : IComparable
        {
            return value.CompareTo(lowerBound) > -1 && value.CompareTo(upperBound) < 1;
        }

        /// <summary>
        /// Returns either the left hand side or the right hand side, whichever is the lesser value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        public static TValue Min<TValue>(TValue lhs, TValue rhs) where TValue : IComparable<TValue>
        {
            return lhs.CompareTo(rhs) < 1 ? lhs : rhs;
        }

        public static int Sum(params int[] numbers)
        {
            if (numbers == null || !numbers.Any())
            {
                return 0;
            }

            var total = 0;

            foreach (var number in numbers)
            {
                total += number;
            }

            return total;
        }

        public static bool ParseBool(string text)
        {
            switch (text)
            {
                case "Yes":
                    return true;
                case "No":
                    return false;
                default:
                    return bool.Parse(text);
            }
        }
    }
}

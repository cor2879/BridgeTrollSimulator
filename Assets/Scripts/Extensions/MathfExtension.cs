/**************************************************
 *  MathfExtension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using System;

    /// <summary>
    /// Static class that offers methods that extend the functionality of the Mathf class.
    /// </summary>
    public static class MathfExtension
    {
        /// <summary>
        /// Returns the value if it is between the max or min values.  If it is greater than the
        /// max value, the max value is returned.  If it is less than the min value, the min
        /// value is returned.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns></returns>
        public static TValue MaxOrMin<TValue>(TValue value, TValue max, TValue min) where TValue : struct, IComparable<TValue>
        {
            if (min.CompareTo(max) > 0)
            {
                throw new InvalidOperationException("The min value must be less than or equal to the max value.");
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            return value;
        }

        /// <summary>
        /// Sums the specified numbers.
        /// </summary>
        /// <param name="num0">The num0.</param>
        /// <param name="num1">The num1.</param>
        /// <returns></returns>
        public static int Sum(int num0, int num1)
        {
            unchecked
            {
                return num0 + num1;
            }
        }

        /// <summary>
        /// Sums the specified numbers.
        /// </summary>
        /// <param name="num0">The num0.</param>
        /// <param name="num1">The num1.</param>
        /// <param name="num2">The num2.</param>
        /// <returns></returns>
        public static int Sum(int num0, int num1, int num2)
        {
            unchecked
            {
                return num0 + num1 + num2;
            }
        }

        /// <summary>
        /// Sums the specified numbers.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns></returns>
        public static int Sum(params int[] numbers)
        {
            unchecked
            {
                var total = 0;

                foreach (var num in numbers)
                {
                    total += num;
                }

                return total;
            }
        }
    }
}

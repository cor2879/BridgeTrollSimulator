/**************************************************
 *  IEnumerableExtension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extends the <see cref="IEnumerable{T}" /> interface
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Determines whether an instance is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the items contained in the collection</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///   <c>true</c> if the instance is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            if (!(collection?.Any() ?? false))
            {
                throw new InvalidOperationException("Attempted to retrieve from a null or empty collection.");
            }

            return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
        }
    }
}

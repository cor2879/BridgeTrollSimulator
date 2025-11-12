namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class HashSetExtension
    {
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> collection)
        {
            if (hashSet == null)
            {
                throw new ArgumentNullException($"The parameter {nameof(hashSet)} may not be null", nameof(hashSet));
            }

            if (collection == null)
            {
                throw new ArgumentNullException($"The parameter {nameof(collection)} may not be null", nameof(collection));
            }

            foreach (var item in collection)
            {
                hashSet.Add(item);
            }
        }
    }
}

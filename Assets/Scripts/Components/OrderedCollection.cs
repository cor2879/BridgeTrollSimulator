/**************************************************
 *  OrderedCollection.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a collection of ordered elements.  Use the 'AllowDuplicateValues' property to control whether
    /// or not the collection can contain duplicate values.  The default value for this property is <c>true</c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class OrderedCollection<T>
        : ICollection<T> where T : IComparable<T>
    {
        private List<T> innerList;
        private Comparison<T> comparison = (Tx, Ty) => Tx.CompareTo(Ty);

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        public OrderedCollection()
        {
            this.innerList = new List<T>();
        }

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        /// <param name="collection">
        /// The existing collection.
        /// </param>
        public OrderedCollection(IEnumerable<T> collection)
            : this()
        {
            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        /// <param name="capacity">The capacity</param>
        public OrderedCollection(int capacity)
        {
            this.innerList = new List<T>(capacity);
        }

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        /// <param name="comparison">The comparison</param>
        public OrderedCollection(Comparison<T> comparison)
            : this()
        {
            this.comparison = comparison;
        }

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        /// <param name="comparison">The comparison</param>
        /// <param name="capacity">The capacity</param>
        public OrderedCollection(Comparison<T> comparison, int capacity)
        {
            this.comparison = comparison;
            this.innerList = new List<T>(capacity);
        }

        /// <summary>
        /// Creates a new instance of the OrderedCollection class.
        /// </summary>
        /// <param name="comparison">The comparison</param>
        /// <param name="collection">The collection</param>
        public OrderedCollection(Comparison<T> comparison, IEnumerable<T> collection)
            : this(comparison)
        {
            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Determines whether or not the collection will allow duplicate values.  Default value is <c>true</c>.
        /// </summary>
        [DataMember]
        public bool AllowDuplicateValues { get; set; } = true;

        /// <summary>
        /// Gets or sets the inner list
        /// </summary>
        [DataMember]
        internal List<T> InnerList
        {
            get { return this.innerList; }
            set { this.innerList = value; }
        }

        [DataMember]
        internal Comparison<T> Comparison
        {
            get { return this.comparison; }
            set { this.comparison = value; }
        }

        /// <summary>
        /// Returns the element found at the specified zero-based index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The <see cref="T" /> instance found at the index.</returns>
        public T this[int index]
        {
            get { return this.innerList[index]; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get { return this.innerList.Count; }
        }

        /// <summary>
        /// Gets the boolean indicating whether or not this collection is read only (hint: it isn't)
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds a new element to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public virtual void Add(T item)
        {
            if (this.innerList.Count == 0)
            {
                this.innerList.Add(item);
                return;
            }

            Type type = typeof(T);

            int lastIndex = Count - 1;

            if (!type.IsValueType && (Object.Equals(item, null) && ((Count > 0) && (this[0] != null))))
            {
                this.innerList.Insert(0, item);
            }
            else if (this.comparison(item, this[0]) < 0)
            {
                this.innerList.Insert(0, item);
            }
            else if (this.comparison(item, this[lastIndex]) > 0)
            {
                this.innerList.Add(item);
            }
            else
            {
                PartitionAdd(item, 0, Count);
            }
        }

        /// <summary>
        /// Adds an element to the collection.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="index">The index of the new item.  If the item could not be added, this will be -1.</param>
        public virtual void Add(T item, out int index)
        {
            if (this.innerList.Count == 0)
            {
                this.innerList.Add(item);
            }

            Type type = typeof(T);

            int lastIndex = Count - 1;

            if (!type.IsValueType && (Object.Equals(item, null) && ((Count > 0) && (this[0] != null))))
            {
                this.innerList.Insert(0, item);
                index = 0;
            }
            else if (this.comparison(item, this[0]) < 0)
            {
                this.innerList.Insert(0, item);
                index = 0;
            }
            else if (this.comparison(item, this[lastIndex]) > 0)
            {
                this.innerList.Add(item);
                index = ++lastIndex;
            }
            else
            {
                index = PartitionAdd(item, 0, Count);
            }
        }

        /// <summary>
        /// Partitions the collection and adds the item to the most appropriate half.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="start">The start index</param>
        /// <param name="count">The count</param>
        /// <returns>The index of the item after it is added.  If the result is -1 then the add operation was unsuccessful.</returns>
        private int PartitionAdd(T item, int start, int count)
        {
            int mid;

            if (count == 0)
            {
                return -1;
            }
            else if (count == 1)
            {
                mid = start;
            }
            else
            {
                mid = start + (count / 2);
            }

            var comparison = this.comparison(item, this[mid]);

            if (comparison == 0)
            {
                if (!item.Equals(this[mid]) || this.AllowDuplicateValues)
                {
                    this.innerList.Insert(++mid, item);
                }

                return mid;
            }
            else if (comparison < 0)
            {
                int comparison2 = this.comparison(item, this[Math.Max(mid - 1, 0)]);

                if (comparison2 > 0 || (this.AllowDuplicateValues && comparison2 == 0))
                {
                    this.innerList.Insert(mid, item);
                    return mid;
                }
                else if (comparison2 < 0)
                {
                    return PartitionAdd(item, start, Math.Min((mid + 1) - start, Count - start));
                }
                else
                {
                    return -1;
                }
            }
            else // comparison > 0
            {
                int nextIndex = Math.Min(mid + 1, Count - 1);
                int comparison2 = this.comparison(item, this[nextIndex]);

                if (comparison2 < 0)
                {
                    this.innerList.Insert(nextIndex, item);
                    return nextIndex;
                }
                else if (comparison2 > 0)
                {
                    return PartitionAdd(item, nextIndex, count - (nextIndex - start));
                }
                else if (this.AllowDuplicateValues && comparison2 == 0)
                {
                    this.innerList.Insert(nextIndex, item);
                    return nextIndex;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            this.innerList.Clear();
        }

        /// <summary>
        /// Indicates whether or not this collection contains an instance of the specified item.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns><c>true</c> if the collection contains the item, otherwise fals.</returns>
        public bool Contains(T item)
        {
            return PartitionIndexOf(item, 0, Count) > -1;
        }

        /// <summary>
        /// Gets the index of the specified item, or -1 if the item is not found.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>The zero based index of the item, or -1 if the item is not found.</returns>
        public int IndexOf(T item)
        {
            return PartitionIndexOf(item, 0, Count);
        }

        /// <summary>
        /// Partitions the collection and searches for the index of the item in the most appropriate half.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="start">The start index</param>
        /// <param name="count">The count</param>
        /// <returns>The zero-based index of the item if found.  If not found, -1 is returned.</returns>
        private int PartitionIndexOf(T item, int start, int count)
        {
            int mid;

            if (count == 0)
            {
                return -1;
            }
            else if (count == 1)
            {
                mid = start;
            }
            else
            {
                mid = start + (count / 2);
            }

            int comparison = this.comparison(item, this[mid]);

            if (comparison == 0)
            {
                if (item.Equals(this[mid]))
                {
                    return mid;
                }

                var index = mid;

                while (++index != Count && this.comparison(item, this[index]) == 0)
                {
                    if (item.Equals(this[index]))
                    {
                        return index;
                    }
                }

                return -1;
            }
            else if (comparison < 0)
            {
                return PartitionIndexOf(item, start, Math.Min(mid - start, Count - start));
            }
            else // comparison > 0
            {
                int partitionStart = Math.Min(mid + 1, Count - 1);

                return PartitionIndexOf(item, partitionStart, count - (partitionStart - start));
            }
        }

        /// <summary>
        /// Copies the collection to an array
        /// </summary>
        /// <param name="array">The array index</param>
        /// <param name="arrayIndex">The start index to begin copying in the target array</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified item
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns><c>true</c> if the item was removed, otherwise false.</returns>
        public bool Remove(T item)
        {
            int index = PartitionIndexOf(item, 0, Count);

            if (index > -1)
            {
                this.innerList.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the item at the specified index
        /// </summary>
        /// <param name="index">The zero-based index at which to remove an item.</param>
        public void RemoveAt(int index)
        {
            this.innerList.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }
    }
}
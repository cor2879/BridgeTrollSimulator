/**************************************************************
 * Carousel.cs
 * 
 * Copyright (c) 2023 Old Skool Games
 **************************************************************/
namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class Carousel<TValue> : IEnumerable<TValue>
    {
        private Queue<TValue> innerQueue;

        public Carousel()
        {
            this.InnerQueue = new Queue<TValue>();
        }

        public Carousel(IEnumerable<TValue> collection)
        {
            this.InnerQueue = new Queue<TValue>(collection);
        }

        public Carousel(int capacity)
        {
            this.InnerQueue = new Queue<TValue>(capacity);
        }

        private Queue<TValue> InnerQueue { get; set; }

        public int Count { get => this.InnerQueue.Count; }

        public void Clear()
        {
            this.InnerQueue.Clear();
        }

        public bool Contains(TValue item)
        {
            return this.InnerQueue.Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            this.InnerQueue.CopyTo(array, arrayIndex);
        }

        public TValue GetNext()
        {
            var value = this.InnerQueue.Dequeue();
            this.InnerQueue.Enqueue(value);

            return value;
        }

        public void Add(TValue item)
        {
            this.InnerQueue.Enqueue(item);
        }

        public TValue Peek()
        {
            return this.InnerQueue.Peek();
        }

        public TValue[] ToArray()
        {
            return this.InnerQueue.ToArray();
        }

        public void TrimExcess()
        {
            this.InnerQueue.TrimExcess();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.InnerQueue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerQueue.GetEnumerator();
        }
    }
}

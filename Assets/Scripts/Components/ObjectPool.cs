/**************************************************
 *  ObjectPool.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Defines a data structure that is used to manage the object lifetime
    /// of objects that may need to be regenerated repeatedly.  Objects managed
    /// by an ObjectPool must implement the <see cref="IPoolable" /> interface.
    /// </summary>
    public class ObjectPool
    {
        /// <summary>
        /// The standby queue.  Inactive objects are stored here.
        /// </summary>
        private Queue<IPoolable> standbyQueue = new Queue<IPoolable>();

        /// <summary>
        /// The active pool.  Active objects are stored here.
        /// </summary>
        private HashSet<IPoolable> activePool = new HashSet<IPoolable>();

        public ObjectPool(Guid sessionId)
        {
            this.SessionId = sessionId;
        }

        public IEnumerable<IPoolable> ActiveObjects
        {
            get => this.activePool.ToArray();
        }

        public Guid SessionId { get; private set; }

        /// <summary>
        /// Adds a new poolable object to the ObjectPool
        /// </summary>
        /// <param name="poolable">The poolable.</param>
        public void Add(IPoolable poolable)
        {
            Validator.ArgumentIsNotNull(poolable, nameof(poolable));

            poolable.SetObjectPool(this);
            this.AddToStandbyQueue(poolable);
        }

        /// <summary>
        /// Adds the specified object to the Standby Queue.
        /// </summary>
        /// <param name="poolable">The poolable.</param>
        private void AddToStandbyQueue(IPoolable poolable)
        {
            poolable.GameObject.SetActive(false);
            this.standbyQueue.Enqueue(poolable);
        }

        /// <summary>
        /// Gets the next available object from the Standby Queue.  If no
        /// inactive objects are available, returns null.
        /// </summary>
        /// <returns></returns>
        public IPoolable ActivateNext()
        {
            if (this.standbyQueue.Count > 0)
            {
                var nextPoolable = this.standbyQueue.Dequeue();
                nextPoolable.GameObject.SetActive(true);
                nextPoolable.Reset();
                this.activePool.Add(nextPoolable);

                return nextPoolable;
            }

            return null;
        }

        public IPoolable AddOrActivateNext(Func<MonoBehaviour, GameObject> prefabInstantiator, MonoBehaviour prefab)
        {
            if (this.standbyQueue.Count == 0)
            {
                var item = prefabInstantiator.Invoke(prefab);
                var poolable = item.GetComponent<IPoolable>();
                this.standbyQueue.Enqueue(poolable);
            }

            return this.ActivateNext();
        }

        /// <summary>
        /// Deactivates the specified poolable.
        /// </summary>
        /// <param name="poolable">The poolable.</param>
        public void Deactivate(IPoolable poolable)
        {
            Validator.ArgumentIsNotNull(poolable, nameof(poolable));

            this.activePool.Remove(poolable);
            this.AddToStandbyQueue(poolable);
        }

        /// <summary>
        /// Returns the count of the number of objects currently being maintained by this ObjectPool.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.standbyQueue.Count + this.activePool.Count;
        }
    }
}

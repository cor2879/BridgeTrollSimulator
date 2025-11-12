/**************************************************
 *  IPoolable.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using UnityEngine.Pool;

    /// <summary>
    /// Defines an interface for objects whose lifetime will be managed by
    /// an object pool.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Sets the object pool.
        /// </summary>
        /// <param name="objectPool">The object pool.</param>
        void SetObjectPool(ObjectPool objectPool);

        /// <summary>
        /// Gets the game object.
        /// </summary>
        /// <value>
        /// The game object.
        /// </value>
        GameObject GameObject { get; }

        /// <summary>
        /// Called when the poolable object has completed its task and may be
        /// returned to the pool.
        /// </summary>
        void OnDestinationReached();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        IPoolable GetPrefab();
    }
}
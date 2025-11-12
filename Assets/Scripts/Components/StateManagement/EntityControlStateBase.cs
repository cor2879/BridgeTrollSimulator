/**************************************************
 *  EntityControlStateBase.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components.StateManagement
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    public abstract class EntityControlStateBase<TEntity> where TEntity : EntityBehaviour
    {
        private TEntity entity;

        public TEntity Entity { get; private set; }

        protected EntityControlStateBase(TEntity entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Defines code that will be assigned to the state machine during Update
        /// </summary>
        public virtual void Update() { }

        public virtual void FixedUpdate() { }
    }
}

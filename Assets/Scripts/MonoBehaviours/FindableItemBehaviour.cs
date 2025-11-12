/**************************************************
 *  FindableItemBehaviour.cs
 *  
 *  copyright (c) 2024 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines the base behaviour for items which may be picked up and
    /// added to the player inventory.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    public abstract class FindableItemBehaviour : EntityBehaviour, IFindable, IItemType
    {
        /// <summary>
        /// Gets the type of the arrow.
        /// </summary>
        /// <value>
        /// The type of the arrow.
        /// </value>
        public abstract ItemType ItemType { get; }

        /// <summary>
        /// Handles the encounter.
        /// </summary>
        /// <param name="player">The player.</param>
        public virtual void HandleEncounter(PlayerBehaviour player)
        {
            // Do stuff
            Destroy(this.gameObject);
        }
    }
}

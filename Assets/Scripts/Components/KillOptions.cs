/**************************************************
 *  KillOptions.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines a data structure to be passed into the Kill method of an
    /// entity behaviour in order to specify actions that occur while and
    /// after the kill takes place.
    /// </summary>
    public class KillOptions
    {
        /// <summary>
        /// Gets or sets the flag to hide the sprite renderer when the entity is killed.
        /// </summary>
        public bool HideSpriteRenderer { get; set; }

        /// <summary>
        /// Gets or sets the audio clip name.
        /// </summary>
        /// <value>
        /// The audio clip.
        /// </value>
        public string AudioClip { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Action" /> to execute at the beginning of the kill.
        /// </summary>
        /// <value>
        /// The on kill.
        /// </value>
        public Action<EntityBehaviour> OnKill { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Action" /> to execute if the kill is unsuccessful.
        /// </summary>
        /// <value>
        /// The on kill unsuccessful.
        /// </value>
        public Action OnKillUnsuccessful { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Action" /> to execute at the end of the kill if the
        /// kill is successful.
        /// </summary>
        /// <value>
        /// The on killed.
        /// </value>
        public Action OnKilled { get; set; }

        /// <summary>
        /// Gets or sets the game over condition.
        /// </summary>
        /// <value>
        /// The game over condition.
        /// </value>
        public GameOverCondition GameOverCondition { get; set; }
    }
}

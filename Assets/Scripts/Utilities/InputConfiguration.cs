/**************************************************
 *  InputConfiguration.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    using UnityEngine;

    /// <summary>
    /// Defines some constant values that are used for regognizing certain inputs when
    /// the InputAxes are not adequate or just not defined.
    /// </summary>
    public static class InputConfiguration
    {
        /// <summary>
        /// The zoom in
        /// </summary>
        public static KeyCode ZoomIn = KeyCode.Plus;

        /// <summary>
        /// The zoom in alt
        /// </summary>
        public static KeyCode ZoomInAlt = KeyCode.Equals;

        /// <summary>
        /// The zoom out
        /// </summary>
        public static KeyCode ZoomOut = KeyCode.Minus;

        /// <summary>
        /// The zoom out alt
        /// </summary>
        public static KeyCode ZoomOutAlt = KeyCode.Underscore;

        /// <summary>
        /// The left mouse button
        /// </summary>
        public static int LeftMouseButton = 0;
    }
}

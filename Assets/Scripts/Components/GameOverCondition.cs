/**************************************************
 *  GameOverCondition.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    /// <summary>
    /// Defines a range of values used to indicate under what circumstance a game
    /// over condition was reached.
    /// </summary>
    public enum GameOverCondition
    {
        /// <summary>
        /// The unknown condition
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The victory condition
        /// </summary>
        Victory,

        /// <summary>
        /// The eaten condition
        /// </summary>
        Eaten,

        /// <summary>
        /// The fallen condition
        /// </summary>
        Fallen,

        /// <summary>
        /// The quit condition
        /// </summary>
        Quit
    };
}

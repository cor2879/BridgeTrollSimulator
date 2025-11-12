/**************************************************
 *  GameOverSettings.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Defines a data structure for specifying the state of the endgame.
    /// </summary>
    public class GameOverSettings
    {
        /// <summary>
        /// Gets or sets the game over condition.
        /// </summary>
        /// <value>
        /// The game over condition.
        /// </value>
        public GameOverCondition GameOverCondition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player survived.
        /// </summary>
        /// <value>
        ///   <c>true</c> if survived; otherwise, <c>false</c>.
        /// </value>
        public bool Survived { get; set; }

        public DifficultySetting DifficultySetting { get; set; }

        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public int TotalScore { get; set; }
    }
}

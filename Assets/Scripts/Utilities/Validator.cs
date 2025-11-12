/**************************************************
 *  Validator.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    using System;

    /// <summary>
    /// Defines static methods for validating parameter input
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Indicates that the specified parameter is not null.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <param name="name">The parameter name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <returns><c>true</c> if the argument is not null, otherwise <c>false</c>.</returns>
        public static void ArgumentIsNotNull(object arg, string name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}


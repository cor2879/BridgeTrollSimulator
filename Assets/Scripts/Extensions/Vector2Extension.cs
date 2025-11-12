/**************************************************
 *  Vector2Extension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using UnityEngine;

    /// <summary>
    /// Extends the <see cref="Vector2" /> class.
    /// </summary>
    public static class Vector2Extension
    {
        /// <summary>
        /// Determines whether [is moving west].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if [is moving west] [the specified vector2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMovingWest(this Vector2 vector2)
        {
            return vector2.x < 0;
        }

        /// <summary>
        /// Determines whether [is moving east].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if [is moving east] [the specified vector2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMovingEast(this Vector2 vector2)
        {
            return vector2.x > 0;
        }

        /// <summary>
        /// Determines whether [is moving north].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if [is moving north] [the specified vector2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMovingNorth(this Vector2 vector2)
        {
            return vector2.y > 0;
        }

        /// <summary>
        /// Determines whether [is moving south].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if [is moving south] [the specified vector2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMovingSouth(this Vector2 vector2)
        {
            return vector2.y < 0;
        }

        /// <summary>
        /// Determines whether [is near zero].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if [is near zero] [the specified vector2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNearZero(this Vector2 vector2)
        {
            return Mathf.Abs(vector2.x) <= float.Epsilon && Mathf.Abs(vector2.y) <= float.Epsilon;
        }

        /// <summary>
        /// Determines whether [is below tolerance] [the specified tolerance].
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        ///   <c>true</c> if [is below tolerance] [the specified tolerance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBelowTolerance(this Vector2 vector2, float tolerance)
        {
            return Mathf.Abs(vector2.x) < tolerance && Mathf.Abs(vector2.y) < tolerance;
        }

        /// <summary>
        /// Determines whether this instance is idle.
        /// </summary>
        /// <param name="vector2">The vector2.</param>
        /// <returns>
        ///   <c>true</c> if the specified vector2 is idle; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIdle(this Vector2 vector2)
        {
            return vector2.x == 0 && vector2.y == 0;
        }

        /// <summary>
        /// Gets the slope.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        public static float GetSlope(this Vector2 lhs, Vector2 rhs)
        {
            return (rhs.y - lhs.y) / (rhs.x - lhs.x);
        }
    }
}

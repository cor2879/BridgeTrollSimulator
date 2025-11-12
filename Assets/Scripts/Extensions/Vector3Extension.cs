/**************************************************
 *  Vector3Extension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using UnityEngine;

    /// <summary>
    /// Extends the <see cref="Vector3" /> class
    /// </summary>
    public static class Vector3Extension
    {
        /// <summary>
        /// Adds the angle.
        /// </summary>
        /// <param name="vector3">The vector3.</param>
        /// <param name="degrees">The degrees.</param>
        /// <returns></returns>
        public static Vector3 AddAngle(this Vector3 vector3, float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            var newVector3 = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);

            return vector3 + newVector3;
        }

        /// <summary>
        /// Gets the slope.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        public static float GetSlope(this Vector3 lhs, Vector3 rhs)
        {
            return (rhs.y - lhs.y) / (rhs.x - lhs.x);
        }

        /// <summary>
        /// Gets the intercept.
        /// </summary>
        /// <param name="vector3">The vector3.</param>
        /// <param name="slope">The slope.</param>
        /// <returns></returns>
        public static float GetIntercept(this Vector3 vector3, float slope)
        {
            return vector3.y - (slope * vector3.x);
        }

        /// <summary>
        /// Rounds the specified constant.
        /// </summary>
        /// <param name="vector3">The vector3.</param>
        /// <param name="constant">The constant.</param>
        /// <returns></returns>
        public static Vector3 Round(this Vector3 vector3, float constant)
        {
            return new Vector3(
                Mathf.Round(vector3.x * constant) / constant,
                Mathf.Round(vector3.y * constant) / constant,
                Mathf.Round(vector3.z * constant) / constant);
        }
    }
}
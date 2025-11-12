/**************************************************
 *  Direction.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Defines a data structure for handling and identifying direction
    /// </summary>
    /// <seealso cref="System.IEquatable{OldSchoolGames.BridgeTrollSimulator.Scripts.Components.Direction}" />
    public class Direction : IEquatable<Direction>
    {
        /// <summary>
        /// The north direction
        /// </summary>
        public static readonly Direction North = new Direction()
        {
            Value = DirectionValue.North,
            YValue = 1.0f,
            XValue = 0,
            RotationAxis = new Vector3(0, 0, 0)
        };

        /// <summary>
        /// The south direction
        /// </summary>
        public static readonly Direction South = new Direction()
        {
            Value = DirectionValue.South,
            YValue = -1.0f,
            XValue = 0,
            RotationAxis = new Vector3(0, 0, 0)
        };

        /// <summary>
        /// The east direction
        /// </summary>
        public static readonly Direction East = new Direction()
        {
            Value = DirectionValue.East,
            YValue = 0,
            XValue = 1.0f,
            RotationAxis = new Vector3(0, 0, -1)
        };

        /// <summary>
        /// The west direction
        /// </summary>
        public static readonly Direction West = new Direction()
        {
            Value = DirectionValue.West,
            YValue = 0,
            XValue = -1.0f,
            RotationAxis = new Vector3(0, 0, 1)
        };

        /// <summary>
        /// The idle direction
        /// </summary>
        public static readonly Direction Idle = new Direction() { Value = DirectionValue.Idle, YValue = 0, XValue = 0 };

        /// <summary>
        /// Prevents a default instance of the <see cref="Direction"/> class from being created.
        /// </summary>
        private Direction()
        { }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public DirectionValue Value { get; private set; }

        /// <summary>
        /// Gets the rotation axis.
        /// </summary>
        /// <value>
        /// The rotation axis.
        /// </value>
        public Vector3 RotationAxis { get; private set; }

        /// <summary>
        /// Gets the x value.
        /// </summary>
        /// <value>
        /// The x value.
        /// </value>
        public float XValue { get; private set; }

        /// <summary>
        /// Gets the y value.
        /// </summary>
        /// <value>
        /// The y value.
        /// </value>
        public float YValue { get; private set; }

        /// <summary>
        /// Indicates whether or not this instance is equivalent to a <see cref="Direction" />
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public bool Equals(Direction direction)
        {
            if (((object)direction) == null)
            {
                return false;
            }

            return this.Value == direction?.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Direction);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Direction lhs, Direction rhs)
        {
            if (((object)lhs) == null)
            {
                return rhs == null;
            }

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Direction lhs, Direction rhs)
        {
            if (((object)lhs) == null)
            {
                return rhs != null;
            }

            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Defines the range of possible direction values
        /// </summary>
        public enum DirectionValue
        {
            /// <summary>
            /// The idle
            /// </summary>
            Idle = 0,

            /// <summary>
            /// The north
            /// </summary>
            North = 1,

            /// <summary>
            /// The east
            /// </summary>
            East,

            /// <summary>
            /// The south
            /// </summary>
            South,

            /// <summary>
            /// The west
            /// </summary>
            West
        };

        /// <summary>
        /// Gets the opposing direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static Direction GetOpposingDirection(Direction direction)
        {
            switch (direction?.Value)
            {
                case DirectionValue.North:
                    return South;
                case DirectionValue.South:
                    return North;
                case DirectionValue.East:
                    return West;
                case DirectionValue.West:
                    return East;
                default:
                    return Idle;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

}

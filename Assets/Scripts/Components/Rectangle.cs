/**************************************************
 *  Rectangle.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    /// Defines a simple rectangle with for vertices and four edges.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Gets or sets the upper left.
        /// </summary>
        /// <value>
        /// The upper left.
        /// </value>
        public Vector2 UpperLeft { get; private set; }

        /// <summary>
        /// Gets or sets the upper right.
        /// </summary>
        /// <value>
        /// The upper right.
        /// </value>
        public Vector2 UpperRight { get; private set; }

        /// <summary>
        /// Gets or sets the lower right.
        /// </summary>
        /// <value>
        /// The lower right.
        /// </value>
        public Vector2 LowerRight { get; private set; }

        /// <summary>
        /// Gets or sets the lower left.
        /// </summary>
        /// <value>
        /// The lower left.
        /// </value>
        public Vector2 LowerLeft { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public float Height
        {
            get
            {
                return this.UpperRight.y - this.LowerRight.y;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float Width
        {
            get
            {
                return this.UpperRight.x - this.UpperLeft.x;
            }
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public Vector2 Center
        {
            get
            {
                return new Vector2(
                    this.UpperLeft.x + this.Width / 2,
                    this.LowerLeft.y + this.Height / 2);
            }
        }

        /// <summary>
        /// Gets the upper bound.
        /// </summary>
        /// <value>
        /// The upper bound.
        /// </value>
        public float UpperBound
        {
            get { return this.UpperLeft.y; }
        }

        /// <summary>
        /// Gets the lower bound.
        /// </summary>
        /// <value>
        /// The lower bound.
        /// </value>
        public float LowerBound
        {
            get { return this.LowerLeft.y; }
        }

        /// <summary>
        /// Gets the right bound.
        /// </summary>
        /// <value>
        /// The right bound.
        /// </value>
        public float RightBound
        {
            get { return this.UpperRight.x; }
        }

        /// <summary>
        /// Gets the left bound.
        /// </summary>
        /// <value>
        /// The left bound.
        /// </value>
        public float LeftBound
        {
            get { return this.UpperLeft.x; }
        }

        /// <summary>
        /// Gets the rectangle.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        public static Rectangle GetRectangle(IEnumerable<Vector2> points)
        {
            var minX = points.Select(vector => vector.x).Min();
            var maxX = points.Select(vector => vector.x).Max();
            var minY = points.Select(vector => vector.y).Min();
            var maxY = points.Select(vector => vector.y).Max();

            return new Rectangle()
            {
                UpperLeft = new Vector2(minX, maxY),
                UpperRight = new Vector2(maxX, maxY),
                LowerRight = new Vector2(maxX, minY),
                LowerLeft = new Vector2(minX, minY)
            };
        }
    }
}

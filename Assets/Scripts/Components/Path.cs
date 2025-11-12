namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    using UnityEngine;

    public class Path : IEnumerable<KeyValuePair<Direction, Vector2>>, IComparable<Path>
    {
        private Stack<KeyValuePair<Direction, Vector2>> innerPath;

        private Stack<KeyValuePair<Direction, Vector2>> InnerPath
        {
            get
            {
                if (this.innerPath == null)
                {
                    this.innerPath = new Stack<KeyValuePair<Direction, Vector2>>();
                }

                return this.innerPath;
            }
        }

        public int Length { get => this.InnerPath.Count; }

        public KeyValuePair<Direction, Vector2> Peek()
        {
            return this.InnerPath.Peek();
        }

        public KeyValuePair<Direction, Vector2> PeekLast()
        {
            return this.innerPath.Last();
        }

        public KeyValuePair<Direction, Vector2> Pop()
        {
            return this.InnerPath.Pop();
        }

        public void Push(KeyValuePair<Direction, Vector2> step)
        {
            this.InnerPath.Push(step);
        }

        public IEnumerator<KeyValuePair<Direction, Vector2>> GetEnumerator()
        {
            return this.InnerPath.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerPath.GetEnumerator();
        }

        public static explicit operator Path(Stack<KeyValuePair<Direction, Vector2>> stack)
        {
            var path = new Path();
            path.innerPath = stack;

            return path;
        }

        public int CompareTo(Path other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Length - other.Length;
        }

        public static bool operator >(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return false;
            }

            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator >=(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return ((object)rhs) == null;
            }

            return lhs.CompareTo(rhs) > -1;
        }

        public static bool operator <(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return ((object)rhs) != null;
            }

            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return true;
            }

            return lhs.CompareTo(rhs) < 1;
        }
    }
}

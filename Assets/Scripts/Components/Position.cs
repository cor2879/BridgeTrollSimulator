using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Text.Json;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    public struct Position : IEquatable<Position>
    {
        public static Position Zero = new Position(0.0f, 0.0f, 0.0f);

        public Position(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Z { get; private set; }

        public bool IsAdjacent(Position other)
        {
            // TODO: Implement logic
            return false;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Position Parse(string text)
        {
            return JsonSerializer.Deserialize<Position>(text);
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return this.Equals((Position)obj);
        }

        public bool Equals(Position other)
        {
            return this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^
                this.Y.GetHashCode() ^
                this.Z.GetHashCode();
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            return lhs.X == rhs.X &&
                lhs.Y == rhs.Y &&
                lhs.Z == rhs.Z;
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !(lhs.X == rhs.X) ||
                !(lhs.Y == rhs.Y) ||
                !(lhs.Z == rhs.Z);
        }
    }
}
using System;
using System.Diagnostics;

namespace MarsRover
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Position : IEquatable<Position>
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y) =>
            (X, Y) = (x, y);

        public override bool Equals(object obj) =>
            (obj is Position position && Equals(position));

        public bool Equals(Position other) =>
            (other.X == X && other.Y == Y);

        public void Deconstruct(out int x, out int y) =>
            (x, y) = (X, Y);

        private string DebuggerDisplay =>
            $"( X = {X}, Y = {Y} )";

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X;
            hashCode = hashCode * -1521134295 + Y;
            return hashCode;
        }

        public static bool operator ==(Position left, Position right) =>
            left.Equals(right);

        public static bool operator !=(Position left, Position right) =>
            !left.Equals(right);
    }
}
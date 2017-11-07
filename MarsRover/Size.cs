using System;
using System.Diagnostics;

namespace MarsRover
{
    /// <summary>
    /// Represents the rectangular width and height or an area.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Size : IEquatable<Size>
    {
        public int Width { get; }
        public int Height { get; }
        
        public Size(int width, int height) =>
            (Width, Height) = (width, height);
        
        public override bool Equals(object other) =>
            (other is Size size && Equals(size));
        
        public bool Equals(Size other) =>
            Width == other.Width &&
            Height == other.Height;
        
        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width;
            hashCode = hashCode * -1521134295 + Height;
            return hashCode;
        }

        private string DebuggerDisplay =>
            $"(Width = {Width}, Height = {Height})";
        
        public static bool operator ==(Size left, Size right) =>
            left.Equals(right);
        
        public static bool operator !=(Size left, Size right) =>
            !left.Equals(right);
    }
}
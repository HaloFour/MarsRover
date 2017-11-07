using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover
{
    public class Map
    {
        private readonly Size size;
        private readonly List<IRover> rovers = new List<IRover>();

        public Map(int width, int height) : this(new Size(width, height)) { }
        public Map(Size size)
        {
            this.size = size;
        }

        public void AddRover(IRover rover)
        {
            if (!IsInBounds(rover.Position))
            {
                throw new ArgumentException("The rover position is out of bounds.", nameof(rover));
            }
            if (DetectCollision(rover))
            {
                throw new ArgumentException("Another rover already occupies this position.", nameof(rover));
            }
            rovers.Add(rover);
            rover.Subscribe(position => DetectRoverMovement(rover, position));
        }

        public IReadOnlyList<IRover> Rovers => rovers.AsReadOnly();

        private void DetectRoverMovement(IRover rover, Position position)
        {
            if (!IsInBounds(position))
            {
                throw new InvalidOperationException("The rover has moved out of bounds.");
            }
            if (DetectCollision(rover))
            {
                throw new InvalidOperationException("The rover has collided into another rover.");
            }
        }

        private bool IsInBounds(Position position) =>
            position.X >= 0 &&
            position.X <= size.Width &&
            position.Y >= 0 &&
            position.Y <= size.Height;

        private bool DetectCollision(IRover rover) =>
            rovers.Any(other => other != rover && other.Position == rover.Position);
    }
}
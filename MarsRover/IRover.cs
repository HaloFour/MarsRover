using System;

namespace MarsRover
{
    public interface IRover : IObservable<Position>
    {
        Heading Heading { get; }
        Position Position { get; }
        void MoveForward();
        void TurnLeft();
        void TurnRight();
    }
}
using System;

namespace MarsRover
{
    public class RoverCommandObserver : IObserver<char>
    {
        private readonly IRover rover;

        public RoverCommandObserver(IRover rover)
        {
            this.rover = rover;
        }

        public void OnNext(char value)
        {
            switch (value)
            {
                case 'M':
                case 'm':
                    rover.MoveForward();
                    break;
                case 'L':
                case 'l':
                    rover.TurnLeft();
                    break;
                case 'R':
                case 'r':
                    rover.TurnRight();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value");
            }
        }

        void IObserver<char>.OnCompleted() { }
        void IObserver<char>.OnError(Exception error) { }
    }
}
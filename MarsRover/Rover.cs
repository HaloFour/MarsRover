using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace MarsRover
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Rover : IRover
    {
        private readonly ISubject<Position> subject = new Subject<Position>();
        private Position position;
        private Heading heading;

        public Position Position => position;
        public Heading Heading => heading;

        public Rover(Position position, Heading heading)
        {
            this.position = position;
            this.heading = heading;
        }

        public void MoveForward()
        {
            var (x, y) = position;
            switch (heading)
            {
                case Heading.North:
                    y += 1;
                    break;
                case Heading.East:
                    x += 1;
                    break;
                case Heading.South:
                    y -= 1;
                    break;
                case Heading.West:
                    x -= 1;
                    break;
            }
            position = new Position(x, y);
            subject.OnNext(position);
        }

        public void TurnRight()
        {
            switch (heading)
            {
                case Heading.North:
                    heading = Heading.East;
                    break;
                case Heading.East:
                    heading = Heading.South;
                    break;
                case Heading.South:
                    heading = Heading.West;
                    break;
                case Heading.West:
                    heading = Heading.North;
                    break;
            }
        }

        public void TurnLeft()
        {
            switch (heading)
            {
                case Heading.North:
                    heading = Heading.West;
                    break;
                case Heading.West:
                    heading = Heading.South;
                    break;
                case Heading.South:
                    heading = Heading.East;
                    break;
                case Heading.East:
                    heading = Heading.North;
                    break;
            }
        }

        public override string ToString() =>
            $"{position.X} {position.Y} {heading.ToString()[0]}";

        public IDisposable Subscribe(IObserver<Position> observer) =>
            subject.Subscribe(observer);

        private string DebuggerDisplay =>
            $"( X = {position.X}, Y = {position.Y}, Heading = {heading} )";
    }
}
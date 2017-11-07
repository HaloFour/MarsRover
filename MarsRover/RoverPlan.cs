using System;

namespace MarsRover
{
    public class RoverPlan
    {
        public Position Position { get; }
        public Heading Heading { get; }
        public IObservable<char> Commands { get; }

        public RoverPlan(Position position, Heading heading, IObservable<char> commands)
        {
            Position = position;
            Heading = heading;
            Commands = commands;
        }
    }
}
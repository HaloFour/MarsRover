using Moq;
using NUnit.Framework;
using System;

namespace MarsRover.Test
{
    [TestFixture]
    public class RoverTests
    {
        [Test]
        public void NewRoverInstanceReportsInitialPosition()
        {
            var expectedPosition = new Position(2, 3);
            var expectedHeading = Heading.South;

            var rover = new Rover(new Position(2, 3), Heading.South);

            Assert.AreEqual(expectedPosition, rover.Position);
            Assert.AreEqual(expectedHeading, rover.Heading);
        }

        [Test]
        public void RoverShouldMoveForward()
        {
            var expectedPosition = new Position(2, 4);
            var expectedHeading = Heading.North;

            var rover = new Rover(new Position(2, 3), Heading.North);

            rover.MoveForward();
            
            Assert.AreEqual(expectedPosition, rover.Position);
            Assert.AreEqual(expectedHeading, rover.Heading);
        }

        [Test]
        public void RoverShouldTurnLeft()
        {
            var expectedHeading = Heading.West;
            var rover = new Rover(new Position(1, 1), Heading.North);

            rover.TurnLeft();

            Assert.AreEqual(expectedHeading, rover.Heading);
        }

        [Test]
        public void RoverShouldTurnRight()
        {
            var expectedHeading = Heading.East;
            var rover = new Rover(new Position(1, 1), Heading.North);

            rover.TurnRight();

            Assert.AreEqual(expectedHeading, rover.Heading);
        }

        [Test]
        public void RoverShouldReportMovement()
        {
            var rover = new Rover(new Position(1, 1), Heading.East);
            var mock = new Mock<IObserver<Position>>(MockBehavior.Strict);
            var sequence = new MockSequence();
            mock.InSequence(sequence).Setup(m => m.OnNext(It.Is<Position>(p => p == new Position(2, 1))));
            mock.InSequence(sequence).Setup(m => m.OnNext(It.Is<Position>(p => p == new Position(3, 1))));
            mock.InSequence(sequence).Setup(m => m.OnNext(It.Is<Position>(p => p == new Position(3, 2))));

            using (rover.Subscribe(mock.Object))
            {
                rover.MoveForward();
                rover.MoveForward();
                rover.TurnLeft();
                rover.MoveForward();
            }
            rover.MoveForward();
        }

    }
}

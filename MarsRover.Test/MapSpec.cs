using System;
using Moq;
using NUnit.Framework;

namespace MarsRover.Test
{
    [TestFixture]
    public class MapSpec
    {
        [Test]
        public void AddRoverSubscribesToMovement()
        {
            var mock = new Mock<IRover>();
            mock.Setup(m => m.Position).Returns(new Position(1, 1));
            var map = new Map(new Size(5, 5));
            map.AddRover(mock.Object);
            mock.Verify(m => m.Subscribe(It.IsAny<IObserver<Position>>()), Times.Once);
        }

        [Test]
        public void AddRoverOutOfBoundsThrowsException()
        {
            var map = new Map(new Size(5, 5));
            var rover = new Rover(new Position(10, 3), Heading.North);
            Assert.Throws<ArgumentException>(() =>
            {
                map.AddRover(rover);
            });
        }

        [Test]
        public void AddRoverOnOtherRoverThrowsException()
        {
            var map = new Map(new Size(5, 5));
            var rover1 = new Rover(new Position(2, 2), Heading.North);
            map.AddRover(rover1);

            Assert.Throws<ArgumentException>(() =>
            {
                var rover2 = new Rover(new Position(2, 2), Heading.North);
                map.AddRover(rover2);
            });
        }

        [Test]
        public void MapDetectsRoverMovingOutOfBounds()
        {
            var map = new Map(new Size(5, 5));
            var rover = new Rover(new Position(0, 0), Heading.South);
            map.AddRover(rover);
            Assert.Throws<InvalidOperationException>(() =>
            {
                rover.MoveForward();
            });
        }

        [Test]
        public void MapDetectsRoverCollision()
        {
            var map = new Map(new Size(5, 5));
            var rover1 = new Rover(new Position(2, 2), Heading.North);
            var rover2 = new Rover(new Position(1, 2), Heading.East);
            map.AddRover(rover1);
            map.AddRover(rover2);
            Assert.Throws<InvalidOperationException>(() =>
            {
                rover2.MoveForward();
            });
        }
    }
}
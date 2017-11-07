using System;
using NUnit.Framework;
using Moq;

namespace MarsRover.Test
{
    [TestFixture]
    public class RoverCommandObserverSpec
    {
        private Mock<IRover> mock;
        private RoverCommandObserver interpreter;

        [SetUp]
        public void SetUp()
        {
            mock = new Mock<IRover>(MockBehavior.Strict);
            interpreter = new RoverCommandObserver(mock.Object);
        }

        [Test]
        public void InterpretsMoveForward()
        {
            mock.Setup(m => m.MoveForward());
            interpreter.OnNext('M');
            mock.Verify(r => r.MoveForward(), Times.Once);
        }

        [Test]
        public void InterpretsTurnLeft()
        {
            mock.Setup(m => m.TurnLeft());
            interpreter.OnNext('L');
            mock.Verify(r => r.TurnLeft(), Times.Once);
        }

        [Test]
        public void InterpretsTurnRight()
        {
            mock.Setup(m => m.TurnRight());
            interpreter.OnNext('R');
            mock.Verify(r => r.TurnRight(), Times.Once);
        }

        [Test]
        public void InterpretsSeriesOfCommands()
        {
            var sequence = new MockSequence();
            mock.InSequence(sequence).Setup(m => m.MoveForward());
            mock.InSequence(sequence).Setup(m => m.TurnLeft());
            mock.InSequence(sequence).Setup(m => m.TurnRight());

            interpreter.OnNext('M');
            interpreter.OnNext('L');
            interpreter.OnNext('R');

            mock.Verify(r => r.MoveForward(), Times.Once);
            mock.Verify(r => r.TurnLeft(), Times.Once);
            mock.Verify(r => r.TurnRight(), Times.Once);
        }

        [Test]
        public void InterpretsSeriesOfCommandsCaseInsensitive()
        {
            var sequence = new MockSequence();
            mock.InSequence(sequence).Setup(m => m.MoveForward());
            mock.InSequence(sequence).Setup(m => m.TurnLeft());
            mock.InSequence(sequence).Setup(m => m.TurnRight());

            interpreter.OnNext('m');
            interpreter.OnNext('l');
            interpreter.OnNext('r');

            mock.Verify(r => r.MoveForward(), Times.Once);
            mock.Verify(r => r.TurnLeft(), Times.Once);
            mock.Verify(r => r.TurnRight(), Times.Once);
        }

        [Test]
        public void InvalidCommandThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                interpreter.OnNext('F');
            });
        }
    }
}

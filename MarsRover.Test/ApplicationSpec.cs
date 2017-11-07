using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Test
{
    [TestFixture]
    public class ApplicationSpec
    {
        private Application application;

        [SetUp]
        public void SetUp()
        {
            var interpreter = new InputCommandInterpreter();
            Func<Position, Heading, IRover> roverFactory = (position, heading) => new Rover(position, heading);
            Func<IRover, IObserver<char>> observerFactory = rover => new RoverCommandObserver(rover);
            application = new Application(interpreter, roverFactory, observerFactory);
        }

        [Test]
        public async Task TestSingleRoverLocationNoCommands()
        {
            string expected = FormatLines("1 1 N");

            string input = FormatLines("5 5", "1 1 N");
            var reader = new StringReader(input);
            var writer = new StringWriter();

            await application.Execute(reader, writer);

            var results = writer.ToString();
            Assert.AreEqual(expected, results);
        }

        [Test]
        public async Task TestMultipleRoverLocationsNoCommands()
        {
            string expected = FormatLines("1 1 N", "2 2 S");

            string input = FormatLines("5 5", "1 1 N", "", "2 2 S");
            var results = await ProcessInput(input);

            Assert.AreEqual(expected, results);
        }

        [Test]
        public void TestRoverInitialOutOfBounds()
        {
            string input = FormatLines("5 5", "6 1 N");

            Assert.ThrowsAsync<ArgumentException>(() =>
                ProcessInput(input),
            "The rover position is out of bounds.");
        }

        [Test]
        public void TestRoverInitialCollision()
        {
            string input = FormatLines("5 5", "2 2 N", "", "2 2 S");

            Assert.ThrowsAsync<ArgumentException>(() =>
                ProcessInput(input),
            "Another rover already occupies this position.");
        }

        [Test]
        public void TestRoverMovesOutOfBounds()
        {
            string input = FormatLines("5 5", "1 1 W", "MM");
            
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                ProcessInput(input),
            "The rover has moved out of bounds.");
        }

        [Test]
        public void TestRoverCollision()
        {
            string input = FormatLines("5 5", "1 1 E", "", "2 1 N", "LM");
            
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                ProcessInput(input),
            "The rover has collided into another rover.");
        }

        [Test]
        public async Task TestExampleData()
        {
            string expected = FormatLines("1 3 N", "5 1 E");

            string input = FormatLines("5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM");

            var results = await ProcessInput(input);
            Assert.AreEqual(expected, results);
        }

        private async Task<string> ProcessInput(string input)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            await application.Execute(reader, writer);

            return writer.ToString();
        }

        private string FormatLines(params string[] values)
        {
            var builder = new StringBuilder();
            foreach (var value in values)
            {
                builder.AppendLine(value);
            }
            return builder.ToString();
        }
    }
}
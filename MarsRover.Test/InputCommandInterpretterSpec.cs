using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;

namespace MarsRover.Test
{
    [TestFixture]
    public class InputCommandInterpreterSpec
    {
        private InputCommandInterpreter interpreter;

        [SetUp]
        public void SetUp()
        {
            interpreter = new InputCommandInterpreter();
        }

        [Test]
        public void InterpretMapSize()
        {
            var input = "5 6";

            var result = interpreter.TryParseMapSize(input, out var size);

            Assert.AreEqual(true, result);
            Assert.AreEqual(new Size(5, 6), size);
        }

        [Test]
        public void InterpretInvalidSize()
        {
            var input = "5 foo";

            var result = interpreter.TryParseMapSize(input, out var size);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void InterpretRoverBearings()
        {
            var input = "2 3 E";

            var result = interpreter.TryParseRover(input, out var position, out var heading);

            Assert.AreEqual(true, result);
            Assert.AreEqual(new Position(2, 3), position);
            Assert.AreEqual(Heading.East, heading);
        }

        [Test]
        public void InterpretInvalidRoverBearings()
        {
            var input = "2 3 X";

            var result = interpreter.TryParseRover(input, out var position, out var heading);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void InterpretRoverCommands()
        {
            var input = "MRL";

            var result = interpreter.TryParseRoverCommands(input, out var commands);

            Assert.AreEqual(true, result);
            Assert.IsNotNull(commands);
            var array = commands.ToEnumerable().ToArray();

            Assert.AreEqual(new[] { 'M', 'R', 'L' }, array);
        }

        [Test]
        public void InterpretInvalidRoverCommands()
        {
            var input = "FOO";

            var result = interpreter.TryParseRoverCommands(input, out var commands);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void InterpretNullRoverCommands()
        {
            string input = null;
            var result = interpreter.TryParseRoverCommands(input, out var commands);

            Assert.AreEqual(true, result);
            Assert.IsNotNull(commands);
            var array = commands.ToEnumerable().ToArray();

            Assert.IsEmpty(array);
        }
    }
}
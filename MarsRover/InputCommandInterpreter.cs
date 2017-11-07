using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRover
{
    public class InputCommandInterpreter : IInputCommandInterpreter
    {
        private static readonly Regex MAP_SIZE_PATTERN;
        private static readonly Regex ROVER_INIT_PATTERN;
        private static readonly Regex ROVER_COMMAND_PATTERN;
        private static readonly Dictionary<string, Heading> HEADINGS;

        static InputCommandInterpreter()
        {
            MAP_SIZE_PATTERN = new Regex(@"^(\d+) (\d+)$");
            ROVER_INIT_PATTERN = new Regex(@"^(\d+) (\d+) (?i)(N|S|E|W)$");
            ROVER_COMMAND_PATTERN = new Regex(@"^(?i)((?:M|R|L|\s)*)$");
            HEADINGS = new Dictionary<string, Heading>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "N", Heading.North },
                { "E", Heading.East },
                { "S", Heading.South },
                { "W", Heading.West }
            };
        }

        public bool TryParseMapSize(string input, out Size size)
        {
            var match = MAP_SIZE_PATTERN.Match(input);
            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out var width) && int.TryParse(match.Groups[2].Value, out var height))
                {
                    size = new Size(width, height);
                    return true;
                }
            }
            size = default(Size);
            return false;
        }

        public bool TryParseRover(string input, out Position position, out Heading heading)
        {
            var match = ROVER_INIT_PATTERN.Match(input);
            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out var x) && int.TryParse(match.Groups[2].Value, out var y))
                {
                    position = new Position(x, y);
                    return HEADINGS.TryGetValue(match.Groups[3].Value, out heading);
                }
            }
            position = default(Position);
            heading = default(Heading);
            return false;
        }

        public bool TryParseRoverCommands(string input, out IObservable<char> commands)
        {
            if (input == null)
            {
                commands = Observable.Empty<char>();
                return true;
            }

            var match = ROVER_COMMAND_PATTERN.Match(input);
            if (match.Success)
            {
                commands = input.ToObservable(Scheduler.Immediate);
                return true;
            }

            commands = null;
            return false;
        }

        public async Task<RoverInput> ParseRoverInput(TextReader reader)
        {
            var input = await reader.ReadLineAsync();
            if (!TryParseMapSize(input, out var size))
            {
                throw new InvalidOperationException("Unable to parse map data from input \"" + input + "\".");
            }
            var plans = new List<RoverPlan>();
            input = await reader.ReadLineAsync();
            var line = 1;
            while (!string.IsNullOrEmpty(input))
            {
                if (!TryParseRover(input, out var position, out var heading))
                {
                    throw new InvalidOperationException("Unable to parse rover bearing from input \"" + input + "\" at line " + line + ".");
                }

                input = await reader.ReadLineAsync();
                if (!TryParseRoverCommands(input, out var commands))
                {
                    throw new InvalidOperationException("Unable to parse rover command string from input \"" + input + "\" at line " + line + ".");
                }
                plans.Add(new RoverPlan(position, heading, commands));

                input = await reader.ReadLineAsync();
                line += 2;
            }
            return new RoverInput(size, plans);
        }
    }
}
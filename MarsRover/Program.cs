using System;
using System.Threading.Tasks;

namespace MarsRover {
    static class Program
    {
        static async Task Main() {
            var interpreter = new InputCommandInterpreter();
            Func<Position, Heading, IRover> roverFactory = (position, heading) => new Rover(position, heading);
            Func<IRover, IObserver<char>> roverCommandObserverFactory = rover => new RoverCommandObserver(rover);

            var application = new Application(interpreter, roverFactory, roverCommandObserverFactory);
            await application.Execute(Console.In, Console.Out);
        }
    }
}
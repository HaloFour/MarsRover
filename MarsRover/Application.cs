using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MarsRover
{
    public class Application
    {
        private readonly IInputCommandInterpreter interpreter;
        private readonly Func<Position, Heading, IRover> roverFactory;
        private readonly Func<IRover, IObserver<char>> commandObserverFactory;

        public Application(
            IInputCommandInterpreter interpreter,
            Func<Position, Heading, IRover> roverFactory,
            Func<IRover, IObserver<char>> commandObserverFactory
        )
        {
            this.interpreter = interpreter;
            this.roverFactory = roverFactory;
            this.commandObserverFactory = commandObserverFactory;
        }
        

        public IObservable<string> Execute(TextReader reader) =>
            Observable.Create<string>(async observer =>
            {
                var input = await interpreter.ParseRoverInput(reader);
                var map = new Map(input.Size);
                var plans = input.RoverPlans;

                var count = plans.Count;
                var rovers = new IRover[count];
                var observers = new IObserver<char>[count];

                // First pass:  Create the rovers from the input rover plans
                // and add them to the map.
                for (var i = 0; i < count; i++)
                {
                    var plan = plans[i];
                    var rover = roverFactory(plan.Position, plan.Heading);
                    map.AddRover(rover);
                    rovers[i] = rover;
                    observers[i] = commandObserverFactory(rover);
                }

                // Second pass:  Execute the commands for each rover
                // and emit their final position sequentially.
                for (var i = 0; i < count; i++)
                {
                    var plan = plans[i];
                    var commands = plan.Commands;
                    var rover = rovers[i];
                    var commandObserver = observers[i];
                    commands.Subscribe(commandObserver);
                    observer.OnNext(rover.ToString());
                }

                observer.OnCompleted();

                return Disposable.Empty;
            });

        public Task Execute(TextReader reader, TextWriter writer)
        {
            return Execute(reader)
                .ForEachAsync(result => writer.WriteLine(result));
        }
    }
}
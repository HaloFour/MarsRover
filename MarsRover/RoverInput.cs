using System;
using System.Collections.Generic;

namespace MarsRover
{
    public class RoverInput
    {
        public Size Size { get; }
        public IReadOnlyList<RoverPlan> RoverPlans { get; }

        public RoverInput(Size size, List<RoverPlan> plans)
        {
            Size = size;
            RoverPlans = plans.AsReadOnly();
        }
    }
}
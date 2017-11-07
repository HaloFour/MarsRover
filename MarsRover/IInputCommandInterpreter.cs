using System.IO;
using System.Threading.Tasks;

namespace MarsRover
{
    public interface IInputCommandInterpreter
    {
        Task<RoverInput> ParseRoverInput(TextReader reader);
    }
}
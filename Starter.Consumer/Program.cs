using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Starter.Bootstrapper;

namespace Starter.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Setup.BootstrapHost().RunConsoleAsync();
        }
    }
}



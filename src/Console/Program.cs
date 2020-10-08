using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole;
using System;
using System.Threading.Tasks;

namespace OrderParser
{
    class Program
    {
        static int Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var commandLineConfig = scope.Resolve<CommandLineConfig>();
                var commandLineApp = commandLineConfig.Configure();

                return commandLineApp.Execute(args);
            }
        }
    }
}

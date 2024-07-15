using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using DevFun.DbMigration.Cli.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DevFun.DbMigration.Cli
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            Parser runner = BuildCommandLine()
                .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
                {
                    builder.UseEnvironment("CLI")
                        .ConfigureLogging((context, logging) =>
                        {
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Information);
                            logging.AddConsole();
                            logging.AddFilter((category, logLevel) =>
                            {
                                // ignore Microsoft info logs
                                if (category.StartsWith("Microsoft", System.StringComparison.OrdinalIgnoreCase)
                                    && logLevel < LogLevel.Warning)
                                {
                                    return false;
                                }

                                return true;
                            });
                        })
                        .ConfigureServices((hostContext, services) =>
                        {

                        })
                        .UseCommandHandler<DeployDbSchemaCommand, DeployDbSchemaCommand.Handler>();
                }).UseDefaults().Build();

            return runner.InvokeAsync(args);
        }

        private static CommandLineBuilder BuildCommandLine()
        {
            RootCommand root = new();

            root.AddCommand(new DeployDbSchemaCommand());

            root.Handler = CommandHandler.Create(async () =>
            {
                await root.InvokeAsync("-h").ConfigureAwait(false);
            });

            return new CommandLineBuilder(root);
        }

    }
}

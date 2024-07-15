using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace DevFun.DbMigration.Cli.Commands
{
    internal class DeployDbSchemaCommand : Command
    {
        public DeployDbSchemaCommand() : base("deploy", "Deploy the database schema with DacPac")
        {
            this.AddOption(new Option<string>(new[] { "--connectionstring", "-c" }, "Connection String"));
            this.AddOption(new Option<string>(new[] { "--server", "-s" }, "Database Server"));
            this.AddOption(new Option<string>(new[] { "--database", "-d" }, "Database"));
            this.AddOption(new Option<string>(new[] { "--user", "-u" }, "User"));
            this.AddOption(new Option<string>(new[] { "--password", "-p" }, "Password"));
            this.AddOption(new Option<string>(new[] { "--dacpacfile", "-f" }, "DacPac File"));
        }

        public new class Handler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                var connectionString = context.ParseResult.ValueForOption<string>("--connectionstring");
                if (string.IsNullOrEmpty(connectionString))
                {
                    return HandleDbDeployment(context.ParseResult.ValueForOption<string>("--server"), context.ParseResult.ValueForOption<string>("--database"), context.ParseResult.ValueForOption<string>("--user") , context.ParseResult.ValueForOption<string>("--password"), context.ParseResult.ValueForOption<string>("--dacpacfile"));
                }
                else
                {
                    return HandleDbDeployment(connectionString, context.ParseResult.ValueForOption<string>("--dacpacfile"));
                }
            }

            private static async Task<int> HandleDbDeployment(string server, string database, string user, string password, string dacpacFile)
            {
                var deployer = new DbSchemaDeployer();
                var result = await deployer.DeployDatabase(server, database, user, password, dacpacFile).ConfigureAwait(false);
                return result ? 0 : 1;
            }


            private static async Task<int> HandleDbDeployment(string connectionString, string dacpacFile)
            {
                var deployer = new DbSchemaDeployer();
                var result = await deployer.DeployDatabase(connectionString, dacpacFile).ConfigureAwait(false);
                return result ? 0 : 1;
            }
        }
    }
}

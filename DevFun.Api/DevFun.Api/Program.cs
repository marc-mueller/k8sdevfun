using ReaFx.DependencyInjection.AutofacAdapter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DevFun.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .SetupAutofac()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    /// <summary>
    /// Factory for swagger auto-generation when using autofac
    /// </summary>
    /// <remarks>
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcorecli
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1180
    /// </remarks>
    public static class SwaggerHostFactory
    {
        public static IHost CreateHost()
        {
            return Program.CreateHostBuilder(System.Array.Empty<string>()).Build();
        }
    }
}
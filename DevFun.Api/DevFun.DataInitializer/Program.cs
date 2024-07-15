using DevFun.Clients.V1_0;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging.Abstractions;
using ReaFx.ApiClients.Common.Factory;
using ReaFx.ApiClients.Common.Options;
using ReaFx.ApiClients.Common.Settings;

namespace DevFun.DataInitializer
{
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "Sets the base URL of the greeter service. Default is http://localhost:4567.", LongName = "service", ShortName = "s")]
        public string ServiceBaseUrl { get; } = "http://localhost:4567";

        private void OnExecute()
        {
            ServiceEndpointSettingsBase endpointSettings = new();
            endpointSettings.TlsCertificateValidationConfiguration.AcceptAnyServerCertificates = true;
            endpointSettings.SetUri<IDevFunServiceClient>(new Uri(ServiceBaseUrl));


            CustomTokenHandler tokenHandler = new(new NullLogger<CustomTokenHandler>(), endpointSettings);
            ServiceClientFactory factory = new(endpointSettings, tokenHandler, new NullLogger<ServiceClientFactory>(), Enumerable.Empty<IServiceClientOptionProvider>());
            IDevFunServiceClient client = factory.GetClientForService<IDevFunServiceClient>().ConfigureAwait(false).GetAwaiter().GetResult();

            DataInitializer dataInitializer = new(client);
            dataInitializer.InitializeData().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Data initialized.");
        }
    }
}
using DevFun.Clients.V1_0;
using DevFun.Common.Model.Dtos.V1_0;
using DevFun.Web.Options;
using Microsoft.AspNetCore.Mvc;
using ReaFx.ApiClients.Common.Factory;
using ReaFx.ApiClients.Common.Settings;

namespace DevFun.Web.Controllers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "ok for sample")]
    public class HomeController : Controller
    {
        private readonly DevFunOptions apiOptions;
        private readonly ILogger<HomeController> logger;
        private readonly HttpClientHandler clientHandler;
        private readonly HttpClient httpClient;
        private readonly IServiceClientFactory serviceClientFactory;
        private readonly IServiceEndpointSettings endpointSettings;

        public HomeController(IServiceClientFactory serviceClientFactory, IServiceEndpointSettings endpointSettings, DevFunOptions apiOptions, ILogger<HomeController> logger)
        {
            this.serviceClientFactory = serviceClientFactory ?? throw new ArgumentNullException(nameof(serviceClientFactory));
            this.endpointSettings = endpointSettings ?? throw new ArgumentNullException(nameof(endpointSettings));
            this.apiOptions = apiOptions ?? throw new ArgumentNullException(nameof(apiOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            ViewData["FlagEnableAlternateUrl"] = apiOptions.FlagEnableAlternateUrl;
            bool useAlternateUrl = Request.Query.ContainsKey("useAlternateUrl") && bool.Parse(Request.Query["useAlternateUrl"]);
            ViewData["UseAlternateUrl"] = useAlternateUrl;
            endpointSettings.SetUri<IDevFunServiceClient>(new Uri(useAlternateUrl ? apiOptions.AlternateTestingUrl : apiOptions.ApiUrl));
            DevJokeDto joke = await GetRandomJoke().ConfigureAwait(false);
            if (joke != null && joke.CategoryName == null)
            {
                joke.CategoryName = (await GetCategory(joke.CategoryId).ConfigureAwait(false))?.Name;
            }

            return View(joke);
        }

        public IActionResult About()
        {
            ViewData["DeploymentEnvironment"] = apiOptions.DeploymentEnvironment;

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            httpClient?.Dispose();
            clientHandler?.Dispose();

            base.Dispose(disposing);
        }

        private async Task<DevJokeDto> GetRandomJoke()
        {
            DevJokeDto devJoke = null;
            try
            {
                IDevFunServiceClient client = await serviceClientFactory.GetClientForService<IDevFunServiceClient>().ConfigureAwait(false);
                devJoke = (await client.GetRandomAsync().ConfigureAwait(false)).Result;
            }
            catch (Exception ex)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                logger.LogError(ex, $"Error occurred while get random joke: {ex.Message}");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }
            return devJoke;
        }

        private async Task<CategoryDto> GetCategory(int id)
        {
            CategoryDto category = null;
            try
            {
                IDevFunServiceClient client = await serviceClientFactory.GetClientForService<IDevFunServiceClient>().ConfigureAwait(false);
                category = (await client.GetCategoryAsync(id).ConfigureAwait(false)).Result;
            }
            catch (Exception ex)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                logger.LogError(ex, $"Error occurred while get category: {ex.Message}");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }
            return category;
        }
    }
}
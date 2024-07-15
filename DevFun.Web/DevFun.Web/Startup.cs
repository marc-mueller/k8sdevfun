using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DevFun.Clients.V1_0;
using DevFun.Web.Health;
using DevFun.Web.Options;
using DevFun.Web.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;
using ReaFx.ApiClients.Common.Factory;
using ReaFx.ApiClients.Common.Settings;
using ReaFx.ApiClients.Common.Tokens;

namespace DevFun.Web
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "needed by design")]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddHealthChecks()
                .AddCheck<OperationalHealthCheck>("operational_health_check", tags: new[] { HealthCheckTags.Liveness })
                .AddCheck<BackendConnectivityHealthCheck>("backendconnectivity_health_check", tags: new[] { HealthCheckTags.Readiness })
                .ForwardToPrometheus();

            // Add framework services.
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages();

            services.AddSingleton<IConfiguration>(Configuration);
            var devfunOptions = new DevFunOptions()
            {
                ApiUrl = Configuration["DevFunOptions:Url"],
                DeploymentEnvironment = Configuration["DevFunOptions:DeploymentEnvironment"],
                AlternateTestingUrl = Configuration["DevFunOptions:AlternateTestingUrl"],
                FlagEnableAlternateUrl = bool.Parse(Configuration["DevFunOptions:FlagEnableAlternateUrl"])
            };
            services.AddSingleton<DevFunOptions>(devfunOptions);


            var apiServiceUri = new Uri(devfunOptions.ApiUrl);
            var endpointSettings = new ServiceEndpointSettingsBase
            {
                TlsCertificateValidationConfiguration = new TlsCertificateValidationConfiguration(){ AcceptAnyServerCertificates = devfunOptions.DeploymentEnvironment.Equals("Production", StringComparison.OrdinalIgnoreCase) ? false : true }
            };
            endpointSettings.SetUri<IDevFunServiceClient>(new Uri(devfunOptions.ApiUrl));
            services.AddSingleton<IServiceEndpointSettings>(endpointSettings);

            services.AddTransient<IServiceClientFactory, ServiceClientFactory>();
            services.AddTransient<ITokenHandler, CustomTokenHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = WriteHealthResponse,
                    Predicate = (check) => check.Tags.Contains(HealthCheckTags.Liveness)
                });
                endpoints.MapHealthChecks("/health/startup", new HealthCheckOptions()
                {
                    ResponseWriter = WriteHealthResponse,
                    Predicate = (check) => check.Tags.Contains(HealthCheckTags.Startup)
                });
                endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions()
                {
                    ResponseWriter = WriteHealthResponse,
                    Predicate = (check) => check.Tags.Contains(HealthCheckTags.Liveness) || check.Tags.Contains(HealthCheckTags.Readiness)
                });
                endpoints.MapMetrics();
            });
        }

        private static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteStartObject("results");
                    foreach (var entry in result.Entries)
                    {
                        writer.WriteStartObject(entry.Key);
                        writer.WriteString("status", entry.Value.Status.ToString());
                        writer.WriteString("description", entry.Value.Description);
                        writer.WriteStartObject("data");
                        foreach (var item in entry.Value.Data)
                        {
                            writer.WritePropertyName(item.Key);
                            JsonSerializer.Serialize(
                                writer, item.Value, item.Value?.GetType() ??
                                                    typeof(object));
                        }
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return context.Response.WriteAsync(json);
            }
        }
    }
}
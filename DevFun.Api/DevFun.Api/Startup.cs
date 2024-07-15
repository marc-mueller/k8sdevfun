using System.Diagnostics;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevFun.Api.Extensions;
using DevFun.Api.Health;
using DevFun.Common.Model.Validators.V1_0;
using DevFun.Logic.Modularity;
using DevFun.Logic.Security;
using DevFun.Storage.Modularity;
using DevFun.Storage.Storages;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Prometheus;
using ReaFx.ApiServices.Common.Middleware;
using ReaFx.ApiServices.Common.Swagger;
using ReaFx.DataAccess.Common.Security;
using ReaFx.DataAccess.EntityFramework.Storages;
using ReaFx.DependencyInjection.Microsoft.Common;
using ReaFx.DependencyInjection.Microsoft.Server;
using ReaFx.Modularity.Common;

namespace DevFun.Api
{
    public class Startup : ReaFx.DependencyInjection.AutofacAdapter.StartupBase
    {
        private ILogger<Startup> logger; // todo
        private IWebHostEnvironment CurrentEnvironment { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            CurrentEnvironment = env ?? throw new ArgumentNullException(nameof(env));
            Configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
            }

            // add licensing
            services.AddLicensing(Configuration);

            services.AddApplicationInsightsTelemetry(); // todo
            services.AddHealthChecks()
                .AddCheck<StartupHealthCheck>("startup_health_check", tags: new[] { HealthCheckTags.Startup })
                .AddCheck<OperationalHealthCheck>("operational_health_check", tags: new[] { HealthCheckTags.Liveness })
                .AddCheck<CacheReadyHealthCheck>("cacheready_health_check", tags: new[] { HealthCheckTags.Readiness })
                .AddCheck<DatabaseConnectivityHealthCheck>("database_health_check", tags: new[] { HealthCheckTags.Readiness })
                .ForwardToPrometheus();

            // Use jwt bearer authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration["BaseServiceSettings:Authority"];
                    options.Audience = "DevFunApi";
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            await Task.CompletedTask.ConfigureAwait(false);
                        },
                        OnMessageReceived = async context =>
                        {
                            await Task.CompletedTask.ConfigureAwait(false);
                        },
                        OnTokenValidated = async context =>
                        {
                            await Task.CompletedTask.ConfigureAwait(false);
                        },
                        OnForbidden = async context =>
                        {
                            await Task.CompletedTask.ConfigureAwait(false);
                        },
                        OnAuthenticationFailed = async context =>
                        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                            logger?.LogError(context?.Exception, $"Error occurred on authentication: {context?.Exception?.Message}");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                            await Task.CompletedTask.ConfigureAwait(false);
                        }
                    };
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        {
                            if (CurrentEnvironment.IsDevelopment())
                            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                                logger.LogWarning($"Accepting invalid SSL Certificate ({sslPolicyErrors})");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                                return true;
                            }

                            return sslPolicyErrors == SslPolicyErrors.None;
                        }
                    };
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });


            // Add framework services.
            services.AddCors();

            services
                .AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase, allowIntegerValues: false));
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<DevJokeDtoValidator>());

            services.AddHttpContextAccessor();

            services.AddApiVersioningAndSwaggerGen("SimpleTraders API", apiVersionFormat: "VVVV");

            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<DevFunStorage>((serviceProvider, options) =>
            {
                if (!bool.Parse(Configuration["BaseServiceSettings:UseInMemoryDatabase"]))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DevFunDatabase"));
                }
                else
                {
                    options.UseInMemoryDatabase(databaseName: "DevFunDatabase");
                }

                options.AddInterceptors(new AzureAuthenticationInterceptor());

                options
                    .ConfigureWarnings(warnings =>
                    {
                        Microsoft.EntityFrameworkCore.Diagnostics.WarningsConfigurationBuilder configuredWarnings = warnings.Default(WarningBehavior.Log);
                    })
                    .UseInternalServiceProvider(serviceProvider)
                    .EnableSensitiveDataLogging();
            });

            // init modules
            services.AddScoped<ISecurityContext, SecurityContext>();

            services.AddModulesFromConfiguration(Configuration, InitializeModuleCatalog(services));

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DevFunStorage storage, ILogger<Startup> logger)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (Debugger.IsAttached)
            {
                if (!bool.Parse(Configuration["BaseServiceSettings:UseInMemoryDatabase"]))
                {
                    app.EnsureSqlTablesAreCreated<DevFunStorage, IRelationalEntityConfiguration>();
                }
                else
                {
                    storage.Database.EnsureCreated();
                }
            }

            app.UseCustomExceptionHandling();

            app.UseRouting();
            app.UseHttpMetrics();

            app.UseCors(policy => policy
                .SetIsOriginAllowed(host => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("*"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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

            app.UseSwaggerAndSwaggerUi("SimpleTraders API");

            //app.UseWelcomePage();
        }

        private static IModuleCatalogCollection InitializeModuleCatalog(IServiceCollection services)
        {
            IModuleCatalogCollection moduleCatalog = services.InitializeModuleCatalog();
            moduleCatalog.AddDevFunLogicModule();
            moduleCatalog.AddDevFunStorageModule();
            moduleCatalog.AddCustomLabelServiceModule();
            moduleCatalog.AddCustomViewDefinitionServiceModule();
            //moduleCatalog.AddAuditTrailModule();
            return moduleCatalog;
        }

        private static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            JsonWriterOptions options = new()
            {
                Indented = true
            };

            using MemoryStream stream = new();
            using (Utf8JsonWriter writer = new(stream, options))
            {
                writer.WriteStartObject();
                writer.WriteString("status", result.Status.ToString());
                writer.WriteStartObject("results");
                foreach (System.Collections.Generic.KeyValuePair<string, HealthReportEntry> entry in result.Entries)
                {
                    writer.WriteStartObject(entry.Key);
                    writer.WriteString("status", entry.Value.Status.ToString());
                    writer.WriteString("description", entry.Value.Description);
                    writer.WriteStartObject("data");
                    foreach (System.Collections.Generic.KeyValuePair<string, object> item in entry.Value.Data)
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

            string json = Encoding.UTF8.GetString(stream.ToArray());

            return context.Response.WriteAsync(json);
        }
    }
}
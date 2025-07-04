using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FAA.Api.AppStart;
using SFA.DAS.FAA.Api.HealthCheck;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Newtonsoft.Json.Converters;
using SFA.DAS.Common.Domain.Json;
using SFA.DAS.Common.Domain.Models;
using SFA.DAS.FAA.Api.Infrastructure;
using SFA.DAS.FAA.Data;

namespace SFA.DAS.FAA.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {

#if DEBUG
                config
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile("appsettings.Development.json", true);
#endif

                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }

            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<FindApprenticeshipsApiConfiguration>(_configuration.GetSection("FindApprenticeshipsApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipsApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(_configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            var findApprenticeshipsApiConfiguration = _configuration
                .GetSection("FindApprenticeshipsApi")
                .Get<FindApprenticeshipsApiConfiguration>();
            services.AddDatabaseRegistration(findApprenticeshipsApiConfiguration!, _configuration["Environment"]);

            if (!ConfigurationIsLocalOrDev())
            {
                var azureAdConfiguration = _configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();

                var policies = new Dictionary<string, string>
                {
                    {PolicyNames.Default, "Default"}
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            if (_configuration["Environment"] != "DEV")
            {
                services
                    .AddHealthChecks()
                    .AddDbContextCheck<FindApprenticeshipsDataContext>()
                    .AddCheck<AzureSearchHealthCheck>("Azure search re-indexing health",
                        failureStatus: HealthStatus.Degraded, tags: new[] {"ready"});
                
            }

            services.AddMediatR(_ => _.RegisterServicesFromAssembly(typeof(SearchApprenticeshipVacanciesQuery).Assembly));
            services.AddServiceRegistration();

            services
                .AddMvc(o =>
                {
                    if (!ConfigurationIsLocalOrDev())
                    {
                        o.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                    }
                    o.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.Converters.Add(new VacancyReferenceJsonConverter());
                });

            services.AddApplicationInsightsTelemetry();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindApprenticeshipsApi", Version = "v2" });
                c.OperationFilter<SwaggerVersionHeaderFilter>();
                c.DocumentFilter<JsonPatchDocumentFilter>();
                c.MapType<VacancyReference>(() => new OpenApiSchema { Type = "string" });
            });

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindApprenticeshipsApi v2");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(logger);

            app.UseAuthentication();

            if (!_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseHealthChecks();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Vacancies}/{action=Get}/{id?}");
            });
        }

        private bool ConfigurationIsLocalOrDev()
        {
            return _configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   _configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
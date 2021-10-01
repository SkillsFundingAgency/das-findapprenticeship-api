using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FAA.Api.AppStart;
using SFA.DAS.FAA.Domain.Configuration;

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
            
            //services.AddElasticSearch(_configuration);
            services.AddSingleton(new FindApprenticeshipsApiEnvironment(_configuration["Environment"]));
            
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
                services.AddHealthChecks();
            }
            
            services.AddMediatR(typeof(Application.Class1).Assembly);
            services.AddServiceRegistration();
            
            services
                .AddMvc(o =>
                {
                    o.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindApprenticeshipsApi", Version = "v1" });
                c.OperationFilter<SwaggerVersionHeaderFilter>();
            });
            
            services.AddApiVersioning(opt => {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            services.AddLogging();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindApprenticeshipsApi v1");
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
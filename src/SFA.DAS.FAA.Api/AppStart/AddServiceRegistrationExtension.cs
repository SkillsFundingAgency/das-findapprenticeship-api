using System;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddTransient<IElasticSearchQueries, ElasticSearchQueries>();
            services.AddTransient<IElasticSearchQueryBuilder, ElasticSearchQueryBuilder>();
            services.AddTransient<IVacancySearchRepository, ApprenticeshipVacancySearchRepository>();
            services.AddTransient<IAcsVacancySearchRepository, AcsVacancySearchRepository>();
            services.AddTransient<IAzureSearchHelper, AzureSearchHelper>();
        }
        
        public static void AddElasticSearch(this IServiceCollection collection, FindApprenticeshipsApiConfiguration configuration)
        {
            var connectionPool = new SingleNodeConnectionPool(new Uri(configuration.ElasticSearchServerUrl));

            var settings = new ConnectionConfiguration(connectionPool);

            if (!string.IsNullOrEmpty(configuration.ElasticSearchUsername) &&
                !string.IsNullOrEmpty(configuration.ElasticSearchPassword))
            {
                settings.BasicAuthentication(configuration.ElasticSearchUsername, configuration.ElasticSearchPassword);
            }
                        
            collection.AddTransient<IElasticLowLevelClient>(sp => new ElasticLowLevelClient(settings));
            collection.AddSingleton<IElasticSearchQueries, ElasticSearchQueries>();
        }
    }
}

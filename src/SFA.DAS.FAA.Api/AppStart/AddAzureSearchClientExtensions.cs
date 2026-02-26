using Azure.Core;
using Azure.Core.Serialization;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Interfaces;
using System;
using System.Text.Json;

namespace SFA.DAS.FAA.Api.AppStart;

public static class AddAzureSearchClientExtensions
{
    public static void AddAzureSearchClient(this IServiceCollection services, FindApprenticeshipsApiConfiguration config)
    {
        services.AddSingleton<TokenCredential>(_ =>
        {
            const int maxRetries = 2;
            var networkTimeout = TimeSpan.FromSeconds(1);
            var delay = TimeSpan.FromMilliseconds(100);

            return new ChainedTokenCredential(
                new ManagedIdentityCredential(options: new TokenCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay }
                }),
                new AzureCliCredential(new AzureCliCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay }
                }),
                new VisualStudioCredential(new VisualStudioCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay }
                }),
                new VisualStudioCodeCredential(new VisualStudioCodeCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay }
                })
            );
        });

        services.AddSingleton(sp =>
        {
            var credential = sp.GetRequiredService<TokenCredential>();
            var options = new SearchClientOptions
            {
                Serializer = new JsonObjectSerializer(new JsonSerializerOptions
                {
                    Converters = { new MicrosoftSpatialGeoJsonConverter() }
                })
            };
            return new SearchClient(
                new Uri(config.AzureSearchBaseUrl),
                AzureSearchIndex.IndexName,
                credential,
                options);
        });
        services.AddSingleton(sp =>
        {
            var credential = sp.GetRequiredService<TokenCredential>();
            return new SearchIndexClient(new Uri(config.AzureSearchBaseUrl), credential);
        });
        services.AddScoped<IAzureSearchHelper, AzureSearchHelper>();
    }
}
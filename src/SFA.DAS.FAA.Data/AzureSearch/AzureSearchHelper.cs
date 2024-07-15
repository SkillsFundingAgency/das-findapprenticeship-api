using Azure.Core.Serialization;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FAA.Domain.Constants;

namespace SFA.DAS.FAA.Data.AzureSearch;
public class AzureSearchHelper : IAzureSearchHelper
{
    private readonly SearchClient _searchClient;
    private readonly SearchIndexClient _searchIndexerClient;
    private const int MaxRetries = 2;
    private readonly TimeSpan _networkTimeout = TimeSpan.FromSeconds(1);
    private readonly TimeSpan _delay = TimeSpan.FromMilliseconds(100);

    public AzureSearchHelper(FindApprenticeshipsApiConfiguration configuration)
    {
        var clientOptions = new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(new JsonSerializerOptions
            {
                Converters =
                {
                    new MicrosoftSpatialGeoJsonConverter()
                }
            })
        };

        _searchClient = new SearchClient(
            new Uri(configuration.AzureSearchBaseUrl), 
            AzureSearchIndex.IndexName, 
            new ChainedTokenCredential(
                new ManagedIdentityCredential(options: new TokenCredentialOptions
                {
                    Retry = { NetworkTimeout = _networkTimeout, MaxRetries = MaxRetries, Delay = _delay }
                }),
                new AzureCliCredential(options: new AzureCliCredentialOptions
                {
                    Retry = { NetworkTimeout = _networkTimeout, MaxRetries = MaxRetries, Delay = _delay }
                }),
                new VisualStudioCredential(options: new VisualStudioCredentialOptions
                {
                    Retry = { NetworkTimeout = _networkTimeout, MaxRetries = MaxRetries, Delay = _delay }
                }),
                new VisualStudioCodeCredential(options: new VisualStudioCodeCredentialOptions()
                {
                    Retry = { NetworkTimeout = _networkTimeout, MaxRetries = MaxRetries, Delay = _delay }
                })), 
            clientOptions);

        _searchIndexerClient = new SearchIndexClient(new Uri(configuration.AzureSearchBaseUrl), new DefaultAzureCredential());
    }
    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        var searchOptions = new SearchOptions()
            .BuildSort(findVacanciesModel)
            .BuildPaging(findVacanciesModel)
            .BuildFilters(findVacanciesModel)
            .BuildSearch(findVacanciesModel);
        searchOptions.IncludeTotalCount = true;
        searchOptions.SearchMode = SearchMode.All;
        searchOptions.QueryType = SearchQueryType.Simple;

        var searchTerm = BuildSearchTerm(findVacanciesModel.SearchTerm);

        var searchResultsTask = _searchClient.SearchAsync<SearchDocument>($"{searchTerm}", searchOptions);

        var totalCountSearchOptions = new SearchOptions().BuildFiltersForTotalCount(findVacanciesModel.AdditionalDataSources);
        var totalVacanciesCountTask = _searchClient.SearchAsync<SearchDocument>("*", totalCountSearchOptions);

        await Task.WhenAll(searchResultsTask, totalVacanciesCountTask);

        var searchResults = searchResultsTask.Result;
        var totalVacanciesCount = totalVacanciesCountTask.Result;
        var result = searchResults.Value.GetResults().ToList().Select(searchResult => JsonSerializer.Deserialize<ApprenticeshipSearchItem>(searchResult.Document.ToString())).ToList();

        if (findVacanciesModel.Lat.HasValue && findVacanciesModel.Lon.HasValue)
        {
            result.ForEach(c => c.SearchGeoPoint = new GeoPoint { Lat = findVacanciesModel.Lat.Value, Lon = findVacanciesModel.Lon.Value });
        }

        return new ApprenticeshipSearchResponse
        {
            ApprenticeshipVacancies = result.Select(c => c)
                .ToList(),
            TotalFound = Convert.ToInt32(searchResults.Value.TotalCount),
            Total = Convert.ToInt32(totalVacanciesCount.Value.TotalCount)
        };
    }

    public async Task<ApprenticeshipVacancyItem> Get(string vacancyReference)
    {
        vacancyReference = vacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
        var searchResults = await _searchClient.GetDocumentAsync<SearchDocument>(vacancyReference);
        return JsonSerializer.Deserialize<ApprenticeshipVacancyItem>(searchResults.Value.ToString());
    }

    public async Task<List<ApprenticeshipSearchItem>> Get(List<string> vacancyReferences)
    {
        var filters = new StringBuilder();
        var count = 0;

        foreach (var reference in vacancyReferences)
        {
            if (count > 0)
                filters.Append(" or ");

            count++;
            filters.Append($"VacancyReference eq '{reference}'");
        }

        var searchOptions = new SearchOptions { Filter = filters.ToString() };
        var searchResults = await _searchClient.SearchAsync<SearchDocument>("*", searchOptions);
        var results = searchResults.Value.GetResults()
            .Select(searchResult => JsonSerializer.Deserialize<ApprenticeshipSearchItem>(searchResult.Document.ToString()))
            .ToList();

        return results;
    }

    public async Task<string> GetIndexName(CancellationToken cancellationToken)
    {
        var result = await _searchIndexerClient.GetIndexAsync(AzureSearchIndex.IndexName, cancellationToken);
        return result.Value.Name;
    }

    public async Task<int> Count(List<AdditionalDataSource> additionalDataSources)
    {
        var totalCountSearchOptions = new SearchOptions().BuildFiltersForTotalCount(additionalDataSources);
        var count = await _searchClient.SearchAsync<SearchDocument>("*", totalCountSearchOptions);
        return Convert.ToInt32(count.Value.TotalCount);
    }

    private string BuildSearchTerm(string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return "*";
        }
        if (searchTerm.Contains(' '))
        {
            var searchTermArray = searchTerm.Split(' ');
            var newSearch = new StringBuilder();
            foreach (var s in searchTermArray)
            {
                newSearch.Append('+');
                newSearch.Append(s);
                newSearch.Append('*');
            }
            return newSearch.ToString();
        }
        return $"{searchTerm}*";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.AzureSearch;
public class AzureSearchHelper : IAzureSearchHelper
{
    private const string IndexName = "apprenticeships";
    private readonly SearchClient _searchClient;

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
            IndexName, 
            new DefaultAzureCredential(), 
            clientOptions);
    }
    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        var searchOptions = new SearchOptions()
            .BuildSort(findVacanciesModel)
            .BuildPaging(findVacanciesModel)
            .BuildFilters(findVacanciesModel)
            .BuildSearch(findVacanciesModel);
        searchOptions.IncludeTotalCount = true;

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

    public async Task<int> Count(List<AdditionalDataSource> additionalDataSources)
    {
        var totalCountSearchOptions = new SearchOptions().BuildFiltersForTotalCount(additionalDataSources);
        var count = await _searchClient.SearchAsync<SearchDocument>("*", totalCountSearchOptions);
        return Convert.ToInt32(count.Value.TotalCount);
    }

    public string BuildSearchTerm(string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) { return "*"; }

        var alphaRegex = new Regex("[a-zA-Z0-9 ]", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
        var illegalChars = searchTerm.Where(x => !alphaRegex.IsMatch(x.ToString())).ToList();

        while (illegalChars.Contains(searchTerm[0]))
        {
            searchTerm = searchTerm.Substring(1);
        }
        while (illegalChars.Contains(searchTerm[searchTerm.Length - 1]))
        {
            searchTerm = searchTerm.Substring(0, searchTerm.Length - 1);
        }
        return string.IsNullOrEmpty(searchTerm) ? "*" : $"{searchTerm}*";
    }
}
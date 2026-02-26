using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Data.AzureSearch;
public class AzureSearchHelper(SearchClient searchClient,
    SearchIndexClient searchIndexClient) 
    : IAzureSearchHelper
{
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

        var searchResultsTask = searchClient.SearchAsync<SearchDocument>($"{searchTerm}", searchOptions);

        var totalCountSearchOptions = new SearchOptions().BuildFiltersForTotalCount(findVacanciesModel.AdditionalDataSources);
        var totalVacanciesCountTask = searchClient.SearchAsync<SearchDocument>("*", totalCountSearchOptions);

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
        try
        {
            vacancyReference = vacancyReference.StartsWith("VAC") ? vacancyReference.Replace("VAC","") : vacancyReference;
            var searchResults = await searchClient.GetDocumentAsync<SearchDocument>(vacancyReference);
            return JsonSerializer.Deserialize<ApprenticeshipVacancyItem>(searchResults.Value.ToString());
        }
        catch (RequestFailedException)
        {
            return null;
        }
    }

    public async Task<List<ApprenticeshipSearchItem>> Get(List<string> vacancyReferences)
    {
        if (vacancyReferences is not { Count: > 0 })
        {
            return [];
        }

        var searchOptions = new SearchOptions
        {
            Filter = $"{vacancyReferences.SearchIn("VacancyReference")} and IsPrimaryLocation eq true",
            QueryType = SearchQueryType.Full,
            Size = 500
        };
        var searchResults = await searchClient.SearchAsync<SearchDocument>("*", searchOptions);
        var results = searchResults.Value.GetResults().ToList()
            .Select(searchResult => JsonSerializer.Deserialize<ApprenticeshipSearchItem>(searchResult.Document.ToString()))
            .ToList();
        
        return results;
    }

    public async Task<string> GetIndexName(CancellationToken cancellationToken)
    {
        var result = await searchIndexClient.GetIndexAsync(AzureSearchIndex.IndexName, cancellationToken);
        return result.Value.Name;
    }

    public async Task<int> Count(FindVacanciesCountModel findVacanciesModel)
    {
        var searchOptions = new SearchOptions()
            .BuildFiltersForTotalSearchCount(findVacanciesModel);
        searchOptions.IncludeTotalCount = true;
        searchOptions.SearchMode = SearchMode.All;
        searchOptions.QueryType = SearchQueryType.Simple;

        var searchTerm = BuildSearchTerm(findVacanciesModel.SearchTerm);
        var searchResults = await searchClient.SearchAsync<SearchDocument>($"{searchTerm}", searchOptions);
        
        return Convert.ToInt32(searchResults.Value.TotalCount);
    }

    private static string BuildSearchTerm(string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return "*";
        }

        if (!searchTerm.Contains(' ')) return $"{searchTerm}*";

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
}
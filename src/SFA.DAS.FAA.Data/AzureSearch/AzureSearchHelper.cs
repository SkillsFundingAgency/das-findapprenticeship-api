using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
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
            new AzureCliCredential(new AzureCliCredentialOptions
            {
                TenantId = configuration.AzureSearchResource
            }), 
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
        var totalVacanciesCountTask = _searchClient.GetDocumentCountAsync();

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
            Total = Convert.ToInt32(totalVacanciesCount)
        };
    }

    public async Task<ApprenticeshipVacancyItem> Get(string vacancyReference)
    {
        vacancyReference = vacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
        var searchResults = await _searchClient.GetDocumentAsync<SearchDocument>(vacancyReference);
        return JsonSerializer.Deserialize<ApprenticeshipVacancyItem>(searchResults.Value.ToString());
    }

    public async Task<int> Count()
    {
        var count = await _searchClient.GetDocumentCountAsync();
        return Convert.ToInt32(count);
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
        return string.IsNullOrEmpty(searchTerm) ? "*" : $"\"{searchTerm}*\"";
    }
}
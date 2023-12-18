using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Core.Serialization;
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
            new AzureKeyCredential(configuration.AzureSearchKey), 
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

        var searchResultsTask = _searchClient.SearchAsync<SearchDocument>($"{findVacanciesModel.SearchTerm}*", searchOptions);
        var totalVacanciesCountTask = _searchClient.GetDocumentCountAsync();

        await Task.WhenAll(searchResultsTask, totalVacanciesCountTask);

        var searchResults = searchResultsTask.Result;
        var totalVacanciesCount = totalVacanciesCountTask.Result;
        var result = searchResults.Value.GetResults().ToList().Select(searchResult => JsonSerializer.Deserialize<ApprenticeshipSearchItem>(searchResult.Document.ToString())).ToList();
        return new ApprenticeshipSearchResponse
        {
            ApprenticeshipVacancies = result.Select(c=>c)
                .ToList(),
            TotalFound = Convert.ToInt32(searchResults.Value.TotalCount),
            Total = Convert.ToInt32(totalVacanciesCount)
        };
    }

    public async Task<ApprenticeshipVacancyItem> Get(string vacancyReference)
    {
        var searchResults = await _searchClient.GetDocumentAsync<SearchDocument>(vacancyReference);
        return JsonSerializer.Deserialize<ApprenticeshipVacancyItem>(searchResults.Value.ToString());
    }
}
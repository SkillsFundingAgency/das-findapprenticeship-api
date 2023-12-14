using System;
using Azure.Search.Documents;
using Azure;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Models;
using System.Linq;
using SFA.DAS.FAA.Domain.Entities;
using System.Collections.Generic;
using Azure.Search.Documents.Models;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using System.Text.Json;

namespace SFA.DAS.FAA.Data.Repository;
public class AzureSearchHelper : IAzureSearchHelper
{
    private readonly AzureKeyCredential _azureKeyCredential;
    private readonly SearchClientOptions _clientOptions;
    private readonly Uri _endpoint;
    private readonly SearchClient _searchClient;

    public AzureSearchHelper(FindApprenticeshipsApiConfiguration configuration)
    {
        _clientOptions = new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(new JsonSerializerOptions
            {
                Converters =
                {
                    new MicrosoftSpatialGeoJsonConverter()
                }
            })
        };

        _azureKeyCredential = new AzureKeyCredential(configuration.AzureSearchKey);
        _endpoint = new Uri(configuration.AzureSearchBaseUrl);
        
        _searchClient = new SearchClient(_endpoint, "apprenticeships", _azureKeyCredential, _clientOptions);
    }
    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        var searchOptions = new SearchOptions();
        searchOptions = BuildSort(findVacanciesModel, searchOptions);
        searchOptions = BuildPaging(findVacanciesModel, searchOptions);
        searchOptions = BuildFilters(findVacanciesModel, searchOptions);
        searchOptions.IncludeTotalCount = true;

        var searchResultsTask = _searchClient.SearchAsync<SearchDocument>(searchOptions.Filter);
        var totalVacanciesCountTask = _searchClient.GetDocumentCountAsync();

        await Task.WhenAll(searchResultsTask, totalVacanciesCountTask);

        var searchResults = searchResultsTask.Result;
        var totalVacanciesCount = totalVacanciesCountTask.Result;
        
        return MapResponse(searchResults.Value, totalVacanciesCount.Value);
    }

    private static SearchOptions BuildSort(FindVacanciesModel searchVacanciesModel, SearchOptions searchOptions)
    {
        switch (searchVacanciesModel.VacancySort)
        {
            case VacancySort.AgeAsc:
                searchOptions.OrderBy.Add("PostedDate asc");
                break;
            case VacancySort.AgeDesc:
                searchOptions.OrderBy.Add("PostedDate desc");
                break;
            case VacancySort.ExpectedStartDateAsc:
                searchOptions.OrderBy.Add("StartDate asc");
                break;
            case VacancySort.ExpectedStartDateDesc:
                searchOptions.OrderBy.Add("StartDate desc");
                break;
            case VacancySort.DistanceAsc:
                if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                {
                    searchOptions.OrderBy.Add($"geo.distance(Location, geography'POINT({searchVacanciesModel.Lat} {searchVacanciesModel.Lon})') asc");
                }
                break;
            case VacancySort.DistanceDesc:
                if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                {
                    searchOptions.OrderBy.Add($"geo.distance(Location, geography'POINT({searchVacanciesModel.Lat} {searchVacanciesModel.Lon})') desc");
                }
                break;
        }
        return searchOptions;
    }

    private static SearchOptions BuildPaging(FindVacanciesModel findVacanciesModel, SearchOptions searchOptions)
    {
        findVacanciesModel.PageNumber = findVacanciesModel.PageNumber < 2 ? 1 : findVacanciesModel.PageNumber;
        searchOptions.Skip = (findVacanciesModel.PageNumber - 1) * findVacanciesModel.PageSize;
        searchOptions.Size = findVacanciesModel.PageSize;
        return searchOptions;
    }

    private static SearchOptions BuildFilters(FindVacanciesModel findVacanciesModel, SearchOptions searchOptions)
    {
        List<string> searchFilters = new();

        if (findVacanciesModel.Ukprn.HasValue)
        {
            searchFilters.Add($"Ukprn eq {findVacanciesModel.Ukprn}");
        }

        if (!string.IsNullOrEmpty(findVacanciesModel.AccountPublicHashedId))
        {
            searchFilters.Add($"AccountPublicHashedId eq {findVacanciesModel.AccountPublicHashedId}");
        }

        if (!string.IsNullOrEmpty(findVacanciesModel.AccountLegalEntityPublicHashedId))
        {
            searchFilters.Add($"AccountLegalEntityPublicHashedId eq {findVacanciesModel.AccountLegalEntityPublicHashedId}");
        }

        if (findVacanciesModel.StandardLarsCode != null && findVacanciesModel.StandardLarsCode.Any())
        {
            findVacanciesModel.StandardLarsCode.ForEach(larsCode => searchFilters.Add(($"Course/any(c: c/LarsCode eq {larsCode})")));
        }

        if (findVacanciesModel.Categories != null && findVacanciesModel.Categories.Any())
        {
            findVacanciesModel.Categories.ForEach(category => searchFilters.Add($"Route eq {category}"));
        }

        if (findVacanciesModel.Lat.HasValue && findVacanciesModel.Lon.HasValue && findVacanciesModel.DistanceInMiles.HasValue)
        {
            var distanceInMiles = Convert.ToDecimal(findVacanciesModel.DistanceInMiles);
            var distanceInKm = (distanceInMiles - (distanceInMiles / 5)) * 2;
            searchFilters.Add($"geo.distance(Location, geography'POINT({findVacanciesModel.Lat} {findVacanciesModel.Lon})') le {distanceInKm}");
        }

        if (findVacanciesModel.NationWideOnly.HasValue)
        {
            if (findVacanciesModel.NationWideOnly == true)
            {
                searchFilters.Add("VacancyLocationType eq National");
            }
            else
            {
                searchFilters.Add("VacancyLocationType eq NonNational");
            }
        }

        if (findVacanciesModel.PostedInLastNumberOfDays.HasValue)
        {
            var numberOfDays = Convert.ToDouble(findVacanciesModel.PostedInLastNumberOfDays);
            searchFilters.Add($"PostedDate ge {DateTime.UtcNow.AddDays(-numberOfDays)}");
        }

        searchOptions.Filter = string.Join(" and ", searchFilters.ToArray());
        return searchOptions;
    }

    private ApprenticeshipSearchResponse MapResponse(SearchResults<SearchDocument> searchResponse, long totalVacanciesCount)
    {
        var result = searchResponse.GetResults().ToList().Select(searchResult => JsonSerializer.Deserialize<ApprenticeshipSearchItem>(searchResult.Document.ToString())).ToList();
        return new ApprenticeshipSearchResponse
        {
            ApprenticeshipVacancies = result.Select(c=>c)
                .ToList(),
            TotalFound = Convert.ToInt32(searchResponse.TotalCount),
            Total = Convert.ToInt32(totalVacanciesCount)
        };
    }
}

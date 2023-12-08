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

namespace SFA.DAS.FAA.Data.Repository;
public class AzureSearchHelper : IAzureSearchHelper
{
    private readonly AzureKeyCredential _azureKeyCredential;
    private readonly Uri _endpoint;
    private readonly SearchClient _searchClient;

    public AzureSearchHelper(FindApprenticeshipsApiConfiguration configuration)
    {
        _azureKeyCredential = new AzureKeyCredential(configuration.AzureSearchKey);
        _endpoint = new Uri(configuration.AzureSearchBaseUrl);
        _searchClient = new SearchClient(_endpoint, "apprenticeships", _azureKeyCredential);
    }
    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        var searchOptions = new SearchOptions();
        searchOptions = BuildSort(findVacanciesModel, searchOptions);
        searchOptions = BuildPaging(findVacanciesModel, searchOptions);
        searchOptions = BuildFilters(findVacanciesModel, searchOptions);
        searchOptions.IncludeTotalCount = true;

        var searchResults = await _searchClient.SearchAsync<ApprenticeshipAzureSearchDocument>(searchOptions);
        var totalVacanciesCount = await _searchClient.GetDocumentCountAsync();

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
        findVacanciesModel.PageNumber = findVacanciesModel.PageNumber < 2 ? 0 : findVacanciesModel.PageNumber;
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

    private static ApprenticeshipSearchResponse MapResponse(SearchResults<ApprenticeshipAzureSearchDocument> searchResponse, long totalVacanciesCount)
    {
        return new ApprenticeshipSearchResponse()
        {
            ApprenticeshipVacancies = MapVacancies(searchResponse.GetResults()),
            TotalFound = Convert.ToInt32(searchResponse.TotalCount),
            Total = Convert.ToInt32(totalVacanciesCount)
        };
    }

    private static IEnumerable<ApprenticeshipSearchItem> MapVacancies(Pageable<SearchResult<ApprenticeshipAzureSearchDocument>> searchResults)
    {
        var searchItems = new List<ApprenticeshipSearchItem>();
        foreach (var result in searchResults)
        {
            var document = result.Document;
            searchItems.Add(new ApprenticeshipSearchItem()
            {
                //Id = document.Id,
                //AnonymousEmployerName = document
                ApprenticeshipLevel = (ApprenticeshipLevel)document.Course.Level,
                //Category
                //CategoryCode
                ClosingDate = document.ClosingDate.DateTime,
                Description = document.Description,
                EmployerName = document.EmployerName,
                FrameworkLarsCode = string.Empty, // - this shouldn't be needed at all
                HoursPerWeek = document.HoursPerWeek,
                //IsDisabilityConfident = 
                //IsEmployerAnonymous = document.
                //IsPositiveAboutDisability = document
                //IsRecruitVacancy =
                Location = new GeoPoint() { Lat = document.Location.Latitude, Lon = document.Location.Longitude },
                NumberOfPositions = Convert.ToInt32(document.NumberOfPositions),
                PostedDate = document.PostedDate.DateTime,
                ProviderName = document.ProviderName,
                StandardLarsCode = Convert.ToInt32(document.Course.LarsCode),
                StartDate = document.StartDate.DateTime,
                //SubCategory = 
                //SubCategoryCode = 
                Title = document.Title,
                Ukprn = (long)document.Ukprn,
                //VacancyLocationType = document
                VacancyReference = document.VacancyReference,
                WageAmount = document.Wage.WageAmount,
                //WageAmountLowerBound = document.Wage.
                //WageAmountUpperBound = document.Wage
                WageText = document.Wage.WageAdditionalInformation,
                WageUnit = Convert.ToInt32(document.Wage.WageUnit),
                WageType = Convert.ToInt32(document.Wage.WageType),
                WorkingWeek = document.Wage.WorkingWeekDescription,
                Address = new Address()
                {
                    AddressLine1 = document.Address.AddressLine1,
                    AddressLine2 = document.Address.AddressLine2,
                    AddressLine3 = document.Address.AddressLine3,
                    AddressLine4 = document.Address.AddressLine4,
                    Postcode = document.Address.Postcode
                },
                //EmployerWebsiteUrl = document.
                //EmployerDescription = document.
                //EmployerContactName = 
                //EmployerContactPhone = 
                //EmployerContactEmail = 
                //Duration = document
                //DurationUnit = document.
                //ExpectedDuration = document
                //Distance = document.di
                //Score = document
            });
        }
        return searchItems.AsEnumerable();
    }
}

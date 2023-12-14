using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Search.Documents;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.AzureSearch;

public static class AzureSearchOptionExtensions
{
    public static SearchOptions BuildSort(this SearchOptions searchOptions, FindVacanciesModel searchVacanciesModel)
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

    public static SearchOptions BuildPaging(this SearchOptions searchOptions, FindVacanciesModel findVacanciesModel)
    {
        findVacanciesModel.PageNumber = findVacanciesModel.PageNumber < 2 ? 1 : findVacanciesModel.PageNumber;
        searchOptions.Skip = (findVacanciesModel.PageNumber - 1) * findVacanciesModel.PageSize;
        searchOptions.Size = findVacanciesModel.PageSize;
        return searchOptions;
    }

    public static SearchOptions BuildFilters(this SearchOptions searchOptions, FindVacanciesModel findVacanciesModel)
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
}
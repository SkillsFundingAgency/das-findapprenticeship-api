using System;
using System.Collections.Generic;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.AzureSearch;

public static class AzureSearchOptionExtensions
{
    public static SearchOptions BuildSort(this SearchOptions searchOptions, FindVacanciesModel searchVacanciesModel)
    {
        switch (searchVacanciesModel.VacancySort)
        {
            case VacancySort.AgeAsc:
                searchOptions.OrderBy.Add("PostedDate desc");
                break;
            case VacancySort.AgeDesc:
                searchOptions.OrderBy.Add("PostedDate asc");
                break;
            case VacancySort.ExpectedStartDateAsc:
                searchOptions.OrderBy.Add("StartDate asc");
                break;
            case VacancySort.ExpectedStartDateDesc:
                searchOptions.OrderBy.Add("StartDate desc");
                break;
            case VacancySort.ClosingAsc:
                searchOptions.OrderBy.Add("ClosingDate asc");
                break;
            case VacancySort.ClosingDesc:
                searchOptions.OrderBy.Add("ClosingDate desc");
                break;
            case VacancySort.SalaryAsc:
                if (searchVacanciesModel.SkipWageType is null)
                {
                    if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                    {
                        searchOptions.OrderBy.Add($"Wage/WageType asc, Wage/Between18AndUnder21NationalMinimumWage asc, geo.distance(Location, geography'POINT({searchVacanciesModel.Lon} {searchVacanciesModel.Lat})') asc, PostedDate asc, ClosingDate asc");
                    }
                    else
                    {
                        searchOptions.OrderBy.Add("Wage/WageType asc, Wage/Between18AndUnder21NationalMinimumWage asc, PostedDate asc, ClosingDate asc");
                    }
                }
                else
                {
                    searchOptions.OrderBy.Add("Wage/Between18AndUnder21NationalMinimumWage asc");
                }
                break;
            case VacancySort.SalaryDesc:
                if (searchVacanciesModel.SkipWageType is null)
                {
                    if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                    {
                        searchOptions.OrderBy.Add($"Wage/WageType asc, Wage/Between18AndUnder21NationalMinimumWage desc, geo.distance(Location, geography'POINT({searchVacanciesModel.Lon} {searchVacanciesModel.Lat})') asc, PostedDate asc, ClosingDate asc");
                    }
                    else
                    {
                        searchOptions.OrderBy.Add("Wage/WageType asc, Wage/Between18AndUnder21NationalMinimumWage desc, PostedDate asc, ClosingDate asc");
                    }
                }
                else
                {
                    searchOptions.OrderBy.Add("Wage/Between18AndUnder21NationalMinimumWage desc");
                }
                break;
            case VacancySort.DistanceAsc:
                if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                {
                    searchOptions.OrderBy.Add($"geo.distance(Location, geography'POINT({searchVacanciesModel.Lon} {searchVacanciesModel.Lat})') asc");
                }
                break;
            case VacancySort.DistanceDesc:
                if (searchVacanciesModel.Lat.HasValue || searchVacanciesModel.Lon.HasValue)
                {
                    searchOptions.OrderBy.Add($"geo.distance(Location, geography'POINT({searchVacanciesModel.Lon} {searchVacanciesModel.Lat})') desc");
                }
                break;
        }

        searchOptions.BuildSortTiesBreakers();
        return searchOptions;
    }

    public static void BuildSortTiesBreakers(this SearchOptions searchOptions)
    {
        searchOptions.OrderBy.Add("Title asc");
        searchOptions.OrderBy.Add("Course/Title asc");
        searchOptions.OrderBy.Add("TypicalJobTitles asc");
        searchOptions.OrderBy.Add("EmployerName asc");
        searchOptions.OrderBy.Add("ProviderName asc");
        searchOptions.OrderBy.Add("Ukprn asc");
        searchOptions.OrderBy.Add("VacancyReference asc");
    }

    public static SearchOptions BuildPaging(this SearchOptions searchOptions, FindVacanciesModel findVacanciesModel)
    {
        findVacanciesModel.PageNumber = findVacanciesModel.PageNumber < 2 ? 1 : findVacanciesModel.PageNumber;
        searchOptions.Skip = (findVacanciesModel.PageNumber - 1) * findVacanciesModel.PageSize;
        searchOptions.Size = findVacanciesModel.PageSize;
        return searchOptions;
    }

    public static SearchOptions BuildFiltersForTotalCount(this SearchOptions searchOptions,
        List<AdditionalDataSource> additionalDataSources)
    {
        List<string> searchFilters = [];

        if (additionalDataSources != null && additionalDataSources.Count != 0)
        {
            var sourceClauses = new List<string> { AzureSearchConstants.VacancySourceEqualsRaa };
            additionalDataSources.ForEach(source => sourceClauses.Add($"VacancySource eq '{source.GetAzureSearchTerm()}'"));
            searchFilters.Add($"({string.Join(" or ", [.. sourceClauses])})");
        }
        else
        {
            searchFilters.Add(AzureSearchConstants.VacancySourceEqualsRaa);
        }

        searchOptions.Filter = string.Join(" and ", searchFilters.ToArray());
        searchOptions.IncludeTotalCount = true;
        return searchOptions;
    }

    public static SearchOptions BuildFilters(this SearchOptions searchOptions, FindVacanciesModel findVacanciesModel)
    {
        List<string> searchFilters = [];

        if (findVacanciesModel.AdditionalDataSources != null && findVacanciesModel.AdditionalDataSources.Count != 0)
        {
            var sourceClauses = new List<string> { AzureSearchConstants.VacancySourceEqualsRaa };
            findVacanciesModel.AdditionalDataSources.ForEach(source => sourceClauses.Add($"VacancySource eq '{source.GetAzureSearchTerm()}'"));
            searchFilters.Add($"({string.Join(" or ", [.. sourceClauses])})");
        }
        else
        {
            searchFilters.Add(AzureSearchConstants.VacancySourceEqualsRaa);
        }

        if (findVacanciesModel.Ukprn.HasValue)
        {
            searchFilters.Add($"Ukprn eq '{findVacanciesModel.Ukprn}'");
        }

        if (!string.IsNullOrEmpty(findVacanciesModel.AccountPublicHashedId))
        {
            searchFilters.Add($"AccountPublicHashedId eq '{findVacanciesModel.AccountPublicHashedId}'");
        }

        if (!string.IsNullOrEmpty(findVacanciesModel.AccountLegalEntityPublicHashedId))
        {
            searchFilters.Add($"AccountLegalEntityPublicHashedId eq '{findVacanciesModel.AccountLegalEntityPublicHashedId}'");
        }

        if (findVacanciesModel.StandardLarsCode != null && findVacanciesModel.StandardLarsCode.Count != 0)
        {
            var larsCodeClauses = new List<string>();
            findVacanciesModel.StandardLarsCode.ForEach(larsCode => larsCodeClauses.Add($"Course/LarsCode eq {larsCode}"));
            searchFilters.Add($"({string.Join(" or ", [.. larsCodeClauses])})");
        }

        if (findVacanciesModel.Categories != null && findVacanciesModel.Categories.Count != 0)
        {
            var categoryClauses = new List<string>();
            findVacanciesModel.Categories.ForEach(category => categoryClauses.Add($"Route eq '{category}'"));
            searchFilters.Add($"({string.Join(" or ", [.. categoryClauses])})");
        }

        if (findVacanciesModel.Levels != null && findVacanciesModel.Levels.Count != 0)
        {
            var levelClauses = new List<string>();
            findVacanciesModel.Levels.ForEach(level => levelClauses.Add($"Course/Level eq '{level}'"));
            searchFilters.Add($"({string.Join(" or ", [.. levelClauses])})");
        }

        if (findVacanciesModel.Lat.HasValue && findVacanciesModel.Lon.HasValue && findVacanciesModel.DistanceInMiles.HasValue)
        {
            var distanceInMiles = Convert.ToDecimal(findVacanciesModel.DistanceInMiles);
            var distanceInKm = (distanceInMiles - (distanceInMiles / 5)) * 2;
            searchFilters.Add($"geo.distance(Location, geography'POINT({findVacanciesModel.Lon} {findVacanciesModel.Lat})') le {distanceInKm}");
        }

        if (findVacanciesModel.NationWideOnly.HasValue)
        {
            searchFilters.Add(findVacanciesModel.NationWideOnly == true
                ? "VacancyLocationType eq 'National'"
                : "VacancyLocationType eq 'NonNational'");
        }

        if (findVacanciesModel.PostedInLastNumberOfDays.HasValue)
        {
            var numberOfDays = Convert.ToDouble(findVacanciesModel.PostedInLastNumberOfDays);
            searchFilters.Add($"PostedDate ge {DateTime.UtcNow.AddDays(-numberOfDays):O}");
        }

        if (findVacanciesModel.DisabilityConfident != false)
        {
            searchFilters.Add("IsDisabilityConfident eq true");
        }

        if (findVacanciesModel.VacancySort is VacancySort.SalaryAsc or VacancySort.SalaryDesc)
        {
            if (findVacanciesModel.SkipWageType is not null)
            {
                searchFilters.Add($"Wage/WageType ne '{findVacanciesModel.SkipWageType}'");
            }
        }

        searchOptions.Filter = string.Join(" and ", searchFilters.ToArray());

        return searchOptions;
    }

    public static SearchOptions BuildFiltersForTotalSearchCount(this SearchOptions searchOptions, FindVacanciesCountModel findVacanciesModel)
    {
        List<string> searchFilters = [];

        if (findVacanciesModel.AdditionalDataSources != null && findVacanciesModel.AdditionalDataSources.Count != 0)
        {
            var sourceClauses = new List<string> { AzureSearchConstants.VacancySourceEqualsRaa };
            findVacanciesModel.AdditionalDataSources.ForEach(source => sourceClauses.Add($"VacancySource eq '{source.GetAzureSearchTerm()}'"));
            searchFilters.Add($"({string.Join(" or ", [.. sourceClauses])})");
        }
        else
        {
            searchFilters.Add(AzureSearchConstants.VacancySourceEqualsRaa);
        }

        if (findVacanciesModel.Ukprn.HasValue)
        {
            searchFilters.Add($"Ukprn eq '{findVacanciesModel.Ukprn}'");
        }

        if (findVacanciesModel.Categories != null && findVacanciesModel.Categories.Count != 0)
        {
            var categoryClauses = new List<string>();
            findVacanciesModel.Categories.ForEach(category => categoryClauses.Add($"Route eq '{category}'"));
            searchFilters.Add($"({string.Join(" or ", [.. categoryClauses])})");
        }

        if (findVacanciesModel.Levels != null && findVacanciesModel.Levels.Count != 0)
        {
            var levelClauses = new List<string>();
            findVacanciesModel.Levels.ForEach(level => levelClauses.Add($"Course/Level eq '{level}'"));
            searchFilters.Add($"({string.Join(" or ", [.. levelClauses])})");
        }

        if (findVacanciesModel.Lat.HasValue && findVacanciesModel.Lon.HasValue && findVacanciesModel.DistanceInMiles.HasValue)
        {
            var distanceInMiles = Convert.ToDecimal(findVacanciesModel.DistanceInMiles);
            var distanceInKm = (distanceInMiles - (distanceInMiles / 5)) * 2;
            searchFilters.Add($"geo.distance(Location, geography'POINT({findVacanciesModel.Lon} {findVacanciesModel.Lat})') le {distanceInKm}");
        }

        if (findVacanciesModel.NationWideOnly.HasValue)
        {
            searchFilters.Add(findVacanciesModel.NationWideOnly == true
                ? "VacancyLocationType eq 'National'"
                : "VacancyLocationType eq 'NonNational'");
        }

        if (findVacanciesModel.DisabilityConfident != false)
        {
            searchFilters.Add("IsDisabilityConfident eq true");
        }

        if (findVacanciesModel.WageType is not null)
        {
            searchFilters.Add($"Wage/WageType eq '{findVacanciesModel.WageType}'");
        }

        searchOptions.Filter = string.Join(" and ", searchFilters.ToArray());

        return searchOptions;
    }

    public static SearchOptions BuildSearch(this SearchOptions searchOptions, FindVacanciesModel findVacanciesModel)
    {
        findVacanciesModel.Ukprn.ToString();

        searchOptions.QueryType = SearchQueryType.Full;
        searchOptions.SearchFields.Add("Title");
        searchOptions.SearchFields.Add("Course/Title");
        searchOptions.SearchFields.Add("EmployerName");
        searchOptions.SearchFields.Add("ProviderName");
        searchOptions.SearchFields.Add("Ukprn");

        return searchOptions;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record SavedSearchDto(
        Guid Id,
        Guid UserReference,
        DateTime DateCreated,
        DateTime? LastRunDate,
        DateTime? EmailLastSendDate,
        SearchParametersDto SearchParameters
    )
    {
        public static SavedSearchDto From(SavedSearch source)
        {
            return new SavedSearchDto(
                source.Id,
                source.UserReference,
                source.DateCreated,
                source.LastRunDate,
                source.EmailLastSendDate,
                SearchParametersDto.From(source.SearchParameters)
            );
        }
    };

    public record SearchParametersDto(
        string? SearchTerm,
        List<int>? SelectedRouteIds,
        int? Distance,
        bool DisabilityConfident,
        List<int>? SelectedLevelIds,
        string? Location,
        string? Latitude,
        string? Longitude
    )
    {
        public static SearchParametersDto From(SearchParameters source)
        {
            return new SearchParametersDto(
                source.SearchTerm,
                source.SelectedRouteIds,
                source.Distance,
                source.DisabilityConfident,
                source.SelectedLevelIds,
                source.Location,
                source.Latitude,
                source.Longitude
            );
        }
    }

public record GetSavedSearchesResponse(
    List<SavedSearchDto> SavedSearches,
    int TotalCount,
    int PageIndex,
    int PageSize,
    int TotalPages
) 
{
    public static GetSavedSearchesResponse From(GetSavedSearchesQueryResult source)
    {
        return new GetSavedSearchesResponse(
            source.SavedSearches.Select(SavedSearchDto.From).ToList(),
            source.TotalCount,
            source.PageIndex,
            source.PageSize,
            source.TotalPages
        );
    }
}
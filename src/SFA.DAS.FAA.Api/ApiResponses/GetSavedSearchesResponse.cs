using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record SavedSearchResponse(
        Guid Id,
        Guid UserReference,
        DateTime DateCreated,
        DateTime? LastRunDate,
        DateTime? EmailLastSendDate,
        SearchParametersResponse SearchParameters
    )
    {
        public static SavedSearchResponse From(SavedSearch source)
        {
            return new SavedSearchResponse(
                source.Id,
                source.UserReference,
                source.DateCreated,
                source.LastRunDate,
                source.EmailLastSendDate,
                SearchParametersResponse.From(source.SearchParameters)
            );
        }
    };

    public record SearchParametersResponse(
        string? SearchTerm,
        List<string>? Categories,
        int? Distance,
        bool DisabilityConfident,
        List<string>? Levels,
        string? Latitude,
        string? Longitude
    )
    {
        public static SearchParametersResponse From(SearchParameters source)
        {
            return new SearchParametersResponse(
                source.SearchTerm,
                source.Categories,
                source.Distance,
                source.DisabilityConfident,
                source.Levels,
                source.Latitude,
                source.Longitude
            );
        }
    }

public record GetSavedSearchesResponse(
    List<SavedSearchResponse> SavedSearches,
    int TotalCount,
    int PageIndex,
    int PageSize,
    int TotalPages
)
{
    public static GetSavedSearchesResponse From(GetSavedSearchesQueryResult source)
    {
        return new GetSavedSearchesResponse(
            source.SavedSearches?.Select(SavedSearchResponse.From)?.ToList() ?? new List<SavedSearchResponse>(),
            source.TotalCount,
            source.PageIndex,
            source.PageSize,
            source.TotalPages
        );
    }
}
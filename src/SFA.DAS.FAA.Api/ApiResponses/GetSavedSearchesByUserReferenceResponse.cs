using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

namespace SFA.DAS.FAA.Api.ApiResponses;

public class GetSavedSearchesByUserReferenceResponse
{
    public List<SavedSearchDto> SavedSearches { get; set; }

    public static GetSavedSearchesByUserReferenceResponse From(GetSavedSearchesByUserReferenceQueryResult source)
    {
        return new GetSavedSearchesByUserReferenceResponse
        {
            SavedSearches = source.SavedSearches.Select(SavedSearchDto.From).ToList()
        };
    }
}
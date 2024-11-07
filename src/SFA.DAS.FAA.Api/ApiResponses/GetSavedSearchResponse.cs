using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record GetSavedSearchResponse(SavedSearchDto SavedSearch);
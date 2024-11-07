using System;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;

public record GetSavedSearchQueryResult(SavedSearch? SavedSearch);
using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

public class GetSavedSearchesByUserReferenceQueryResult
{
    public List<SavedSearch> SavedSearches { get; set; }
}
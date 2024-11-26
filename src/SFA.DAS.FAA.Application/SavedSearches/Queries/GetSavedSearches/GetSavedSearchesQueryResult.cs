using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches
{
    public record GetSavedSearchesQueryResult
    {
        public List<SavedSearch> SavedSearches { get; private init; } = [];
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        
        public static implicit operator GetSavedSearchesQueryResult(PaginatedList<SavedSearchEntity> source)
        {
            return new GetSavedSearchesQueryResult
            {
                TotalCount = source.TotalCount,
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                SavedSearches = source.Items.Select(SavedSearch.From).ToList()
            };
        }
    }
}

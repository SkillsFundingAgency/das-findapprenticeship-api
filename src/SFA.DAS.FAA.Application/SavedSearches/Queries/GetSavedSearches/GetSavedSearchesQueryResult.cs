using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
                SavedSearches = source.Items.Select(s => (SavedSearch)s).ToList()
            };
        }

        public record SavedSearch
        {
            public Guid Id { get; set; }
            public Guid UserReference { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? LastRunDate { get; set; }
            public DateTime? EmailLastSendDate { get; set; }
            public SearchParameters SearchCriteriaParameters { get; set; }

            public static implicit operator SavedSearch(SavedSearchEntity source)
            {
                return new SavedSearch
                {
                    Id = source.Id,
                    UserReference = source.UserRef,
                    DateCreated = source.DateCreated,
                    LastRunDate = source.LastRunDate,
                    EmailLastSendDate = source.EmailLastSendDate,
                    SearchCriteriaParameters = JsonConvert.DeserializeObject<SearchParameters>(source.SearchParameters)
                };
            }
        }

        public record SearchParameters
        {
            public List<string>? Categories { get; set; }
            public List<string>? Levels { get; set; }
            public string? Latitude { get; set; }
            public string? Longitude { get; set; }
            public int? Distance { get; set; }
            public string? SearchTerm { get; set; }
            public bool DisabilityConfident { get; set; }
        }
    }
}

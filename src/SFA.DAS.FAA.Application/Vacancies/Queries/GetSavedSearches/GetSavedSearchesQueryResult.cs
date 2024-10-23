using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches
{
    public record GetSavedSearchesQueryResult
    {
        public List<SavedSearch> SavedSearches { get; set; } = [];

        public static implicit operator GetSavedSearchesQueryResult(List<SavedSearchEntity> source)
        {
            return new GetSavedSearchesQueryResult
            {
                SavedSearches = source.Select(s => (SavedSearch)s).ToList()
            };
        }

        public record SavedSearch
        {
            public Guid Id { get; set; }
            public Guid UserReference { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? LastRunDate { get; set; }
            public List<string> VacancyReferences { get; set; }
            public SearchParameters SearchCriteriaParameters { get; set; }

            public static implicit operator SavedSearch(SavedSearchEntity source)
            {
                return new SavedSearch
                {
                    Id = source.Id,
                    UserReference = source.UserRef,
                    DateCreated = source.DateCreated,
                    LastRunDate = source.LastRunDate,
                    VacancyReferences = JsonConvert.DeserializeObject<List<string>>(source.VacancyRefs),
                    SearchCriteriaParameters = JsonConvert.DeserializeObject<SearchParameters>(source.SearchParameters)
                };
            }
        }

        public record SearchParameters
        {
            public List<string>? Categories { get; set; }
            public List<string>? Levels { get; set; }
            public string? Location { get; set; }
            public int? Distance { get; set; }
            public string? SearchTerm { get; set; }
            public bool DisabilityConfident { get; set; }
        }
    }
}

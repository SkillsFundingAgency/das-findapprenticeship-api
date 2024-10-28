using System;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class SavedSearch
    {
        public Guid Id { get; set; }
        public Guid UserReference { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastRunDate { get; set; }
        public string SearchParameters { get; set; }
        public string? VacancyReferences { get; set; }

        public static implicit operator SavedSearch(SavedSearchEntity source)
        {
            return new SavedSearch
            {
                Id = source.Id,
                UserReference = source.UserRef,
                DateCreated = source.DateCreated,
                LastRunDate = source.LastRunDate,
                SearchParameters = source.SearchParameters,
                VacancyReferences = source.VacancyRefs
            };
        }
    }
}
using System;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class SavedSearchEntity
    {
        public Guid Id { get; set; }
        public Guid UserRef { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastRunDate { get; set; }
        public string SearchParameters { get; set; }
        public string? VacancyRefs { get; set; }

        public static implicit operator SavedSearchEntity(SavedSearch source)
        {
            return new SavedSearchEntity
            {
                Id = source.Id,
                UserRef = source.UserRef,
                DateCreated = source.DateCreated,
                LastRunDate = source.LastRunDate,
                SearchParameters = source.SearchParameters,
                VacancyRefs = source.VacancyRefs
            };
        }
    }
}
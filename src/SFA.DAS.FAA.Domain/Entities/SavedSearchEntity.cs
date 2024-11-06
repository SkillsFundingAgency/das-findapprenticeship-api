using System;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class SavedSearchEntity
    {
        public Guid Id { get; set; }
        public Guid UserRef { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastRunDate { get; set; }
        public DateTime? EmailLastSendDate { get; set; }
        public string SearchParameters { get; set; } = null!;
        public string UnSubscribeToken { get; set; } = null!;

        public static implicit operator SavedSearchEntity(SavedSearch source)
        {
            return new SavedSearchEntity
            {
                Id = source.Id,
                UserRef = source.UserReference,
                DateCreated = source.DateCreated,
                LastRunDate = source.LastRunDate,
                EmailLastSendDate = source.EmailLastSendDate,
                
                SearchParameters = source.SearchParameters.ToJson(),
            };
        }
    }
}
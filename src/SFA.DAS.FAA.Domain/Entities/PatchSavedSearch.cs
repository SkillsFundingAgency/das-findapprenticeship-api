using System;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class PatchSavedSearch
    {
        public DateTime? LastRunDate { get; set; }
        public DateTime? EmailLastSendDate { get; set; }

        public static implicit operator PatchSavedSearch(SavedSearchEntity savedSearch)
        {
            return new PatchSavedSearch
            {
                LastRunDate = savedSearch.LastRunDate,
                EmailLastSendDate = savedSearch.EmailLastSendDate,
            };
        }
    }
}
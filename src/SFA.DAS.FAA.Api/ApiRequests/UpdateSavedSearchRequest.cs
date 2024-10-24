using System;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Api.ApiRequests
{
    public record UpdateSavedSearchRequest
    {
        public List<Guid> SavedSearchesGuids { get; set; }
    }
}

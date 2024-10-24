using System;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Api.ApiRequests;

public record PostUpdateSavedSearchesRequest
{
    public List<Guid> SavedSearchGuids { get; set; }
}
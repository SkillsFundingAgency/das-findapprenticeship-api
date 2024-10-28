using System;

namespace SFA.DAS.FAA.Api.ApiRequests;

public record SaveSearchRequest(
    Guid UserReference,
    string SearchParameters
);
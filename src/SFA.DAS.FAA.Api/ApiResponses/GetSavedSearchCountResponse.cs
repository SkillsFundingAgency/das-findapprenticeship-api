using System;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record GetSavedSearchCountResponse(Guid UserReference, int SavedSearchesCount);
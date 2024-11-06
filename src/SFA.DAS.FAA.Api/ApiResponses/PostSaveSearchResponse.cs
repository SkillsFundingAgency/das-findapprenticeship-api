using System;
using SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record PostSaveSearchResponse(Guid Id)
{
    public static PostSaveSearchResponse From(UpsertSaveSearchCommandResult source)
    {
        return new PostSaveSearchResponse(source.Id);
    }
};
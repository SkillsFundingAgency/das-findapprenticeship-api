using System;
using SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record PutSaveSearchResponse(Guid Id)
{
    public static PutSaveSearchResponse From(UpsertSaveSearchCommandResult source)
    {
        return new PutSaveSearchResponse(source.Id);
    }
};
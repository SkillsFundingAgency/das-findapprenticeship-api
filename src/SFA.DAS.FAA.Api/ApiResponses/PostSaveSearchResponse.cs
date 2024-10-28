using System;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;

namespace SFA.DAS.FAA.Api.ApiResponses;

public record PostSaveSearchResponse(Guid Id)
{
    public static PostSaveSearchResponse From(SaveSearchCommandResult source)
    {
        return new PostSaveSearchResponse(source.Id);
    }
};
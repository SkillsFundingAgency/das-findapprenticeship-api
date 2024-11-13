using System;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

public record UpsertSaveSearchCommandResult(Guid Id)
{
    public static UpsertSaveSearchCommandResult None => new UpsertSaveSearchCommandResult(Guid.Empty);
};
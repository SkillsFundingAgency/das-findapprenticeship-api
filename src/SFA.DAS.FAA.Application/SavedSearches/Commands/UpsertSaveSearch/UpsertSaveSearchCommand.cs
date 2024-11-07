using System;
using MediatR;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

public record UpsertSaveSearchCommand(
    Guid Id,
    Guid UserReference,
    string UnSubscribeToken,
    SearchParameters SearchParameters
) : IRequest<UpsertSaveSearchCommandResult>;
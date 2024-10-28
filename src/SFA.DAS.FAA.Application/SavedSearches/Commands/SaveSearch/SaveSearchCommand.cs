using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;

public record SaveSearchCommand(
    Guid UserReference,
    string SearchParameters
) : IRequest<SaveSearchCommandResult>;
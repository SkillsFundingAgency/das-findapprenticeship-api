using System;
using MediatR;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;

public record SaveSearchCommand(
    Guid UserReference,
    SearchParameters SearchParameters
) : IRequest<SaveSearchCommandResult>;
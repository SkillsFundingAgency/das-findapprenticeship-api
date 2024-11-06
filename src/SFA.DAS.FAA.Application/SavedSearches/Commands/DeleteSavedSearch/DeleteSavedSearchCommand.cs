using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;

public record DeleteSavedSearchCommand(Guid Id, Guid UserReference) : IRequest<Unit>;
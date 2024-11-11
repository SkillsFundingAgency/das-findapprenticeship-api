using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteUserSavedSearch;

public record DeleteUserSavedSearchCommand(Guid Id, Guid UserReference) : IRequest<Unit>;
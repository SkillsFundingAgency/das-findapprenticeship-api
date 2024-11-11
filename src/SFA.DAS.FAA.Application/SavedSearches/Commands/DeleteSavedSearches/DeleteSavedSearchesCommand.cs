using MediatR;
using System;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearches;
public record DeleteSavedSearchesCommand(Guid UserReference) : IRequest<Unit>;
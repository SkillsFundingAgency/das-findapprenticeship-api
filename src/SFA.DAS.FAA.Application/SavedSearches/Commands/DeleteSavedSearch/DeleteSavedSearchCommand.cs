using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;

public class DeleteSavedSearchCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
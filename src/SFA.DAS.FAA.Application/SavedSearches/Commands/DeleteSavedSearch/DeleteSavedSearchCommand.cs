using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;

public class DeleteSavedSearchCommand : IRequest
{
    public Guid Id { get; set; }
}
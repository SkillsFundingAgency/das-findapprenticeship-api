using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;

public class GetSavedSearchQuery : IRequest<GetSavedSearchQueryResult>
{
    public Guid Id { get; set; }
}
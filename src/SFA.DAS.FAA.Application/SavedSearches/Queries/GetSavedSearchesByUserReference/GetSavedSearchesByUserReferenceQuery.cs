using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

public class GetSavedSearchesByUserReferenceQuery: IRequest<GetSavedSearchesByUserReferenceQueryResult>
{
    public Guid UserReference { get; init; }
}
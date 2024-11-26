using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;

public class GetSavedSearchCountQuery : IRequest<int>
{
    public Guid UserReference { get; init; }
}
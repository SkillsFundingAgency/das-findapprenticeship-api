using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.GetSavedSearches
{
    public record GetSavedSearchesQuery(DateTime LastRunDateFilter, int PageNumber, int PageSize) : IRequest<GetSavedSearchesQueryResult>;
}

using MediatR;
using System;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches
{
    public record GetSavedSearchesQuery(DateTime LastRunDateFilter, int PageNumber, int PageSize) : IRequest<GetSavedSearchesQueryResult>;
}

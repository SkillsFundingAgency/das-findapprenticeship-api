using MediatR;
using System;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches
{
    public record GetSavedSearchesQuery(DateTime LastRunDateFilter) : IRequest<GetSavedSearchesQueryResult>;
}

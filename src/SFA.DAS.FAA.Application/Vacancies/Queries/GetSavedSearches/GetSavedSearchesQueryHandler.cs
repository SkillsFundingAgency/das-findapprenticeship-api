using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches;

public record GetSavedSearchesQueryHandler(ISavedSearchRepository SavedSearchRepository) : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
{
    public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        return await SavedSearchRepository.GetAll(request.LastRunDateFilter, request.PageNumber, request.PageSize, cancellationToken);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;

public class GetSavedSearchQueryHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<GetSavedSearchQuery, GetSavedSearchQueryResult>
{
    public async Task<GetSavedSearchQueryResult> Handle(GetSavedSearchQuery request, CancellationToken cancellationToken)
    {
        var result = await savedSearchRepository.GetById(request.Id, cancellationToken);

        return new GetSavedSearchQueryResult
        {
            SavedSearch = result == null ? null : SavedSearch.From(result)
        };
    }
}
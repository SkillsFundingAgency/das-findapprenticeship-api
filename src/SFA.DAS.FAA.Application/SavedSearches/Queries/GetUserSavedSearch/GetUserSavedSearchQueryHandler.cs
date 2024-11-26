using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetUserSavedSearch;

public class GetUserSavedSearchQueryHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<GetUserSavedSearchQuery, GetUserSavedSearchQueryResult>
{
    public async Task<GetUserSavedSearchQueryResult> Handle(GetUserSavedSearchQuery request, CancellationToken cancellationToken)
    {
        var result = await savedSearchRepository.Get(request.UserReference, request.Id, cancellationToken);
        var savedSearch = result is null
            ? null 
            : SavedSearch.From(result);

        return new GetUserSavedSearchQueryResult(savedSearch);
    }
}
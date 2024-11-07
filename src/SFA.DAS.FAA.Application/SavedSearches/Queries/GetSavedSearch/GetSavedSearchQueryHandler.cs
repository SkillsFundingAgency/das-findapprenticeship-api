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
        var result = await savedSearchRepository.Get(request.UserReference, request.Id, cancellationToken);
        var savedSearch = result is null
            ? null 
            : SavedSearch.From(result);
        
        return new GetSavedSearchQueryResult(savedSearch);
    }
}
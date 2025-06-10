using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

public class GetSavedSearchesByUserReferenceQueryHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<GetSavedSearchesByUserReferenceQuery, GetSavedSearchesByUserReferenceQueryResult>
{
    public async Task<GetSavedSearchesByUserReferenceQueryResult> Handle(GetSavedSearchesByUserReferenceQuery request, CancellationToken cancellationToken)
    {
        var results = await savedSearchRepository.GetByUserReference(request.UserReference, cancellationToken);
        return new GetSavedSearchesByUserReferenceQueryResult
        {
            SavedSearches = results
                .Where(x => !string.IsNullOrEmpty(x.SearchParameters))
                .Select(SavedSearch.From).ToList()
        };
    }
}
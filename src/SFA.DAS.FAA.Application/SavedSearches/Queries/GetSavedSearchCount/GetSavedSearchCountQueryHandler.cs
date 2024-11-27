using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;

public class GetSavedSearchCountQueryHandler(ISavedSearchRepository savedSearchesRepository) : IRequestHandler<GetSavedSearchCountQuery, int>
{
    public async Task<int> Handle(GetSavedSearchCountQuery request, CancellationToken cancellationToken)
    {
        return await savedSearchesRepository.Count(request.UserReference);
    }
}
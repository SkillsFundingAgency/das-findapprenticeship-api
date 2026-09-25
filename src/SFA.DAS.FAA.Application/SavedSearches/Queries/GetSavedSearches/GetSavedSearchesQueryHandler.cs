using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;

public class GetSavedSearchesQueryHandler(
    ISavedSearchRepository savedSearchRepository) 
    : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
{
    public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        // we want everything from the last run date & before
        var nearCutOffDate = request.LastRunDateFilter.AddDays(1).Date;
        return await savedSearchRepository.GetAll(nearCutOffDate,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
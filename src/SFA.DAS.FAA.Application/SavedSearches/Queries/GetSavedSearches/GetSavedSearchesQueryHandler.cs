﻿using System.Threading;
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
        return await savedSearchRepository.GetAll(request.LastRunDateFilter,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PostUpdateSavedSearches;

public record PostUpdateSavedSearchesCommandHandler(ISavedSearchRepository SavedSearchRepository) : IRequestHandler<PostUpdateSavedSearchesCommand, Unit>
{
    public async Task<Unit> Handle(PostUpdateSavedSearchesCommand request, CancellationToken cancellationToken)
    {
        var savedSearches = new List<SavedSearchEntity>
        {
            Capacity = 0
        };

        foreach (var savedSearchGuid in request.SavedSearchGuids)
        {
            var savedSearch = await SavedSearchRepository.Get(savedSearchGuid, cancellationToken);
            savedSearch.LastRunDate = DateTime.UtcNow;
            savedSearches.Add(savedSearch);
        }

        await SavedSearchRepository.BulkSave(savedSearches, cancellationToken).ConfigureAwait(false);
        
        return Unit.Value;
    }
}
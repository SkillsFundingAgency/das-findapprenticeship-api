using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PostUpdateSavedSearches;

public record PostUpdateSavedSearchesCommandHandler(ISavedSearchRepository SavedSearchRepository) : IRequestHandler<PostUpdateSavedSearchesCommand, Unit>
{
    public async Task<Unit> Handle(PostUpdateSavedSearchesCommand request, CancellationToken cancellationToken)
    {
        foreach (var savedSearchGuid in request.SavedSearchGuids)
        {
            var savedSearch = await SavedSearchRepository.Get(savedSearchGuid, cancellationToken);
            savedSearch.LastRunDate = DateTime.UtcNow;
            await SavedSearchRepository.Save(savedSearch, cancellationToken);
        }

        return Unit.Value;
    }
}
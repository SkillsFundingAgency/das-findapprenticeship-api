using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;

public class SaveSearchCommandHandler(ISavedSearchRepository savedSearchesRepository): IRequestHandler<SaveSearchCommand, SaveSearchCommandResult>
{
    public async Task<SaveSearchCommandResult> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var result = await savedSearchesRepository.Upsert(new SavedSearchEntity
        {
            UserRef = request.UserReference,
            DateCreated = DateTime.UtcNow,
            SearchParameters = request.SearchParameters.ToJson(),
        },
        cancellationToken);
        
        return new SaveSearchCommandResult(result.Id);
    }
}
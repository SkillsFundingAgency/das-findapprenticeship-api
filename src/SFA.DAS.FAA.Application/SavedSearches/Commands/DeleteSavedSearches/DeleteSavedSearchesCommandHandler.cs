using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearches;

public class DeleteSavedSearchesCommandHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<DeleteSavedSearchesCommand>
{
    public async Task Handle(DeleteSavedSearchesCommand request, CancellationToken cancellationToken)
    {
        await savedSearchRepository.DeleteAll(request.UserReference, cancellationToken);
    }
}
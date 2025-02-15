using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;

public class DeleteSavedSearchCommandHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<DeleteSavedSearchCommand>
{
    public async Task Handle(DeleteSavedSearchCommand request, CancellationToken cancellationToken)
    {
        await savedSearchRepository.Delete(request.Id, cancellationToken);
    }
}
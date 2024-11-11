using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteUserSavedSearch;

public class DeleteUserSavedSearchCommandHandler(ISavedSearchRepository savedSearchRepository) : IRequestHandler<DeleteUserSavedSearchCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserSavedSearchCommand request, CancellationToken cancellationToken)
    {
        await savedSearchRepository.Delete(request.UserReference, request.Id, cancellationToken);
        return Unit.Value;
    }
}
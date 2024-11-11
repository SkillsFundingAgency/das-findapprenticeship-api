using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearches;

public record DeleteSavedSearchesCommandHandler(ISavedSearchRepository SavedSearchRepository) : IRequestHandler<DeleteSavedSearchesCommand, Unit>
{
    public async Task<Unit> Handle(DeleteSavedSearchesCommand request, CancellationToken cancellationToken)
    {
        await SavedSearchRepository.DeleteAll(request.UserReference, cancellationToken);
        return Unit.Value;
    }
}
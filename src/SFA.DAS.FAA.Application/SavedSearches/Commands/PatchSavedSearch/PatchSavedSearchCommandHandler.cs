using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch;

public class PatchSavedSearchCommandHandler(ISavedSearchRepository SavedSearchRepository) : IRequestHandler<PatchSavedSearchCommand, PatchSavedSearchCommandResponse>
{
    public async Task<PatchSavedSearchCommandResponse> Handle(PatchSavedSearchCommand request, CancellationToken cancellationToken)
    {
        var savedSearch = await SavedSearchRepository.GetById(request.Id, cancellationToken);

        if (savedSearch == null)
        {
            return new PatchSavedSearchCommandResponse();
        }

        var patchedDoc = (Domain.Entities.PatchSavedSearch)savedSearch;

        request.Patch.ApplyTo(patchedDoc);

        savedSearch.EmailLastSendDate = patchedDoc.EmailLastSendDate;
        savedSearch.LastRunDate = patchedDoc.LastRunDate;
        await SavedSearchRepository.Update(savedSearch, cancellationToken);

        return new PatchSavedSearchCommandResponse
        {
            SavedSearch = SavedSearch.From(savedSearch)
        };
    }
}
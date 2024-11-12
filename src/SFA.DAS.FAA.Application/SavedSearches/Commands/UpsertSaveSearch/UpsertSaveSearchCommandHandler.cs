﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

public class UpsertSaveSearchCommandHandler(ISavedSearchRepository savedSearchesRepository): IRequestHandler<UpsertSaveSearchCommand, UpsertSaveSearchCommandResult>
{
    public async Task<UpsertSaveSearchCommandResult> Handle(UpsertSaveSearchCommand request, CancellationToken cancellationToken)
    {
        var count = await savedSearchesRepository.Count(request.UserReference);
        if (count >= Constants.SavedSearchLimit)
        {
            return UpsertSaveSearchCommandResult.None;
        }
        
        var result = await savedSearchesRepository.Upsert(new SavedSearchEntity
        {
            Id = request.Id,
            UserRef = request.UserReference,
            SearchParameters = request.SearchParameters.ToJson(),
        },
        cancellationToken);
        
        return new UpsertSaveSearchCommandResult(result.Id);
    }
}
using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch;

public record PatchSavedSearchCommand(Guid Id, JsonPatchDocument<Domain.Entities.PatchSavedSearch> Patch) : IRequest<PatchSavedSearchCommandResponse>;
using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PostUpdateSavedSearches;

public record PostUpdateSavedSearchesCommand(List<Guid> SavedSearchGuids) : IRequest<Unit>;
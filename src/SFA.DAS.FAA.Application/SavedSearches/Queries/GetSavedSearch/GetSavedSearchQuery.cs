using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;

public record GetSavedSearchQuery(Guid UserReference, Guid Id): IRequest<GetSavedSearchQueryResult>;
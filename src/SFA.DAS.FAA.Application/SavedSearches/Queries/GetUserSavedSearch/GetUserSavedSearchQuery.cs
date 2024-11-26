using System;
using MediatR;

namespace SFA.DAS.FAA.Application.SavedSearches.Queries.GetUserSavedSearch;

public record GetUserSavedSearchQuery(Guid UserReference, Guid Id): IRequest<GetUserSavedSearchQueryResult>;
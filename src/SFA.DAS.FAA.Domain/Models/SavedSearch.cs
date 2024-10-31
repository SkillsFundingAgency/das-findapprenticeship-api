using System;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Domain.Models;

public record SavedSearch(
    Guid Id,
    Guid UserReference,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParameters SearchParameters
)
{
    public static SavedSearch From(SavedSearchEntity source)
    {
        return new SavedSearch(
            source.Id,
            source.UserRef,
            source.DateCreated,
            source.LastRunDate,
            source.EmailLastSendDate,
            SearchParameters.From(source.SearchParameters)
        );
    }
};
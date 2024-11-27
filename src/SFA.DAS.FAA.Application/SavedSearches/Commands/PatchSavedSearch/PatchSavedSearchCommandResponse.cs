using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch
{
    public record PatchSavedSearchCommandResponse
    {
        public SavedSearch SavedSearch { get; set; }
    }
}

using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries;

public class WhenHandlingGetSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_SavedSearch_Is_Returned_By_Id(
        SearchParameters searchParameters,
        GetSavedSearchQuery query,
        SavedSearchEntity savedSearch,
        [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
        [Greedy] GetSavedSearchQueryHandler handler)
    {
        savedSearch.SearchParameters = searchParameters.ToJson();
        savedSearchRepository.Setup(x => x.GetById(query.Id, It.IsAny<CancellationToken>())).ReturnsAsync(savedSearch);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.SavedSearch.Should().BeEquivalentTo(SavedSearch.From(savedSearch));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_SavedSearch_Does_Not_Exist_Null_Returned(
        GetSavedSearchQuery query,
        [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
        [Greedy] GetSavedSearchQueryHandler handler)
    {
        savedSearchRepository.Setup(x => x.GetById(query.Id, It.IsAny<CancellationToken>())).ReturnsAsync((SavedSearchEntity)null);
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.SavedSearch.Should().BeNull();
    }
}
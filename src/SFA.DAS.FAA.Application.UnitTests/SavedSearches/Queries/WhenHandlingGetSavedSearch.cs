using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries;

public class WhenHandlingGetSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_A_Result_Is_Returned(
        SavedSearchEntity savedSearch,
        GetSavedSearchQuery query,
        SearchParameters searchParameters,
        [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
        GetSavedSearchQueryHandler sut
    )
    {
        // arrange
        savedSearch.SearchParameters = searchParameters.ToJson();
        savedSearchRepository
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), default))
            .ReturnsAsync(savedSearch);
        
        // act
        var results = await sut.Handle(query, default);

        // assert
        results.SavedSearch.Should().BeEquivalentTo(savedSearch, options => 
            options.Excluding(x => x.SearchParameters).WithMapping("UserRef", "UserReference"));
        results.SavedSearch!.SearchParameters.Should().BeEquivalentTo(searchParameters);
        savedSearchRepository.Verify(x => 
            x.Get(It.Is<Guid>(id => id == query.UserReference), It.Is<Guid>(id => id == query.Id), default),
            Times.Once);
    }
}
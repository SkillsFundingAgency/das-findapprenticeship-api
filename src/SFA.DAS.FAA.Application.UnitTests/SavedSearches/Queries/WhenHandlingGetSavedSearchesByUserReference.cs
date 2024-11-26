using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries;

public class WhenHandlingGetSavedSearchesByUserReference
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        List<SavedSearchEntity> savedSearches,
        GetSavedSearchesByUserReferenceQuery query,
        SearchParameters searchParameters,
        [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
        GetSavedSearchesByUserReferenceQueryHandler sut
        )
    {
        // arrange
        foreach (var savedSearch in savedSearches)
        {
            savedSearch.SearchParameters = searchParameters.ToJson();
        }

        savedSearchRepository
            .Setup(x => x.GetByUserReference(It.IsAny<Guid>(), default))
            .ReturnsAsync(savedSearches);
        
        // act
        var results = await sut.Handle(query, default);

        // assert
        results.SavedSearches.Count.Should().Be(savedSearches.Count);
        savedSearchRepository.Verify(x => x.GetByUserReference(It.Is<Guid>(id => id == query.UserReference), default), Times.Once);
    }
}
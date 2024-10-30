using System;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries;

public class WhenHandlingGetSavedSearchCountQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Count_Is_Returned(
        GetSavedSearchCountQuery query,
        int count,
        [Frozen] Mock<ISavedSearchesRepository> savedSearchesRepository,
        GetSavedSearchCountQueryHandler sut)
    {
        // arrange
        savedSearchesRepository
            .Setup(repo => repo.Count(It.Is<Guid>(guid => guid == query.UserReference)))
            .ReturnsAsync(count);

        // act
        var actual = await sut.Handle(query, default);

        // assert
        actual.Should().Be(count);
    }
}
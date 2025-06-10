using SFA.DAS.FAA.Application.SavedSearches.Queries.GetUserSavedSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    [TestFixture]
    internal class WhenHandlingGetUserSavedSearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_It_Should_Return_Saved_Searches_For_User(
            GetUserSavedSearchQuery query,
            SearchParameters searchParameters,
            SavedSearchEntity savedSearch,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepositoryMock,
            [Greedy] GetUserSavedSearchQueryHandler handler)
        {
            // Arrange
            savedSearch.SearchParameters = JsonConvert.SerializeObject(searchParameters);
            savedSearchRepositoryMock.Setup(x => x.Get(query.UserReference, query.Id, CancellationToken.None))
                .ReturnsAsync(savedSearch);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.SavedSearch.Should().NotBeNull();
            result.SavedSearch.Should().BeEquivalentTo(savedSearch, options => options
                .Excluding(x => x.SearchParameters)
                .ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task When_Null_Then_It_Should_Return_Null(
            GetUserSavedSearchQuery query,
            SavedSearchEntity savedSearch,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepositoryMock,
            [Greedy] GetUserSavedSearchQueryHandler handler)
        {
            // Arrange
            savedSearchRepositoryMock.Setup(x => x.Get(query.UserReference, query.Id, CancellationToken.None))
                .ReturnsAsync((SavedSearchEntity)null!);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SavedSearch.Should().BeNull();
        }
    }
}

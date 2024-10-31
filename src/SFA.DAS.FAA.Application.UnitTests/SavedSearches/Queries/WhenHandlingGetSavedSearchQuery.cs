using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_SavedSearches_From_Repository(
            SearchParameters searchParameters,
            GetSavedSearchesQuery query,
            PaginatedList<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            [Greedy] GetSavedSearchesQueryHandler handler)
        {
            //arrange
            foreach (var savedSearchEntity in savedSearchEntities.Items)
            {
                savedSearchEntity.SearchParameters = searchParameters.ToJson();
            }

            savedSearchRepository
                .Setup(repository => repository.GetAll(query.LastRunDateFilter, query.PageNumber, query.PageSize, CancellationToken.None))
                .ReturnsAsync(savedSearchEntities);

            var result = await handler.Handle(query, CancellationToken.None);

            result.SavedSearches
                .Should().BeEquivalentTo(savedSearchEntities.Items, options => options
                    .Excluding(ex => ex.SearchParameters)
                    .Excluding(ex => ex.UserRef)
                );
        }
    }
}

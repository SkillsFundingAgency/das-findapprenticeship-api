using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Queries;

[TestFixture]
public class WhenHandlingGetSavedSearchesQuery
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

        DateTime? capturedDateTime = null;
        savedSearchRepository
            .Setup(repository => repository.GetAll(It.IsAny<DateTime>(), query.PageNumber, query.PageSize, CancellationToken.None))
            .Callback<DateTime, int, int, CancellationToken>((nearCutOffDate, _, _, _) => capturedDateTime = nearCutOffDate)
            .ReturnsAsync(savedSearchEntities);

        var result = await handler.Handle(query, CancellationToken.None);

        result.SavedSearches
            .Should().BeEquivalentTo(savedSearchEntities.Items, options => options
                .Excluding(ex => ex.SearchParameters)
                .Excluding(ex => ex.UserRef)
            );
        
        capturedDateTime.Should().NotBeNull();
        capturedDateTime.Should().Be(query.LastRunDateFilter.AddDays(1).Date);
    }
}